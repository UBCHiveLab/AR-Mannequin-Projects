using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSetup : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GameObject go = this.gameObject;
        Rigidbody r = go.GetComponent<Rigidbody>();
        if (r == null)
        {
            r = go.AddComponent<Rigidbody>() as Rigidbody;
        }
        r.useGravity = false;
        r.isKinematic = false;

        BoxCollider c = go.GetComponent<BoxCollider>();
        if (c == null)
        {
            c = go.AddComponent<BoxCollider>() as BoxCollider;
        }
        c.isTrigger = true;

        Renderer re = go.GetComponent<Renderer>();
        if (re != null)
        {
            re.bounds.Encapsulate(c.bounds);
            c.bounds.Encapsulate(re.bounds);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger enter: " + this.name + " " + other.name);
        EventManager.Instance.publishTriggerEnterEvent(this.name, other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("trigger exit : " + this.name + " " + other.name);
        EventManager.Instance.publishTriggerExitEvent(this.name, other.name);
    }
}