using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchObjectsListener : MonoBehaviour
{
    // can be inherited by stuff. where changes depends on what behaviour is wanted

    protected GameObject[] Elements;

    // This doesn't work because button onclick doesn't work

    // Note that button order must match element order
    // EX Education room button and UI canvas must both be in same index for both list
    // (ex: both at index 0)

    virtual protected void Awake()
    {
        var numChildren = 0;
        foreach(Transform child in transform)
        {
            numChildren++;
        }
        Elements = new GameObject[numChildren];
        for(int i = 0; i < numChildren; i++)
        {
            Elements[i] = transform.GetChild(i).gameObject;
            inactiveBehaviour(Elements[i]);
        }
        activeBehaviour(Elements[0]);

        EventManager.Instance.ButtonSwitchEvent += SwitchToElement;
    }

    // TODO will break with more than 1 instance of this code due to name comparing
    private void SwitchToElement(string name, string group)
    {
        if (this.name == group)
        {
            foreach (GameObject element in Elements)
            {
                if (element.name != name)
                {
                    inactiveBehaviour(element);
                }
                else
                {
                    activeBehaviour(element);
                }
                /*
                foreach(Renderer cur in element.GetComponentsInChildren<Renderer>())
                {
                    if (element.name != name)
                    {
                        cur.enabled = false;
                    }
                    else
                    {
                        cur.enabled = true;
                    }
                }
                */
            }
        }
    }

    // default behaviour
    //adding collider reset for the hand tool
    virtual protected void activeBehaviour(GameObject go)
    {
        go.SetActive(true);
    }

    // default behaviour
    virtual protected void inactiveBehaviour(GameObject go)
    {
        go.SetActive(false);
    }
}
