using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteToggle : MonoBehaviour
{
    AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void ToggleBool()
    {
        if (audio.mute)
            audio.mute = false;
        else
            audio.mute = true;

    }
}
