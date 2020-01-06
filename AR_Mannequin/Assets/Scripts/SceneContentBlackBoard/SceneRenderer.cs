using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class SceneRenderer: Singleton<SceneRenderer>
{
    private Dictionary<string, Renderer> dict = new Dictionary<string, Renderer>();

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        EventManager.Instance.SetSceneRendererEvent += OnSetSceneRendererEvent;
    }

    void Start()
    {
       
    }

    private void OnSetSceneRendererEvent(string name, Renderer r)
    {
        dict.Add(name, r);
    }

    public Renderer GetSceneRenderer(string name)
    {
        if (dict.ContainsKey(name))
        {
            return dict[name];
        }
        else
        {
            // Debug.Log("no scene Renderer named " + name + " in scene content Renderer dictionary");
            return null;
        }
    }
}