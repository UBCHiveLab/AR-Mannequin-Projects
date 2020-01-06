using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using System;

public class ApplyIMUUpdate : Singleton<ApplyIMUUpdate>
{

    string objectName = "AnneHead";
    Transform objectTransform;
    //Renderer objectRenderer;

    protected override void Awake()
    {
        base.Awake();
        EventManager.Instance.IMUTransformUpdateEvent += OnIMUTransformUpdateEvent;
    }

    void Start()
    {
        //objectRenderer = GameObject.Find("AnneHead").GetComponent<Renderer>();
        objectTransform = GameObject.Find("AnneHead").transform;
    }

    private void OnIMUTransformUpdateEvent(string name, Quaternion q)
    {
        if (name == objectName)
        {
            //Debug.Log("applying rots");
            objectTransform.localRotation = q;
        }
    }
}
