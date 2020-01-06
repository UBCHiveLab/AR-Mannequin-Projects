using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchObjectsListenerWithTracking : SwitchObjectsListener {

    string MagicStick = "MagicStickRef";
    Transform magicStickRef;
    GameObject currentObject;
    bool trackingStatus;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        trackingStatus = false;
        magicStickRef = GameObject.Find("MagicStick").transform;
        currentObject = Elements[0];
        EventManager.Instance.InteractionOpacityEvent += OnInteractionOpacityEvent;
    }

    private void OnInteractionOpacityEvent(string name, float opacity)
    {
        if (name == MagicStick)
        {
            Renderer[] currentRenderer = currentObject.GetComponentsInChildren<Renderer>();
            if (opacity > 0)
            {
                Debug.Log("reparenting");
                foreach (Renderer rend in currentRenderer)
                {
                    rend.enabled = true;
                }
                currentRenderer[0].transform.parent.SetParent(magicStickRef);
                currentRenderer[0].transform.parent.localPosition = new Vector3(0, 0, 0);
                currentRenderer[0].transform.parent.localRotation = Quaternion.identity;
            }
            else
            {
                foreach (Renderer rend in currentRenderer)
                {
                    rend.enabled = false;
                }
                currentRenderer[0].transform.parent.parent = null;
                currentRenderer[0].transform.parent.position = new Vector3(1000, 1000, 1000);
            }
        }
    }

    protected override void activeBehaviour(GameObject go)
    {
        currentObject = go;   
    }

    protected override void inactiveBehaviour(GameObject go)
    {
        // do nothing
    }

}
