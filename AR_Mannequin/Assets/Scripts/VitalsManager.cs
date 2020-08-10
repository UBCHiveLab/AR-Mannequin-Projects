using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

//public enum Vitals{

//    EndtidalCO2Detector,
//    TakeTemperature,
//    ECGLeads,
//    BloodPressureCuff,
//    PulseOximeter,
//    StartIV,
//    AbdominalExam,
//    InsertLO,
//    LungSounds,
//    HeartSounds,
//    ChestCompression,
//    O2NP,
//    O2Facemask,
//    BVM,
//    O2Sat,
//    IVMedication,
//    IMMedication,
//    DrawBlood,
//    Glucose

//}
//define where the student/user is at in relevant to the manikin
public enum UserPosition
{
    none,
    head,
    chest,
    arm,
    bottom
}

public class VitalsManager : Singleton<VitalsManager>
{
    List<VitalsController> vitalsControllerList;
    List<VitalButtonGroupController> vitalButtonGroupControllers;
    public bool isInTimer = false;
    private bool isVitalUIOn = false;
    [SerializeField]
    private Text timerText;

    //static Dictionary<UserPosition, Vitals[]> positionVitalPairs=new Dictionary<UserPosition, Vitals[]>();

    private UserPosition currentUserPosition = UserPosition.none;

    void Awake()
    {
        vitalsControllerList = GetComponentsInChildren<VitalsController>().ToList();
        vitalButtonGroupControllers = GetComponentsInChildren<VitalButtonGroupController>().ToList();
        foreach (VitalsController vitalsController in vitalsControllerList)
        {
            Debug.Log(vitalsController.name);
            vitalsController.AddListener(() => OnVitalButtonClick(vitalsController));
            vitalsController.AddListener((bool a) => OnVitalButtonClick(vitalsController));
        }

        TurnOffAllVitalUI();

        timerText.gameObject.SetActive(false);
        
    }
    public void VitalsUIControlBasedOnUserPosition(UserPosition userPosition)
    {

        if (userPosition == UserPosition.none)
        {
            TurnOffAllVitalUI();
        }

        if (currentUserPosition != userPosition)
        {
            Debug.Log(userPosition);
            currentUserPosition = userPosition;
            TurnOffAllVitalUI();
            if(isVitalUIOn)
            TurnOnCurrentVital();
            
        }
        
    }
    public void ToggleVitalUI(bool isOn)
    {
        if (isOn)
        {
            TurnOnCurrentVital();
        }
        else
        {
            TurnOffAllVitalUI();
        }
        isVitalUIOn = isOn;
    }

    #region PRIVATE_METHODS
    private void TurnOffAllVitalUI()
    {
        //deactivate all vitals ui
        vitalButtonGroupControllers.ForEach(x => x.gameObject.SetActive(false));
    }
    private void TurnOnCurrentVital()
    {
        if (currentUserPosition != UserPosition.none && !isInTimer)
        {
            vitalButtonGroupControllers.Find(x => x.correspondingUserPosition == currentUserPosition).gameObject.SetActive(true);
        }
    }
    
    #endregion


    #region TIMER_RELATED_METHODS
    public void OnVitalButtonClick(VitalsController vitalsController)
    {
        StartCoroutine(Timer(vitalsController));
        Debug.Log(vitalsController);
    }
    private IEnumerator Timer(VitalsController vitalsController)
    {
        
        timerText.gameObject.SetActive(true);
        
        float countdown = vitalsController.countdownTime;
        
        isInTimer = true;
        
        TurnOffAllVitalUI();
        vitalsController.InvokeActionBeforeTimer();
        Debug.Log("Timer Starting");
        while (countdown > 0)
        {
            timerText.text = FloatToTime(countdown, "00.00");
            countdown -= Time.deltaTime;
            yield return null;
        }
        isInTimer = false;
        timerText.gameObject.SetActive(false);
        if (vitalsController != null)
        {
            vitalsController.InvokeActionAfterTimer();
        }
        
        TurnOnCurrentVital();
    }
    public string FloatToTime(float toConvert, string format)
    {
        switch (format)
        {
            case "00.0":
                return string.Format("{0:00}:{1:0}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 10) % 10));//miliseconds
            case "#0.0":
                return string.Format("{0:#0}:{1:0}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 10) % 10));//miliseconds
            case "00.00":
                return string.Format("{0:00}:{1:00}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 100) % 100));//miliseconds
            case "00.000":
                return string.Format("{0:00}:{1:000}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
            case "#00.000":
                return string.Format("{0:#00}:{1:000}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 1000) % 1000));//miliseconds

            case "#0:00":
                return string.Format("{0:#0}:{1:00}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60);//seconds

            case "#00:00":
                return string.Format("{0:#00}:{1:00}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60);//seconds

            case "0:00.0":
                return string.Format("{0:0}:{1:00}.{2:0}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 10) % 10));//miliseconds
            case "#0:00.0":
                return string.Format("{0:#0}:{1:00}.{2:0}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 10) % 10));//miliseconds
            case "0:00.00":
                return string.Format("{0:0}:{1:00}.{2:00}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 100) % 100));//miliseconds
            case "#0:00.00":
                return string.Format("{0:#0}:{1:00}.{2:00}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 100) % 100));//miliseconds
            case "0:00.000":
                return string.Format("{0:0}:{1:00}.{2:000}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
            case "#0:00.000":
                return string.Format("{0:#0}:{1:00}.{2:000}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
        }
        return "error";
    }
    #endregion
}
