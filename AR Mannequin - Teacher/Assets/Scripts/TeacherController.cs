using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

/// <summary>
/// Created by Kimberly Burke, 2019
/// 
/// Receives RPCs from student Hololens client and sends RPCs to mannequin object with the method being called.
/// 
/// Remote Procedural Calls (RPCs) can be sent across different scenes/projects as long as the two clients are in the same room.
/// All RPCs must be called through a PhotonView. Receiving object does not need to be an PhotonView, but methods must be identified with the
/// attribute [PunRPC]
/// </summary>
public class TeacherController : MonoBehaviourPunCallbacks
{
    private PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    #region RPC Calls
    public void BodyScarButton()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            Debug.Log("Add body scar...");
            GetComponent<PhotonView>().RPC("StartBodyScar", RpcTarget.Others);
        }
    }

    public void TransferMessage()
    {
        Debug.Log("Players in room: " + PhotonNetwork.PlayerList);
        if (GetComponent<PhotonView>().IsMine)
        {
            Debug.Log("Sending message...");
            GetComponent<PhotonView>().RPC("RPC_TransferMessage", RpcTarget.Others);
        }
    }
    #endregion

    #region RPC Callbacks - events called from Mannikin to teacher client
    [PunRPC]
    void RPC_CollissionMessage(string action)
    {
        // TODO
    }

    [PunRPC]
    void RPC_TeacherMessage(string message)
    {
        Debug.Log("Message to teacher: " + message);
    }
    #endregion
}
