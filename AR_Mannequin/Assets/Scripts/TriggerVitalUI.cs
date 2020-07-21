using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerVitalUI : MonoBehaviour
{
    public List<Vitals> TrigggeredVitals;
    public UserPosition userPosition;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("MainCamera"))
    //    {
    //        VitalsManager.Instance.TurnOffAllVitalUI();
    //        VitalsManager.Instance.TurnOnVitalsUI(TrigggeredVitals);
    //        Debug.Log("entered" + userPosition.ToString());
    //        // missing code for telling the server where this user is at
    //    }
       
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("MainCamera"))
    //        VitalsManager.Instance.TurnOffAllVitalUI();
    //}
}
