using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

#if ENABLE_WINMD_SUPPORT
#if WINDOWS_UWP
    using System;
    using System.Threading;
    using System.Text;
    using System.Collections.Concurrent;
    using Windows.Security.Cryptography;
    using Windows.Devices.Bluetooth;
    using Windows.Devices.Enumeration;
    using Windows.Devices.Bluetooth.GenericAttributeProfile;
    using Windows.Foundation;
    using Windows.Storage.Streams;
#endif
#endif

public class IMUControl : MonoBehaviour
{

#if ENABLE_WINMD_SUPPORT
    private DeviceWatcher deviceWatcher;
    
    private Dictionary<Guid, ConcurrentQueue<IBuffer>> dataBytesQueues = new Dictionary<Guid, ConcurrentQueue<IBuffer>>();
    private Dictionary<Guid, ConcurrentQueue<IBuffer>> processingBytesQueues = new Dictionary<Guid, ConcurrentQueue<IBuffer>>();
    private Dictionary<Guid, GameObject> controlledObjects = new Dictionary<Guid, GameObject>();
    private Dictionary<Guid, Guid> characteristicToService = new Dictionary<Guid, Guid>();
    private ConcurrentQueue<GattCharacteristic> characteristics = new ConcurrentQueue<GattCharacteristic>();
    private ConcurrentQueue<GattDeviceService> services = new ConcurrentQueue<GattDeviceService>();
    private BluetoothLEDevice btdev;
#endif

    public string[] GUIDs;
    public bool calibrated = false;
    public IMUDiagnostics diagnostics;

    private Quaternion originalRotation;
    private Quaternion currentMatrix;
    private float rotX;
    private float rotY;
    private float rotZ;
    private float rotW;
    private Quaternion calibration;
    private AudioSource soundFX;
    private GameObject pivot;
    private bool establishedConnection = false;
    private static System.Threading.ReaderWriterLockSlim rwl;

    private static class Constants
    {
        public const string Rotate = "rotate-icon";
        public const string Reset = "reset-icon";
        public const string ControlUI = "ControlsUI";
        public const string PelvisPartsName = "PelvisParts";
    }


    private void Start()
    {
        //rotateButton = GameObject.Find(Constants.Rotate);
        soundFX = gameObject.GetComponent<AudioSource>();

#if ENABLE_WINMD_SUPPORT
        rwl = new System.Threading.ReaderWriterLockSlim();
#endif
        /*
#if ENABLE_WINMD_SUPPORT
        foreach(string id in GUIDs) {
            Guid guid = new Guid(id);
            dataBytesQueues.Add(guid, new ConcurrentQueue<IBuffer>());
            processingBytesQueues.Add(guid, new ConcurrentQueue<IBuffer>());
            try {
                GameObject go = GameObject.Find(id);
                Debug.Log("[IMUControl] Found GameObject:" + go.name);
                controlledObjects.Add(guid, go);
            } catch(NullReferenceException e) {
                Debug.Log("[IMUControl] Could not find GameObject");
            }
        }
#endif*/
        pivot = GameObject.Find("AnneHead"); //TEMPORARY, refactor to make this a variable
        
        if (!calibrated)
        {
            /*pelvisParts.GetComponent<ResetState>().ResetEverything();
            controlsUi.GetComponent<SubMenusManager>().EnableDefaultMenus();
            if (GameObject.Find(Constants.Reset) != null)
            {
                GameObject.Find(Constants.Reset).GetComponent<ResetButtonAction>().ResetUIButtons();
            }*/
        }

#if ENABLE_WINMD_SUPPORT
        deviceWatcher = null;
        StartBleDeviceWatcher();
#endif
    }

    private void Update()
    {
        if (calibrated && establishedConnection)
        {
            if(rwl != null) {
                rwl.EnterReadLock();
                try
                {
                    currentMatrix = new Quaternion(rotY, -rotZ, -rotX, rotW);
                    currentMatrix = calibration * currentMatrix;
                    if (pivot.transform.localRotation != currentMatrix)
                    {
                        pivot.transform.localRotation = currentMatrix;
                    }
                }
                finally
                {
                    rwl.ExitReadLock();
                }
            }
        }
    }

    public void ResetIMU()
    {
        calibrated = false;
    }

    public void IMUCalibrated()
    {
        if(rwl != null) { 
            rwl.EnterReadLock();
            try
            {
                calibration = new Quaternion(rotY, -rotZ, -rotX, rotW);
                calibration = Quaternion.Inverse(Quaternion.RotateTowards(pivot.transform.localRotation, calibration, 360f));
                calibrated = true;
                if (diagnostics != null)
                {
                    diagnostics.UpdateState("Calibrated");
                    diagnostics.UpdateX("c: " + calibration.x);
                    diagnostics.UpdateY("c: " + calibration.y);
                    diagnostics.UpdateZ("c: " + calibration.z);
                    diagnostics.UpdateW("c: " + calibration.w);
                }
            } finally
            {
                rwl.ExitReadLock();
            }
        }
    }

#if ENABLE_WINMD_SUPPORT
    IEnumerator WaitForDataProcessing()
    {
        InsertToProcessingQueue();
        ParseCharacteristicsForTransform();
        yield return null;
    }

    private void UpdateTransformFromGatt()
    {
        InsertToProcessingQueue();
        ParseCharacteristicsForTransform();
    }

    private void InsertToProcessingQueue()
    {
        IBuffer data;
        foreach(KeyValuePair<Guid, ConcurrentQueue<IBuffer>> currentItem in dataBytesQueues) {  
           try {
                if (currentItem.Value.TryDequeue(out data))
                {
                    processingBytesQueues[currentItem.Key].Enqueue(data);
                }
            } catch(Exception e) {
                Debug.Log("processing queue does not exist: " + currentItem.Key.ToString());
            }
        }
    }

    private void ParseCharacteristicsForTransform()
    {
        IBuffer dataToProcess;
        foreach(KeyValuePair<Guid, ConcurrentQueue<IBuffer>> currentItem in processingBytesQueues) {
            if(currentItem.Value.TryDequeue(out dataToProcess)) 
            {
                var reader = DataReader.FromBuffer(dataToProcess);

                var byteData = new byte[reader.UnconsumedBufferLength];
                CryptographicBuffer.CopyToByteArray(dataToProcess, out byteData);

                var parsedCharacteristics = Encoding.UTF8.GetString(byteData);
                //Debug.Log("Parsed string: " + parsedCharacteristics);

                string[] characteristicsToUpdate = new string[4];
                Array.Copy(parsedCharacteristics.Split(','), 1, characteristicsToUpdate, 0, 4);
                //Debug.Log("Characteristics to update: " + characteristicsToUpdate.Length);

                //Debug.Log("CharX: " + characteristicsToUpdate[0]);
                //Debug.Log("CharY: " + characteristicsToUpdate[1]);
                //Debug.Log("CharZ: " + characteristicsToUpdate[2]);
                //Debug.Log("CharW: " + characteristicsToUpdate[3]);

                rotX = (float)(Convert.ToDouble(characteristicsToUpdate[0]) * (1.0 / 100.0));
                rotY = (float)(Convert.ToDouble(characteristicsToUpdate[1]) * (1.0 / 100.0));
                rotZ = (float)(Convert.ToDouble(characteristicsToUpdate[2]) * (1.0 / 100.0));
                rotW = (float)(Convert.ToDouble(characteristicsToUpdate[3]) * (1.0 / 100.0));

                PerformRotationTransformation(currentItem.Key);
            }
        }
    }

    private void PerformRotationTransformation(Guid guid)
    {
        GameObject go = controlledObjects[guid];
        if (calibrated)
        {
            //Debug.Log("performing IMU transformations on : " + guid.ToString());

            // The co-ordinates are mixed around due to the orientation of the IMU in the 3D printed model.
            Quaternion currentMatrix = new Quaternion(rotY, -rotZ, -rotX, rotW);
            currentMatrix = calibration * currentMatrix;
            go.transform.localRotation = currentMatrix;
    /*
                //diagnostics.UpdateX("" + currentMatrix.x);
                //diagnostics.UpdateY("" + currentMatrix.y);
                //diagnostics.UpdateZ("" + currentMatrix.z);
                //diagnostics.UpdateW("" + currentMatrix.w);*/
            //pelvisParts.GetComponent<RotateStructures>().ResetRotation();
            //rotateButton.GetComponent<ButtonSwapFeedback>().ResetButtonState();
            //rotateButton.SetActive(false);
        }
        else
        {
            //rotateButton.SetActive(true);
            //go.transform.localEulerAngles = new Vector3(0, 0, 0); 
        }
    }
#endif

#if ENABLE_WINMD_SUPPORT
#if WINDOWS_UWP
    private void StartBleDeviceWatcher()
    {
        string[] requestedProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected" };
        deviceWatcher = DeviceInformation.CreateWatcher(
                    BluetoothLEDevice.GetDeviceSelectorFromPairingState(true),
                    requestedProperties,
                    DeviceInformationKind.AssociationEndpoint);

        Debug.Log("Starting the BLE Device Watcher");

        deviceWatcher.Added += DeviceWatcher_Added;
        deviceWatcher.Removed += DeviceWatcher_Removed;
        deviceWatcher.Start();
        //diagnostics.UpdateState("DeviceWatcherStarted");
    }

    private void StopBleDeviceWatcher()
    {
        Debug.Log("Stop BLE Device Watcher");

        if (deviceWatcher != null)
        {
            deviceWatcher.Added -= DeviceWatcher_Added;
            deviceWatcher.Stop();
            deviceWatcher = null;
        }
    
        //diagnostics.UpdateState("DeviceWatcherStopped");
    }

    private async void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation args)
    {
        
        btdev = await BluetoothLEDevice.FromIdAsync(args.Id);
        Debug.Log("This is the argsID " + args.Id);

        GattDeviceServicesResult result = await btdev.GetGattServicesAsync();
        Debug.Log("Gatt Device Service Result has been found!!");
    
        //diagnostics.UpdateState("ServicesFound");

        foreach (GattDeviceService service in result.Services)
        {
            //Debug.Log("Service Found, uuid: " + service.Uuid);
            
            foreach(string id in GUIDs) {

                Guid customGuid = new Guid(id);
                if (customGuid == service.Uuid)
                {
                    //Debug.Log("GUID matches UUID");

                        //diagnostics.UpdateState("ServiceMatch");

                    services.Enqueue(service);
                    GattCharacteristicsResult gattResult = await service.GetCharacteristicsAsync();
                    //Debug.Log("Characteristic UUID: " + gattResult.Characteristics[0].Uuid);
                    characteristicToService.Add(gattResult.Characteristics[0].Uuid, service.Uuid); 
                    var communicationEstablished = false;
                    while(!communicationEstablished) {
                        btdev = await BluetoothLEDevice.FromIdAsync(args.Id);
                        var status = await gattResult.Characteristics[0].WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
                        if(status == 0) {
                            gattResult.Characteristics[0].ValueChanged += Characteristic_ValueChanged();
                            //Debug.Log("Characteristic Properties: " + gattResult.Characteristics[0].CharacteristicProperties.ToString());
                            characteristics.Enqueue(gattResult.Characteristics[0]);
                            communicationEstablished = true;

                                //diagnostics.UpdateState("CommunicationEstablished");

                        } else {
                            Debug.Log("failed to get configuration: " + status);

                                //diagnostics.UpdateState("RetryingCommunication");

                        }
                    }
                }
            }
            establishedConnection = true;
        }
    }

    private async void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
    {
        Debug.Log("Device disconnected: " + args.Id);
    }

    private TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs> Characteristic_ValueChanged()
    {
        return new TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs> (
            delegate(GattCharacteristic sender, GattValueChangedEventArgs args)
            {
                //Debug.Log("The characteristic value has been changed");

                var reader = DataReader.FromBuffer(args.CharacteristicValue);

                var byteData = new byte[reader.UnconsumedBufferLength];
                CryptographicBuffer.CopyToByteArray(args.CharacteristicValue, out byteData);

                var parsedCharacteristics = Encoding.UTF8.GetString(byteData);
                //Debug.Log("Parsed string: " + parsedCharacteristics);

                string[] characteristicsToUpdate = new string[4];
                Array.Copy(parsedCharacteristics.Split(','), 1, characteristicsToUpdate, 0, 4);

                rwl.EnterWriteLock();
                try {
                    Debug.Log("updating rots");
                    rotX = (float)(Convert.ToDouble(characteristicsToUpdate[0]) * (1.0 / 100.0));
                    rotY = (float)(Convert.ToDouble(characteristicsToUpdate[1]) * (1.0 / 100.0));
                    rotZ = (float)(Convert.ToDouble(characteristicsToUpdate[2]) * (1.0 / 100.0));
                    rotW = (float)(Convert.ToDouble(characteristicsToUpdate[3]) * (1.0 / 100.0));
                } finally {
                    rwl.ExitWriteLock();
                }

                
    /*
                ConcurrentQueue<IBuffer> current;
                if(dataBytesQueues.TryGetValue(characteristicToService[sender.Uuid], out current)) {
                    current.Enqueue(args.CharacteristicValue);
                }
    */
            });
    }
#endif
#endif
}