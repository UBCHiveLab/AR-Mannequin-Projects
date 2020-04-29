using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/// <summary>
/// Created by Kimberly Burke, 2019
/// 
/// Controls UI during session and functions of ECG monitor
/// </summary>
public class ConnectionUIManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text statusText;
    [SerializeField] Text facilitatorText;
    public Toggle calibrateToggle;
    [SerializeField] Canvas activeCanvas;

    private bool dead;
    private float heartRate;
    [SerializeField] AudioClip[] ecgSounds; // [single beep, warning, dead]
    [SerializeField] AudioSource heartSound;

    private AudioSource audioSource;
    private AudioMixerGroup heartMixer;
    private bool heartCoroutine;

    private void Awake()
    {
        if (GameStateUtility.GetConnectionStatus())
        {
            statusText.text = "Successfully joined " + GameStateUtility.GetRoomName() + " room.";
        }

        EventManager.Instance.TeacherPresentEvent += OnTeacherPresentEvent;
        EventManager.Instance.ECGUpdateEvent += OnHeartbeatUpdate;
        activeCanvas.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        audioSource = GetComponent<AudioSource>();
        dead = true;

        // https://forum.unity.com/threads/change-speed-in-music-sound.502429/
        heartMixer = Resources.Load<AudioMixerGroup>("AudioClips/HEART/HeartMixer");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        statusText.text = "Attempting to reconnect to server...";
        SceneManager.LoadScene(0);
    }

    public override void OnJoinedRoom()
    {
        statusText.text = "Successfully rejoined " + GameStateUtility.GetRoomName() + "  room.";
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Could not join room. " + returnCode + ": " + message);
        statusText.text = "Could not join room. " + returnCode + ": " + message;
    }

    private void OnHeartbeatUpdate(float[] values)
    {
        heartRate = values[0];
        if (heartRate <= 0)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(ecgSounds[2]);
            if (heartSound.outputAudioMixerGroup != null) { heartSound.outputAudioMixerGroup = null; }
            dead = true;
        } else if (heartRate > 0)
        { 
            if (dead) {
                StartCoroutine("PlayECGHeartbeat");
                dead = false;
            }

            if (heartSound.outputAudioMixerGroup == null) { heartSound.outputAudioMixerGroup = heartMixer; }
            float pitch = heartRate / 60f;
            heartSound.pitch = pitch; // adjusts the speed of sound clip
            heartMixer.audioMixer.SetFloat("heartMixer", 1f / pitch); // sets the pitch back to "normal" range
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newMasterClient"></param>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        Debug.Log("Teacher has left room.");
        // Show quit button & update status that "Facilitator is not present."
        facilitatorText.text = "Facilitator not present.";
    }

    private void OnTeacherPresentEvent()
    {
        facilitatorText.text = " Facilitator is present.";
    }

    /// <summary>
    /// Called by Quit button
    /// </summary>
    public void QuitApplication()
    {
        PhotonNetwork.Disconnect();
        Application.Quit();
    }

    /// <summary>
    /// Called by ModelBehavior4 
    /// </summary>
    /// <param name="status"></param>
    public void AdjustCalibrateStatus(bool status)
    {
        calibrateToggle.isOn = status;
    }

    IEnumerator PlayECGHeartbeat()
    {
        while (heartRate > 0)
        {
            audioSource.clip = ecgSounds[0];
            audioSource.Play();
            // heartSound.Play(); // does not work well for speeds > 80
            yield return new WaitForSeconds(60f / heartRate);
        }
    }
}
