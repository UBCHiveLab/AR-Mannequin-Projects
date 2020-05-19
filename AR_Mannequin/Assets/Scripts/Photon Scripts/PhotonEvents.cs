using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;


/// <summary>
/// Created by Kimberly Burke, 2019
/// Originally used in server side
/// Modified by Silver Xu for student side app sending message to other clients
/// 
/// Raise Event Options Documentation:
/// https://doc-api.photonengine.com/en/pun/v2/class_photon_1_1_realtime_1_1_raise_event_options.html
/// https://doc.photonengine.com/en-us/pun/v2/gameplay/rpcsandraiseevent
/// </summary>
public class PhotonEvents :MonoBehaviourPunCallbacks
{
    //[SerializeField] FacilitatorControls teacherControls;

    private byte evCode;
    private object[] content;
    SendOptions sendOptions = new SendOptions { Reliability = true};
    // ReceiverGroup set to All, so sender will also receive this event
    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

    Dictionary<string, List<string>> partGroups;

    Dictionary<byte, bool> toggleEvent = new Dictionary<byte, bool>();
    Dictionary<byte, float> sliderEvents = new Dictionary<byte, float>();

   

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
