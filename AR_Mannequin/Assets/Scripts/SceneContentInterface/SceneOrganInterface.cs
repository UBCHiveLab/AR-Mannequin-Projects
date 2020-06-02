using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOrganInterface : Singleton<SceneOrganInterface>
{
    List<GameObject> organs = new List<GameObject>();

    // Use this for initialization
     void Awake()
    {

        organs.Add(GameObject.Find("Pulmonary"));
        organs.Add(GameObject.Find("Tricuspid"));
        organs.Add(GameObject.Find("Mitral"));
        organs.Add(GameObject.Find("Aortic"));
        //TODO add more organs later on
    }

    private void Start()
    {
        foreach (GameObject o in organs)
        {
            EventManager.Instance.publishSetSceneTransform(o.name, o.transform);
            Make.Instance.MakeCollidee(o);
        }
    }
}
