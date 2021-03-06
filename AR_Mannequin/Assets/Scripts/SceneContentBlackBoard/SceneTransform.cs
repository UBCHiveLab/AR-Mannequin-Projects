﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class SceneTransform: Singleton<SceneTransform> {

    protected override void Awake()
    {
        base.Awake();
        EventManager.Instance.SetSceneTransformEvent += OnSetSceneTransformEvent;
    }

    private Dictionary<string, Transform> dict = new Dictionary<string, Transform>();

    void Start () {
	}

    private void OnSetSceneTransformEvent(string name, Transform t)
    {
        dict.Add(name, t);
    }

    public Transform GetSceneTransform(string name)
    {
        if (dict.ContainsKey(name))
        {
            return dict[name];
        } else
        {
            // Debug.Log("no scene transform named " + name + " in scene content transform dictionary");
            return null;
        }
    }
}
