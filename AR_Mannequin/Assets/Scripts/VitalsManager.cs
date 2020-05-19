using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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


    void Awake()
    {

        vitalsControllerList = GetComponentsInChildren<VitalsController>().ToList();
        TurnOffAllVitalUI();
    }

    public void TurnOffAllVitalUI()
    {
        //deactivate all vitals ui
        vitalsControllerList.ForEach(x => x.gameObject.SetActive(false));
    }
    public void TurnOnVitalsUI(List<Vitals> vitalList)
    {
        //check if vitalsControllerList is empty
        if (vitalsControllerList.Count > 0)
        {
            foreach (Vitals vital in vitalList)
            {
                vitalsControllerList.Find(x => x.vital == vital).gameObject.SetActive(true);
            }
        }


    }
}
