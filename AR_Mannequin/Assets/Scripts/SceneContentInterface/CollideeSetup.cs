using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideeSetup : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject go = this.gameObject;
        BoxCollider c = go.GetComponent<BoxCollider>();
        if (c == null)
        {
            c = go.AddComponent<BoxCollider>();
        }
        go.GetComponent<Renderer>().bounds.Encapsulate(c.bounds);
        //go.GetComponent<Renderer>().enabled = false;
        EventManager.Instance.publishSetSceneCollider(go.name, c);
    }
}
