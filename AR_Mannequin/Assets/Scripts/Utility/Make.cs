using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Make : Singleton<Make> {

	// Use this for initialization
	void Start () {
		
	}
	
    // this one has 
	public void MakeCollider(GameObject go)
    {
        if (go.GetComponent<ColliderSetup>() == null)
        {
            go.AddComponent<ColliderSetup>();
        }
    }

    public void MakeCollidee(GameObject go)
    {
        if (go.GetComponent<CollideeSetup>() == null)
        {
            go.AddComponent<CollideeSetup>();
        }
    }

    public AudioSource MakeAudioSource(GameObject go)
    {
        AudioSource a = go.GetComponent<AudioSource>();
        if (a == null)
        {
            a = go.AddComponent<AudioSource>();
        }
        return a;
    } 
}
