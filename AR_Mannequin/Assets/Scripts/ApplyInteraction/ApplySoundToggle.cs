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
        EventManager.Instance.AudioPlayEvent += OnAudioPlayEvent;
        audioSource = GetComponent<AudioSource>();

        //mute all sound on begining
        OnAudioToggleEvent(sound, true);
    }

    /// <summary>
    /// mute or umute the looped audio
    /// </summary>
    /// <param name="name"></param>
    /// <param name="status"></param>
    private void OnAudioToggleEvent(string name, bool status)
    {
        if (name == sound)
        {
            audioSource.mute = status;
        }
    }

    /// <summary>
    /// change audio volume based on silder
    /// </summary>
    /// <param name="name"></param>
    /// <param name="volume"></param>
    private void OnAudioSlideEvent(string name, float volume)
    {
        if (name == sound)
        {
            audioSource.volume = volume / 100f;
        }
    }
    /// <summary>
    /// switch audio source
    /// </summary>
    /// <param name="name"></param>
    /// <param name="clip"></param>
    private void OnAudioSourceEvent(string name, string clip)
    {
        if (name == sound)
        {
            Debug.Log("changing audio source to " + clip);
            audioSource.clip = SoundRepo.GetSound(type, clip);
            if(audioSource.mute)
                audioSource.mute = false;
            audioSource.Play();
        }
    }
    /// <summary>
    /// created by Silver Xu, play audio once when needed
    /// </summary>
    /// <param name="name"></param>
    /// <param name="status"></param>
    private void OnAudioPlayEvent(string name,bool status)
    {
        if(name == sound)
        {
            if (status)
            {
                Debug.Log("Play audio source" + name);
                if (audioSource.mute)
                    audioSource.mute = false;
                audioSource.Play();
            }
            else
            {
                Debug.Log("Stop playing audio source" + name);
                audioSource.Stop();
            }
        }
    }
}
