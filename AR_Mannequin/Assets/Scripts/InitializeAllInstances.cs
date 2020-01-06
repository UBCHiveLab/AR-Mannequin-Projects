using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using HoloToolkit.Unity;
using UnityEngine;

public class InitializeAllInstances : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        this.gameObject.AddComponent<SortStethoscopeAndOrganSounds>();
        this.gameObject.AddComponent<MagicStickInterface>();
        this.gameObject.AddComponent<SceneOrganInterface>();
        this.gameObject.AddComponent<ApplyOrganSound>();
        this.gameObject.AddComponent<CalculateCollision>();
        this.gameObject.AddComponent<ApplyIMUUpdate>();
        this.gameObject.AddComponent<SortOverlays>();
        this.gameObject.AddComponent<ApplyOverlayOnOff>();

        ApplyIMUUpdate mcStub = ApplyIMUUpdate.Instance;
        CalculateCollision stubi = CalculateCollision.Instance;

        ApplyOrganSound stubz = ApplyOrganSound.Instance;
        ApplyOverlayOnOff stubzzzz = ApplyOverlayOnOff.Instance;

        SortStethoscopeAndOrganSounds stubb = SortStethoscopeAndOrganSounds.Instance;
        MagicStickInterface stubbb = MagicStickInterface.Instance;
        SceneOrganInterface stubbbb = SceneOrganInterface.Instance;
        SortOverlays stubzzz = SortOverlays.Instance;
    }

}
