using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Kimberly Burke 2019
/// 
/// Used to toggle audio clips. Must be attached to game object with the audio sourcd.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ApplySoundToggle : MonoBehaviour
{
    [SerializeField] string sound;
    [SerializeField] string type;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        EventManager.Instance.AudioToggleEvent += OnAudioToggleEvent;
        EventManager.Instance.AudioSlideEvent += OnAudioSlideEvent;
        EventManager.Instance.AudioSourceEvent += OnAudioSourceEvent;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnAudioToggleEvent(string name, bool status)
    {
        if (name == sound)
        {
            audioSource.mute = status;
        }
    }

    private void OnAudioSlideEvent(string name, float volume)
    {
        if (name == sound)
        {
            audioSource.volume = volume / 100f;
        }
    }

    private void OnAudioSourceEvent(string name, string clip)
    {
        if (name == sound)
        {
            Debug.Log("changing audio source to " + clip);
            audioSource.clip = SoundRepo.GetSound(type, clip);
            audioSource.Play();
        }
    }
}
