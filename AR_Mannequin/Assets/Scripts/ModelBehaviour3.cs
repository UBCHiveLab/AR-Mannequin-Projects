using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class ModelBehaviour3 : MonoBehaviour {

    //Most updated ModelBehaviour. Attach to surface level GO that has all relevant GOs as children.
    //works with customtrackabledeventhandler
    
    Transform modelObjectTransform;

    Transform refModelTransform;
    Transform refImageTransform;
    Transform refMountTransform;
    bool modelState;
    bool imageState;
    bool mountState;
    TRACKINGTYPE currentTrackingType;
    enum TRACKINGTYPE { MODEL, IMAGE, MOUNT, NONE }
    TRACKINGSTATE currentTrackingState;
    enum TRACKINGSTATE { NO, YES }
    private Dictionary<Transform, bool> imageTargets;

    private IMUControl imu;

    StringBuilder update;

    // Use this for initialization

    private void Awake()
    {
    }

    void Start()
    {
        modelObjectTransform = this.GetComponent<Transform>();

        modelState = false;
        imageState = false;
        mountState = false;

        currentTrackingType = TRACKINGTYPE.NONE;
        currentTrackingState = TRACKINGSTATE.NO;

        imu = GameObject.Find("IMUControl").GetComponent<IMUControl>();

        imageTargets = new Dictionary<Transform, bool>();

        foreach (Transform child in GameObject.Find("ImageTargets").transform)
        {
            imageTargets.Add(child.GetChild(0), false);
            Debug.Log("image target: " + child.GetChild(0).GetHashCode() + " " + child.GetChild(0).name);
        }

        Debug.Log("img targets total: " + imageTargets.Count);
        Debug.Log("added image targets");

        EventManager.Instance.MainManikinVuforiaEvent += OnVuforiaModelEvent;
    }

    public void OnVuforiaModelEvent(string foundOrLost, string modelType, Transform updatedParentTransform)
    {
        //Debug.Log("found?: " + foundOrLost + " , type: " + modelType);

        TRACKINGTYPE updatedTrackingType = (TRACKINGTYPE)Enum.Parse(typeof(TRACKINGTYPE), modelType);
        TRACKINGSTATE updatedTrackingState = (TRACKINGSTATE)Enum.Parse(typeof(TRACKINGSTATE), foundOrLost);

        if (updatedTrackingState == TRACKINGSTATE.YES)
        {
            switch (updatedTrackingType)
            {
                case TRACKINGTYPE.MODEL:
                    modelState = true;
                    refModelTransform = updatedParentTransform;
                    parent(refModelTransform);
                    currentTrackingType = updatedTrackingType;
                    break;
                case TRACKINGTYPE.IMAGE:
                    imageTargets[updatedParentTransform] = true;
                    Debug.Log("img targets total: " + imageTargets.Count);
                    bool allImagesTracked = true;
                    foreach(KeyValuePair<Transform, bool> cur in imageTargets)
                    {
                        Debug.Log("img target: " + cur.Key.GetHashCode() + ", tracking status: " + cur.Value.ToString());
                        if(!cur.Value)
                        {
                            allImagesTracked = false;
                        }
                    }
                    Debug.Log("all image tracked?: " + allImagesTracked.ToString());
                    refImageTransform = updatedParentTransform;
                    if (!modelState && allImagesTracked)
                    {
                        parent(refImageTransform);
                        currentTrackingType = updatedTrackingType;
                    }
                    break;
                case TRACKINGTYPE.MOUNT:
                    mountState = true;
                    refMountTransform = updatedParentTransform;
                    if (!modelState && !imageState)
                    {
                        parent(refMountTransform);
                        currentTrackingType = updatedTrackingType;
                    }
                    break;
            }

            if (currentTrackingState == TRACKINGSTATE.NO)
            {
                currentTrackingState = updatedTrackingState;
                //EventManager.Instance.publishRecognitionStateChangedEvent(foundOrLost);
            }
        }
        else if (updatedTrackingState == TRACKINGSTATE.NO)
        {
            switch (updatedTrackingType)
            {
                case TRACKINGTYPE.MODEL:
                    modelState = false;
                    if (imageState)
                    {
                        parent(refImageTransform);
                        currentTrackingType = TRACKINGTYPE.IMAGE;
                    }
                    else if (mountState)
                    {
                        parent(refMountTransform);
                        currentTrackingType = TRACKINGTYPE.MOUNT;
                    }
                    else
                    {
                        unparent();
                        //EventManager.Instance.publishRecognitionStateChangedEvent(foundOrLost);
                        currentTrackingState = updatedTrackingState;
                        currentTrackingType = TRACKINGTYPE.NONE;
                    }
                    break;
                case TRACKINGTYPE.IMAGE:
                    //imageTargets[updatedParentTransform] = false; TODO uncomment this later
                    if (modelState)
                    {
                        parent(refModelTransform);
                        currentTrackingType = TRACKINGTYPE.MODEL;
                    }
                    else if (mountState)
                    {
                        parent(refMountTransform);
                        currentTrackingType = TRACKINGTYPE.MOUNT;
                    }
                    else
                    {
                        unparent();
                        //EventManager.Instance.publishRecognitionStateChangedEvent(foundOrLost);
                        currentTrackingState = updatedTrackingState;
                        currentTrackingType = TRACKINGTYPE.NONE;
                    }
                    break;
                case TRACKINGTYPE.MOUNT:
                    imageState = false;
                    if (modelState)
                    {
                        parent(refModelTransform);
                        currentTrackingType = TRACKINGTYPE.MODEL;
                    }
                    else if (imageState)
                    {
                        parent(refImageTransform);
                        currentTrackingType = TRACKINGTYPE.IMAGE;
                    }
                    else
                    {
                        unparent();
                        //EventManager.Instance.publishRecognitionStateChangedEvent(foundOrLost);
                        currentTrackingState = updatedTrackingState;
                        currentTrackingType = TRACKINGTYPE.NONE;
                    }
                    break;
            }
        }
    }

    //NOTE: this function uses slerp to average rotations and positions, but if more than 2 
    //image targets are used then a more involved average function is needed.
    private void parent(Transform parentTransform)
    {
        Transform target1 = null;
        Transform target2 = null;
        Transform target3 = null;
        Transform target4 = null;
        foreach(KeyValuePair<Transform, bool> cur in imageTargets)
        {
            if(target1 == null)
            {
                target1 = cur.Key;
            } else if(target2 == null)
            {
                target2 = cur.Key;
            } else if(target3 == null)
            {
                target3 = cur.Key;
            } else
            {
                target4 = cur.Key;
            }
        }
        Debug.Log("target 1: " + target1.localPosition.ToString("F3"));
        Debug.Log("target 2: " + target2.localPosition.ToString("F3"));
        Debug.Log("target 3: " + target3.localPosition.ToString("F3"));
        Debug.Log("target 4: " + target4.localPosition.ToString("F3"));
        Vector3 avgPosition12 = Vector3.Lerp(target1.TransformPoint(target1.localPosition), target2.TransformPoint(target2.localPosition), 0.5f);
        Vector3 avgPosition34 = Vector3.Lerp(target3.TransformPoint(target3.localPosition), target4.TransformPoint(target4.localPosition), 0.5f);
        Vector3 avgPosition = Vector3.Lerp(avgPosition12, avgPosition34, 0.5f);  // double lerp, 4 targets
        Quaternion avgRotation12 = Quaternion.Lerp(target1.rotation, target2.rotation, 0.5f);
        Quaternion avgRotation34 = Quaternion.Lerp(target3.rotation, target4.rotation, 0.5f);
        Quaternion avgRotation = Quaternion.Lerp(avgRotation12, avgRotation34, 0.5f);  // double lerp, 4 targets
        modelObjectTransform.SetParent(parentTransform);
        modelObjectTransform.SetPositionAndRotation(avgPosition, avgRotation);
        if (imu != null)
        {
            Debug.Log("parented, calibrating imu");
            imu.IMUCalibrated();
        }
        Debug.Log("model position, position: " + this.gameObject.transform.position.ToString("F3"));
    }

    private void unparent()
    {
        modelObjectTransform.SetParent(null);
        modelObjectTransform.position = new Vector3(0f, 0f, -1f);
        if (imu != null)
        {
            imu.ResetIMU();
        }
    }
}
