using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

/// <summary>
/// Created by Kimberly Burke, 2019
/// 
/// How to receive raised events:
/// https://www.youtube.com/watch?v=MUKz8ZX69xI
/// </summary>
public class PhotonReceiver : MonoBehaviour {

    private Dictionary<string, List<string>> parts;

    private void Start()
    {
        parts = Parse.Instance.SwitchButtons; // gets list of body part game objects
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        Debug.Log("Received event: " + obj.Code);

        object[] datas = new object[] { new object() };
        try
        {
            if(obj.CustomData != null)
            {
                datas = (object[])obj.CustomData; // must be cast into object array - when accessing data, cast into appropriate type
            }
        } catch(Exception e)
        {
            Debug.Log("Error while casting event data " + obj.CustomData.ToString() + " " +  e.Message);
        }

        switch (obj.Code)
        {
            case EventCodeUtility.TEACHER_PRESENT:
                InvokeTeacherEvent();
                break;
            case EventCodeUtility.BODY_SCAR_TOGGLE:
                Debug.Log("Toggling body scar.");
                InvokeToggleEvent("body_scar", (bool)datas[0]);
                break;
            case EventCodeUtility.PURPLE_TOGGLE:
                Debug.Log("Toggling lip color.");
                InvokeToggleEvent("lips_color", (bool)datas[0]);
                break;
            case EventCodeUtility.VOMIT_TOGGLE:
                Debug.Log("Toggling vomit animation. Play vomit sound.");
                EventManager.Instance.publishToggleMeshAnimationEvent("vomit", (bool)datas[0]);
                EventManager.Instance.publishAudioPlayEvent("vomit", (bool)datas[0]);
                break;
            case EventCodeUtility.DILATE_TOGGLE_R:
                Debug.Log("Toggling eye dilation.");
                EventManager.Instance.publishToggleAnimationEvent("dilate_right", (bool)datas[0]);
                break;
            case EventCodeUtility.DILATE_TOGGLE_L:
                Debug.Log("Toggling eye dilation.");
                EventManager.Instance.publishToggleAnimationEvent("dilate_left", (bool)datas[0]);
                break;
            case EventCodeUtility.CONSTRICT_TOGGLE_R:
                Debug.Log("Toggling eye constriction.");
                EventManager.Instance.publishToggleAnimationEvent("constrict_right", (bool)datas[0]);
                break;
            case EventCodeUtility.CONSTRICT_TOGGLE_L:
                Debug.Log("Toggling eye constriction.");
                EventManager.Instance.publishToggleAnimationEvent("constrict_left", (bool)datas[0]);
                break;
            case EventCodeUtility.BLEED_TOGGLE:
                Debug.Log("Toggling bleed animation.");
                EventManager.Instance.publishToggleMeshAnimationEvent("bleed", (bool)datas[0]);
                break;
            case EventCodeUtility.SLIDE_EVENT:
                Debug.Log("Updating ECG Monitor values.");
                InvokeUpdateECGEvent(datas);
                break;
            case EventCodeUtility.HEARTBEAT_SLIDE:
                Debug.Log("Adjusting volume for heartbeat.");
                InvokeSoundSlideEvent("heart", (float)datas[0]);
                break;
            case EventCodeUtility.LUNG_L_SLIDE:
                Debug.Log("Adjusting volume for heartbeat.");
                InvokeSoundSlideEvent("lung_left", (float)datas[0]);
                break;
            case EventCodeUtility.LUNG_R_SLIDE:
                Debug.Log("Adjusting volume for heartbeat.");
                InvokeSoundSlideEvent("lung_right", (float)datas[0]);
                break;
            case EventCodeUtility.WHITE_NOISE_SLIDE:
                Debug.Log("Adjusting volume for heartbeat.");
                InvokeSoundSlideEvent("white_noise", (float)datas[0]);
                break;
            case EventCodeUtility.WHITE_NOISE_TOGGLE:
                Debug.Log("Toggling white noise sound.");
                InvokeSoundToggleEvent("white_noise", (bool)datas[0]);
                break;
            case EventCodeUtility.HEARTBEAT_TOGGLE:
                Debug.Log("Toggling heartbeat sound.");
                InvokeSoundToggleEvent("heart", (bool)datas[0]);
                break;
            case EventCodeUtility.LUNG_L_TOGGLE:
                Debug.Log("Toggling left lung sound.");
                InvokeSoundToggleEvent("lung_left", (bool)datas[0]);
                break;
            case EventCodeUtility.LUNG_R_TOGGLE:
                Debug.Log("Toggling right lung sound.");
                InvokeSoundToggleEvent("lung_right", (bool)datas[0]);
                break;
            case EventCodeUtility.ECG_SOUND_TOGGLE:
                Debug.Log("Toggling ecg sound.");
                InvokeSoundToggleEvent("ecg", (bool)datas[0]);
                break;
            case EventCodeUtility.LUNG_R_SOURCE:
                InvokeSoundSourceEvent("lung_right", (string)datas[0]);
                break;
            case EventCodeUtility.LUNG_L_SOURCE:
                InvokeSoundSourceEvent("lung_left", (string)datas[0]);
                break;
            case EventCodeUtility.HEARTBEAT_SOURCE:
                InvokeSoundSourceEvent("heart", (string)datas[0]);
                break;
            case EventCodeUtility.WHITE_NOISE_SOURCE:
                InvokeSoundSourceEvent("white_noise", (string)datas[0]);
                break;
            case EventCodeUtility.XRAY_EVENT:
                Debug.Log("Showing xray.");
                InvokeDisplayImageEvent("XRAY", (string)datas[0]);
                break;
            case EventCodeUtility.CT_EVENT:
                Debug.Log("Showing CT. " + (string)datas[0]);
                InvokeDisplayImageEvent("CT", (string)datas[0]);
                break;
            case EventCodeUtility.ULTRASOUND_EVENT:
                Debug.Log("Showing ULTRASOUND. " + (string)datas[0]);
                InvokeDisplayImageEvent("ULTRASOUND", (string)datas[0]);
                break;
            case EventCodeUtility.SKIN_COLOR:
                Debug.Log("changing skin color");
                InvokeSkinColorEvent("skin_color", (string)datas[0]);
                break;
            case EventCodeUtility.QUIT_SESSION:
                Debug.Log("Session quit");
                //SceneManager.LoadScene(0);
                break;
            default:
                Debug.Log("Unhandled event code" + obj.ToString());
                break;
        }
    }

    private void InvokeTeacherEvent()
    {
        Debug.Log(EventManager.Instance);
        EventManager.Instance.publishTeacherPresentEvent();
    }

    private void InvokeToggleEvent(string group, bool status)
    {
        if (status) {      
            EventManager.Instance.publishSwitchTriggerEvent(parts[group][1], group);
        }
        else
        {
            EventManager.Instance.publishSwitchTriggerEvent(parts[group][0], group);
        }
    }

    private void InvokeSoundToggleEvent(string sound, bool status)
    {
        EventManager.Instance.publishAudioToggleEvent(sound, status);
    }

    private void InvokeUpdateECGEvent(object[] values)
    {
        float[] valuesF = new float[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            valuesF[i] = (float)values[i];
         }
        EventManager.Instance.publishECGUpdateEvent(valuesF);
    }

    private void InvokeSoundSlideEvent(string sound, float volume)
    {
        EventManager.Instance.publishAudioSlideEvent(sound, volume);
    }

    private void InvokeSoundSourceEvent(string sound, string name)
    {
        
        EventManager.Instance.publishAudioSourceEvent(sound, name);
    }

    private void InvokeDisplayImageEvent(string type, string name)
    {
        EventManager.Instance.publishDisplayImageEvent(type, name);
    }

    private void InvokeSkinColorEvent(string name, string color)
    {
        EventManager.Instance.publishSkinColorEvent(name, color);
    }
}
