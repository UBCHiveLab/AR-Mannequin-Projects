using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;


/// <summary>
/// Created by Kimberly Burke, 2019
/// 
/// Raise Event Options Documentation:
/// https://doc-api.photonengine.com/en/pun/v2/class_photon_1_1_realtime_1_1_raise_event_options.html
/// </summary>
public class PhotonEvents : MonoBehaviourPunCallbacks
{
    [SerializeField] FacilitatorControls teacherControls;

    private byte evCode;
    private object[] content;
    SendOptions sendOptions = new SendOptions { Reliability = true};
    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };

    Dictionary<string, List<string>> partGroups;

    Dictionary<byte, bool> toggleEvent = new Dictionary<byte, bool>();
    Dictionary<byte, float> sliderEvents = new Dictionary<byte, float>();

    private void Start()
    {
        toggleEvent.Add(EventCodeUtility.BODY_SCAR_TOGGLE, false);
        toggleEvent.Add(EventCodeUtility.PURPLE_TOGGLE, false);
        toggleEvent.Add(EventCodeUtility.VOMIT_TOGGLE, false);
        toggleEvent.Add(EventCodeUtility.BLEED_TOGGLE, false);
        toggleEvent.Add(EventCodeUtility.WHITE_NOISE_TOGGLE, false);
        toggleEvent.Add(EventCodeUtility.HEARTBEAT_TOGGLE, true);
        toggleEvent.Add(EventCodeUtility.LUNG_L_TOGGLE, true);
        toggleEvent.Add(EventCodeUtility.LUNG_R_TOGGLE, true);
        toggleEvent.Add(EventCodeUtility.DILATE_TOGGLE_L, false);
        toggleEvent.Add(EventCodeUtility.CONSTRICT_TOGGLE_L, false);
        toggleEvent.Add(EventCodeUtility.DILATE_TOGGLE_R, false);
        toggleEvent.Add(EventCodeUtility.CONSTRICT_TOGGLE_R, false);
        toggleEvent.Add(EventCodeUtility.ECG_SOUND_TOGGLE, false);
        toggleEvent.Add(EventCodeUtility.ECG_TOGGLE, false);
    }

    /* public void TestMessage()
    {
        evCode = EventCodeUtility.TEST_MESSAGE;
        content = new object[] { "This should work please..." };
        CallRaisedEvent();
    } */

    public void TeacherPresentEvent()
    {
        evCode = EventCodeUtility.TEACHER_PRESENT;
        content = new object[] { };
        CallRaisedEvent();
    }

    /// <summary>
    /// DEPRECATED - called by toggle buttons
    /// </summary>
    /// <param name="invokedCode"></param>
    public void CallToggleEvent(int invokedCode)
    {
        evCode = (byte)invokedCode;
        toggleEvent[evCode] = !toggleEvent[evCode]; // switch status
        content = new object[] { toggleEvent[evCode] }; // send status to update to 
        CallRaisedEvent();
    }

    /// <summary>
    /// DEPRECATED - called by audio sliders
    /// </summary>
    /// <param name="invokedCode"></param>
    /// <param name="value"></param>
    public void CallSliderEvent(int invokedCode, float value)
    {
        evCode = (byte)invokedCode;
        sliderEvents[evCode] = value;
        content = new object[] { sliderEvents[evCode] };
        CallRaisedEvent();
    }

    /// <summary>
    /// Called by ECG apply button
    /// </summary>
    /// <param name="values"></param>
    public void CallApplySliderEvent(float[] values)
    {
        evCode = EventCodeUtility.SLIDE_EVENT;
        content = new object[] { values };
        CallRaisedEvent();
    }

    /// <summary>
    /// Called by CommandSend
    /// </summary>
    /// <param name="invokedCode"></param>
    /// <param name="data"></param>
    public void CallCommandEvent(byte invokedCode, object[] data)
    {
        evCode = invokedCode;
        content = data;
        CallRaisedEvent();
    }

    private void CallRaisedEvent()
    {
        Debug.Log("Sending event: " + evCode);
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }

    /// <summary>
    /// Makes accessible for Quit button
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
