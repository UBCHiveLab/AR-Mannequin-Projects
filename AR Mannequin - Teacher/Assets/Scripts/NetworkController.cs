using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Kimberly Burke, 2019
/// 
/// Controls the connection to the main client and creating room session
/// </summary>
public class NetworkController : MonoBehaviourPunCallbacks
{

    [SerializeField] private Button connectButton;
    [SerializeField] private Button startButton;
    [SerializeField] private InputField roomInput;
    [SerializeField] private byte maxPlayersPerRoom;

    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private CommandSend commandSender;

    [SerializeField] private Text status;
    [SerializeField] private Text error;

    private string roomName;
    private int playerNum;

    /******************************************************
    * Refer to the Photon documentation and scripting API for official definitions and descriptions
    * 
    * Documentation: https://doc.photonengine.com/en-us/pun/current/getting-started/pun-intro
    * Scripting API: https://doc-api.photonengine.com/en/pun/v2/index.html
    * 
    * If your Unity editor and standalone builds do not connect with each other but the multiple standalones
    * do then try manually setting the FixedRegion in the PhotonServerSettings during the development of your project.
    * https://doc.photonengine.com/en-us/realtime/current/connection-and-authentication/regions
    *
    * ******************************************************/
    // Start is called before the first frame update
    private void Awake()
    {
        roomName = ""; 

        if (GameStateUtility.GetConnectionStatus())
        {
            Debug.Log("Still connected...");
            connectButton.transform.GetChild(0).gameObject.SetActive(false);
            connectButton.transform.GetChild(2).gameObject.SetActive(true);
            startButton.interactable = true;
            roomInput.interactable = true;
            connectButton.interactable = false;
        }
        else
        {
            connectButton.transform.GetChild(2).gameObject.SetActive(false);
            connectButton.transform.GetChild(0).gameObject.SetActive(true);
            startButton.interactable = false;
            roomInput.interactable = false;
            connectButton.interactable = true;
        }
    }

    #region PUN Connection
    public void ConnectToMaster()
    {
        PhotonNetwork.ConnectUsingSettings(); //Connects to Photon master servers
        //Other ways to make a connection can be found here: https://doc-api.photonengine.com/en/pun/v2/class_photon_1_1_pun_1_1_photon_network.html
        connectButton.interactable = false;
        connectButton.transform.GetChild(0).gameObject.SetActive(false);
        connectButton.transform.GetChild(1).gameObject.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        connectButton.transform.GetChild(0).gameObject.SetActive(false);
        connectButton.transform.GetChild(1).gameObject.SetActive(false);
        connectButton.transform.GetChild(2).gameObject.SetActive(true);
        connectButton.interactable = false;
        Debug.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " server!");
        GameStateUtility.SetConnectionStatus(true);
        roomInput.interactable = true;

        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        status.text = "Not connected to server.";
        error.text = "Reason for disconnect: " + cause;
        GameStateUtility.SetConnectionStatus(false);
        if (cause != DisconnectCause.DisconnectByClientLogic && cause != DisconnectCause.DisconnectByServerLogic) { StartCoroutine(Reconnect()); }
    }

    IEnumerator Reconnect()
    {
        while (!GameStateUtility.GetConnectionStatus())
        {
            status.text = "Attempting to reconnect to server...";
            PhotonNetwork.ConnectUsingSettings();
            yield return null;
        }
    }

    private void OnApplicationQuit()
    {
        PhotonNetwork.Disconnect();
        startButton.interactable = false;
        roomInput.interactable = false;
    }
    #endregion

    #region Photon Room Creation
    public void ActivateCreateButton()
    {
        if (roomInput.text != null)
        {
            startButton.interactable = true;
        }
    }

    public void CreateNewRoom()
    {
        roomName = roomInput.text;
        Debug.Log("Creating new room...");
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnCreatedRoom()
    {
        status.text = "Successfully created " + roomName + " room.";
        error.text = "";
        canvasManager.SwitchCanvas(CanvasManager.CanvasState.Control);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        status.text = "Failed to create " + roomName + " room.";
        error.text = returnCode + ": " + message;
    }
    #endregion

    #region Photon Join Room
    private void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom() //Callback function for when we successfully create or join a room.
    {
        status.text = "Successfully joined " + roomName + " room.";
        error.text = "Number of players: " + playerNum;
        Debug.Log(PhotonNetwork.CurrentRoom);
        // PhotonNetwork.Instantiate("Prefabs/PhotonTeacher", Vector3.zero, Quaternion.identity);\
        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);

        // Adds teacher present command to Command Send buffered event list
        Command teacherPresent = new Command(EventCodeUtility.TEACHER_PRESENT, new object[] { });
        commandSender.ApplyCommand(teacherPresent);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        status.text = "Failed to join " + roomName + " room.";
        error.text = returnCode + ": " + message;
        canvasManager.SwitchCanvas(CanvasManager.CanvasState.Menu);
    }
    #endregion

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined lobby..." + PhotonNetwork.InLobby);
        Debug.Log("Player count: " + PhotonNetwork.CountOfPlayers);

        // Reconnection event - rejoin room if open
        if (roomName != "") { JoinRoom(); }
    }

    #region Photon Player Count
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        commandSender.OnNewPlayer();
        playerNum++;
        error.text = "Number of players: " + playerNum;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        playerNum--;
        error.text = "Number of players: " + playerNum;
    }
    #endregion
}
