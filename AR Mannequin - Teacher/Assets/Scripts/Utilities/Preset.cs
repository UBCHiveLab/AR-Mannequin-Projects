using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PresetJSON {

    public string name;

    public string left_eye;
    public string right_eye;
    public string lip_color;
    public bool vomit_state;
    public string body_scar;

    public float[] ecg;

    public string heart_track;
    public string left_lung_track;
    public string right_lung_track;
}

[System.Serializable]
public class PresetList {
    public PresetJSON[] presetList = new PresetJSON[0];
}


