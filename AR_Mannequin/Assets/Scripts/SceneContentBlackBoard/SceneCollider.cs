using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class SceneCollider: Singleton<SceneCollider>
{

    private Dictionary<string, Collider> dict = new Dictionary<string, Collider>();

    protected override void Awake()
    {
        base.Awake();
        EventManager.Instance.SetSceneColliderEvent += OnSetSceneColliderEvent;
    }

    private void OnSetSceneColliderEvent(string name, Collider t)
    {
        dict.Add(name, t);
    }

    public Collider GetSceneCollider(string name)
    {
        if (dict.ContainsKey(name))
        {
            return dict[name];
        }
        else
        {
            Debug.Log("no scene Collider named " + name + " in scene content Collider dictionary");
            return null;
        }
    }
}
