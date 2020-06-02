using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SortOverlays : Singleton<SortOverlays> {

    Dictionary<string, float> currentStatus;

	// Use this for initialization
	void Start () {
        /*
        currentStatus = new Dictionary<string, float>();
        List<string> names = Parse.Instance.ToggleOnOff.Keys.ToList();
        foreach (string n in names)
        {
            if(Parse.Instance.ToggleOnOff[n].enabled)
            {
                currentStatus.Add(n, 1f);
                EventManager.Instance.publishInteractionOpacityEvent(n, 1f);
            } else
            {
                currentStatus.Add(n, 0f);
                EventManager.Instance.publishInteractionOpacityEvent(n, 0f);
            }
        }
        EventManager.Instance.ButtonToggleEvent += OnButtonToggleEvent;
        */
	}

    private void OnButtonToggleEvent(string s)
    {
        if (currentStatus.ContainsKey(s))
        {
            if (currentStatus[s] == 0f)
            {
                currentStatus[s] = 1f;
            } else
            {
                currentStatus[s] = 0f;
            }

            EventManager.Instance.publishInteractionOpacityEvent(s, currentStatus[s]);
        }
    }
}
