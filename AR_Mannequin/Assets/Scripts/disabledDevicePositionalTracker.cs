using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_WSA
using Vuforia;

public class disabledDevicePositionalTracker : MonoBehaviour {

    //https://developer.vuforia.com/forum/model-targets/extended-tracking-enable-even-if-track-device-pose-disabled?sort=2
    //Just attach this to initializer to prevent weird stuff with tracking

    PositionalDeviceTracker deviceTracker;

    void Awake()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaInitialized);
    }

    void OnVuforiaInitialized()
    {
        StartCoroutine(StopTracker());
    }

    private IEnumerator StopTracker()
    {
        deviceTracker = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();

        while (deviceTracker.IsActive == false)
            yield return new WaitForEndOfFrame();
        deviceTracker.Stop();
    }

    /*
    PositionalDeviceTracker pdt;

    // Use this for initialization
    void Start () {
        pdt = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();

        Debug.Log("1");

    }

    // Update is called once per frame
    void Update () {

        Debug.Log("2");

        pdt.Stop();

        if (pdt != null && pdt.IsActive)
        {
            pdt.Stop();
            Debug.Log("3");
        }
    }
    */
}
#endif