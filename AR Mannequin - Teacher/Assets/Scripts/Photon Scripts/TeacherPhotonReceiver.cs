using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using System;

public class TeacherPhotonReceiver : MonoBehaviour
{
    public delegate void SetLogDelegate(string studentName, string logText);
    public SetLogDelegate SetLog;

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
        int senderID = obj.Sender;
        string senderNickName = senderID <= 0 ? "Teacher" : PhotonNetwork.CurrentRoom.GetPlayer(senderID).NickName;

        object[] datas = new object[] { new object() };
        try
        {
            if (obj.CustomData != null)
            {
                datas = (object[])obj.CustomData; // must be cast into object array - when accessing data, cast into appropriate type
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error while casting event data " + obj.CustomData.ToString() + " " + e.Message);
        }

        switch (obj.Code)
        {
            case EventCodeUtility.STUDENT_MESSAGE:
                SetLog(senderNickName, (string)datas[0]);
                break;
        }
    }
}
