using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CustomImageTrackableEventHandler : DefaultTrackableEventHandler
{
    enum TRACKINGTYPE { MODEL, IMAGE, MOUNT, NONE}
    enum TRACKINGSTATE { NO, YES }
    private ModelBehaviour4 modelBehavior;

    override protected void Start()
    {
        base.Start();
        modelBehavior = transform.parent.parent.gameObject.GetComponentInChildren<ModelBehaviour4>();
    }

    protected override void OnTrackingFound()
    {
        Debug.Log("on tracking found");
        Debug.Log("ar pos ref: " + this.gameObject.name + ", hashcode: " + this.transform.Find("arPosRef").transform.GetHashCode() + ", position: " + this.transform.Find("arPosRef").transform.position.ToString("F3"));
        //base.OnTrackingFound(); // to show green man for debugging
        if (this.transform.Find("arPosRef") == null)
        {
            Debug.Log("cant find arPosRef image");
        }
        if (modelBehavior.calibrate)
        {
            GetComponent<AudioSource>().Play();
        }
        EventManager.Instance.publishVuforiaModelEvent(Enum.GetName(typeof(TRACKINGSTATE), TRACKINGSTATE.YES), System.Enum.GetName(typeof(TRACKINGTYPE), TRACKINGTYPE.IMAGE), this.transform.Find("arPosRef"));
    }

    protected override void OnTrackingLost()
    {
        //base.OnTrackingLost();
        EventManager.Instance.publishVuforiaModelEvent(Enum.GetName(typeof(TRACKINGSTATE), TRACKINGSTATE.NO), System.Enum.GetName(typeof(TRACKINGTYPE), TRACKINGTYPE.IMAGE), this.transform.Find("arPosRef"));
    }
}
