using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Kimberly Burke 2019
/// 
/// Connect Calibrate toggle element to ModelBehavior4.SetCalibrate() as EventTrigger
/// </summary>
public class OverlayUIManager : MonoBehaviour
{
    TRACKINGSTATE currentTrackingState;
    enum TRACKINGSTATE { NO, YES }
    [SerializeField] Toggle[] imuToggles;
    [SerializeField] Text calibrateText; 

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.MainManikinVuforiaEvent += OnIMUDetected;
        EventManager.Instance.ManikinPositionedEvent += OnManikinPositioned;
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    /// <summary>
    /// Sets the toggles corresponding to the vuforia detection event
    /// </summary>
    /// <param name="foundOrLost">YES or NO</param>
    /// <param name="modelType"></param>
    /// <param name="updatedParentTransform"></param>
    private void OnIMUDetected(string foundOrLost, string modelType, Transform updatedParentTransform)
    {
        bool tracked = false;
        string name = updatedParentTransform.parent.gameObject.name;
        if (foundOrLost == "YES")
        {
            tracked = true;
        }
        int index = -1;
        switch (name)
        {
            case "8":
                index = 0;
                break;
            case "5":
                index = 1;
                break;
            case "7":
                index = 2;
                break;
            case "1":
                index = 3;
                break;
            default:
                break;
        }
        if (index != -1) { imuToggles[index].isOn = tracked; }
    }

    /// <summary>
    /// Sets all toggles to false.
    /// </summary>
    private void ResetIMUToggles()
    {
        foreach(Toggle imu in imuToggles)
        {
            imu.isOn = false;
        }
        calibrateText.gameObject.SetActive(false);
    }

    private void OnManikinPositioned(bool positioned)
    {
        if (!positioned)
        {
            ResetIMUToggles();
        }
        // Hides/displays all UI elements
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(!positioned);
        }
        // Play small animation when successfully animated
        if (positioned)
        {
            calibrateText.gameObject.SetActive(true);
            calibrateText.GetComponent<Animation>().Play();
        }
    }
}
