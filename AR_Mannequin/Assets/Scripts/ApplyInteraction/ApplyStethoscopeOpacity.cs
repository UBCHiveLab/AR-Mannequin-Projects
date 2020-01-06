using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using System;

public class ApplyStethoscopeOpacity: Singleton<ApplyStethoscopeOpacity> {
    
    string MagicStick = "MagicStickRef";
    Transform magicStickRef;
    Renderer[] stethoscopeRenderers;

    protected override void Awake()
    {
        base.Awake();
        EventManager.Instance.InteractionOpacityEvent += OnInteractionOpacityEvent;
    }

    void Start()
    {
        stethoscopeRenderers = GameObject.Find("stethoscope-tip").GetComponentsInChildren<Renderer>();
        magicStickRef = GameObject.Find("MagicStickRef").transform;
    }

    private void OnInteractionOpacityEvent(string name, float opacity)
    {
        if (name == MagicStick)
        {
            if (opacity > 0)
            {
                Debug.Log("reparenting");
                foreach(Renderer rend in stethoscopeRenderers)
                {
                    rend.enabled = true;
                }
                stethoscopeRenderers[0].transform.parent.SetParent(magicStickRef);
                stethoscopeRenderers[0].transform.parent.localPosition = new Vector3(0, 0, 0);
                stethoscopeRenderers[0].transform.parent.localRotation = Quaternion.identity;
            }
            else
            {
                foreach(Renderer rend in stethoscopeRenderers)
                {
                    rend.enabled = false;
                }
                stethoscopeRenderers[0].transform.parent.parent = null;
                stethoscopeRenderers[0].transform.parent.position = new Vector3(1000, 1000, 1000);
            }
        }
    }
}
