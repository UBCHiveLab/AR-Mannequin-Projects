using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyOverlayOnOff : Singleton<ApplyOverlayOnOff> {

    Dictionary<string, Renderer> objectsToToggle;

	// Use this for initialization
	void Start () {
        //objectsToToggle = Parse.Instance.ToggleOnOff;
        //EventManager.Instance.InteractionOpacityEvent += OnInteractionOpacityEvent;
	}

    //need opacity in materials to do degrees of transparency
    private void OnInteractionOpacityEvent(string name, float opacity)
    {
        if (objectsToToggle.ContainsKey(name))
        {
            if (opacity > 0.5f)
            {
                objectsToToggle[name].enabled = true;
            } else
            {
                objectsToToggle[name].enabled = false;
            }
        }
    }
}
