﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public enum Vitals{

    EndtidalCO2Detector,
    NasopharyngealTemperatureProbe,
    ECGLeads,
    BloodPressureCuff,
    PulseOximeter

}
//define where the student/user is at in relevant to the manikin
public enum UserPosition
{
    none,
    head,
    chest,
    arm
}

public class VitalsManager : Singleton<VitalsManager>
{
    List<VitalsController> vitalsControllerList;
    public bool isInTimer = false;
    [SerializeField]
    private float countdownTime;
    [SerializeField]
    private Text timerText;

    static Dictionary<UserPosition, Vitals[]> positionVitalPairs=new Dictionary<UserPosition, Vitals[]>();

    private UserPosition currentUserPosition = UserPosition.none;

    void Awake()
    {
        vitalsControllerList = GetComponentsInChildren<VitalsController>().ToList();
        
        timerText.gameObject.SetActive(false);
        
        //Temorarily define the position vital pairs in the begining
        positionVitalPairs.Add(UserPosition.head, new Vitals[] { Vitals.EndtidalCO2Detector, Vitals.NasopharyngealTemperatureProbe });
        positionVitalPairs.Add(UserPosition.chest, new Vitals[] { Vitals.ECGLeads });
        positionVitalPairs.Add(UserPosition.arm, new Vitals[] { Vitals.BloodPressureCuff, Vitals.PulseOximeter });
        Button[] vitalButtons = this.GetComponentsInChildren<Button>();
        foreach (Button vitalButton in vitalButtons)
        {
            vitalButton.onClick.AddListener(()=> OnVitalButtonClick(vitalButton));
           
            
        }
        Toggle[] vitalToggles = this.GetComponentsInChildren<Toggle>();
        foreach (Toggle vitalToggle in vitalToggles)
        {
            vitalToggle.onValueChanged.AddListener((bool a) => OnVitalButtonClick(null, vitalToggle));

        }
        TurnOffAllVitalUI();
    }
    public void VitalsUIControlBasedOnUserPosition(UserPosition userPosition)
    {
        Debug.Log(userPosition);
        if (userPosition == UserPosition.none)
        {
            TurnOffAllVitalUI();
        }
        else if (currentUserPosition != userPosition)
        {
            Vitals[] vitals = positionVitalPairs[userPosition];
            TurnOffAllVitalUI();
            TurnOnVitalsUI(vitals.ToList());
        }
        currentUserPosition = userPosition;
    }

    #region PRIVATE_METHODS
    private void TurnOffAllVitalUI()
    {
        //deactivate all vitals ui
        vitalsControllerList.ForEach(x => x.gameObject.SetActive(false));
    }
    private void TurnOnVitalsUI(List<Vitals> vitalList)
    {
        //check if vitalsControllerList is empty
        if (vitalsControllerList.Count > 0 && !isInTimer)
        {
            foreach (Vitals vital in vitalList)
            {
                vitalsControllerList.Find(x => x.vital == vital).gameObject.SetActive(true);
            }
        }
    }
    private void TurnOnCurrentVital()
    {
        if (currentUserPosition != UserPosition.none)
        {
            Vitals[] vitals = positionVitalPairs[currentUserPosition];
            TurnOnVitalsUI(vitals.ToList());
        }
    }
    #endregion


    #region TIMER_RELATED_METHODS
    public void OnVitalButtonClick(Button vitalButton = null, Toggle vitalToggle = null)
    {
        StartCoroutine(Timer(vitalButton, vitalToggle));
        Debug.Log(vitalButton+"is clicked");
    }
    private IEnumerator Timer(Button vitalButton = null, Toggle vitalToggle = null)
    {
        NetworkUI networkUI;
        timerText.gameObject.SetActive(true);
        float countdown = countdownTime;
        isInTimer = true;
        
        TurnOffAllVitalUI();

        Debug.Log("Timer Starting");
        while (countdown > 0)
        {
            timerText.text = FloatToTime(countdown, "00.00");
            countdown -= Time.deltaTime;
            yield return null;
        }
        isInTimer = false;
        timerText.gameObject.SetActive(false);
        if (vitalButton != null)
        {
            networkUI = vitalButton.GetComponent<NetworkUI>();
            if (networkUI!=null)
            {
                networkUI.ChangeToggleValue(true);
            }
            ButtonGrayOut.DisableButton(vitalButton);
        }
        else if (vitalToggle != null)
        {
            networkUI = vitalToggle.GetComponent<NetworkUI>();
            if (networkUI != null)
            {
                networkUI.ChangeToggleValue();
            }
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
