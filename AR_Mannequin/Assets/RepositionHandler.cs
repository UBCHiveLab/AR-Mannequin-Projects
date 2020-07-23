using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepositionHandler : MonoBehaviour
{
    public Button lockButton;
    public GameObject mannekin;
    public GameObject planeFinder;

    public void EnablePlaneFinder()
    {   
        mannekin.transform.localPosition = new Vector3(0,0,0);
        mannekin.transform.localScale = new Vector3(1,1,1);
        lockButton.GetComponent<LockButtonHandler>().UnlockInteraction();
        planeFinder.SetActive(true);
    }
}
