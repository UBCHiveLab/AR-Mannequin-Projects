using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStickImageTrackableEventHandler : DefaultTrackableEventHandler
{
    Collider reference_col;
    Renderer reference_ren;

    void Awake()
    {
        GameObject reference = GameObject.Find("MagicStickRef");
        //reference_col = reference.GetComponent<Collider>();
        //reference_ren = reference.GetComponent<Renderer>();
    }

    protected override void OnTrackingFound()
    {
        //reference_col.enabled = true;
        //reference_ren.enabled = true;
        Debug.Log("before publish vuf");
        //EventManager.Instance.publishGeneralVuforiaEvent(this.gameObject.name /* "MagicStick" */, true);
        EventManager.Instance.publishGeneralVuforiaEvent("MagicStickRef", true);
        Debug.Log("after publish vuf");
    }

    protected override void OnTrackingLost()
    {
        //reference_col.enabled = false;
        //reference_ren.enabled = false;
        //EventManager.Instance.publishGeneralVuforiaEvent(this.gameObject.name /* "MagicStick" */, false);
        EventManager.Instance.publishGeneralVuforiaEvent("MagicStickRef", false);
    }
}
