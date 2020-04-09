using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SceneBuilder : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        this.gameObject.AddComponent<EventManager>();
        EventManager stub = EventManager.Instance;

        this.gameObject.AddComponent<Make>();
        this.gameObject.AddComponent<Parse>();
        this.gameObject.AddComponent<SceneCollider>();
        this.gameObject.AddComponent<SceneRenderer>();
        this.gameObject.AddComponent<SceneTransform>();

        Make stubby = Make.Instance;
        Parse stubbyy = Parse.Instance;
        SceneCollider stubs = SceneCollider.Instance;
        SceneRenderer stubss = SceneRenderer.Instance;
        SceneTransform stubsss = SceneTransform.Instance;

        //Instantiate(Resources.Load("Prefabs/ARCamera"));
        //Object manikin = Instantiate(Resources.Load("Prefabs/Manikin") as GameObject);
        //Object overlay = Instantiate(Resources.Load("Prefabs/OverlayCanvas") as GameObject);
        // Instantiate(Resources.Load("Prefabs/MagicStick"));
        Instantiate(Resources.Load("Prefabs/SoundManager") as GameObject);
        
        //Instantiate(Resources.Load("Prefabs/UIPanel"));

        Instantiate(Resources.Load("Prefabs/Initializer") as GameObject);
        PhotonNetwork.Instantiate("Prefabs/PhotonPlayer", Vector3.zero, Quaternion.identity);
        //Instantiate(Resources.Load("Prefabs/PhotonPlayer"));
    }
}
