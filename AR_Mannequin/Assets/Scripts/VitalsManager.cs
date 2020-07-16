using System.Collections;
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

    void Awake()
    {
        vitalsControllerList = GetComponentsInChildren<VitalsController>().ToList();
        TurnOffAllVitalUI();
        timerText.gameObject.SetActive(false);
    }

    public void TurnOffAllVitalUI()
    {
        //deactivate all vitals ui
        vitalsControllerList.ForEach(x => x.gameObject.SetActive(false));
    }
    public void TurnOnVitalsUI(List<Vitals> vitalList)
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
    public void OnVitalButtonClick()
    {
        StartCoroutine(Timer());
    }
    private IEnumerator Timer()
    {
        timerText.gameObject.SetActive(true);
        float countdown = countdownTime;
        isInTimer = true;
        while (countdown > 0)
        {
            timerText.text = FloatToTime(countdown, "00.0");
            yield return null;
        }
        isInTimer = false;
        timerText.gameObject.SetActive(false);
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
}
