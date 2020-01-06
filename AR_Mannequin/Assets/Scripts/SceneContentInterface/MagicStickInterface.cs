using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using System;

/// <summary>
/// DEPRECATED
/// </summary>
public class MagicStickInterface : Singleton<MagicStickInterface> {

    GameObject MagicStick;
    Transform MagicStick_ref_transform;
    Renderer MagicStick_ref_renderer;
    List<Transform> tools;
    Renderer[] renderers;

    // Use this for initialization
    /*protected override void Awake()
    {
        base.Awake();

        MagicStick = GameObject.Find("MagicStick(Clone)");
        tools = new List<Transform>();
        foreach (Transform cur in MagicStick.transform.Find("hand_tool"))
        {
            tools.Add(cur);
        }
        MagicStick_ref_transform = MagicStick.transform;
        MagicStick_ref_renderer = MagicStick_ref_transform.Find("MagicStickRef").gameObject.GetComponent<Renderer>();
    }

    private void Start()
    {
        foreach(Transform cur in tools)
        {
            Make.Instance.MakeCollider(cur.gameObject);
            EventManager.Instance.publishSetSceneTransform(cur.name, cur);
            EventManager.Instance.publishSetSceneRenderer(cur.name, cur.gameObject.GetComponent<Renderer>());
        }
    }*/
}
