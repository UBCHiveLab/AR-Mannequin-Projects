using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using UnityEngine.SceneManagement;

/// <summary>
/// Created by Kimberly Burke, 2019
/// 
/// Controls the connection to the main client and joining room session
/// </summary>
public class NetworkController : MonoBehaviourPunCallbacks
{

    [SerializeField] private Button connectButton;
    [SerializeField] private Button startButton;
    [SerializeField] private InputField roomInput;
    [SerializeField] private Text textInput;
    [SerializeField] private Text failureText;
    [SerializeField] private Text userNameInput;

    private string roomName, userId;


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
    private void SetUserID()
    {
        AuthenticationValues authValues = new AuthenticationValues();
        userId = userNameInput.text;
        
        authValues.UserId = userId;
        PhotonNetwork.AuthValues = authValues;
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
        if (!GameStateUtility.GetJoinedRoomStatus())
        {
            // Start up UI setting - haven't already joined an existing room
            connectButton.transform.GetChild(0).gameObject.SetActive(false);
            connectButton.transform.GetChild(1).gameObject.SetActive(false);
            connectButton.transform.GetChild(2).gameObject.SetActive(true);
            connectButton.interactable = false;
            roomInput.interactable = true;
        }
        Debug.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " server!");
        GameStateUtility.SetConnectionStatus(true); 
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        GameStateUtility.SetConnectionStatus(false);
        if (cause != DisconnectCause.DisconnectByClientLogic && cause != DisconnectCause.DisconnectByServerLogic) { StartCoroutine(Reconnect()); }
    }

    IEnumerator Reconnect()
    {
        while (!GameStateUtility.GetConnectionStatus())
        {
            PhotonNetwork.ConnectUsingSettings();
            yield return null;
        }
    }

    private void OnApplicationQuit()
    {
        PhotonNetwork.Disconnect();
        // startButton.interactable = false;
        // roomInput.interactable = false;
    }
    #endregion

    #region Photon Lobby
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined lobby..." + PhotonNetwork.InLobby);
        Debug.Log("Player count: " + PhotonNetwork.CountOfPlayers);
        Debug.Log("Master?: " + PhotonNetwork.IsMasterClient);

        // Reconnection event - rejoin room if open
        if (GameStateUtility.GetJoinedRoomStatus())
        {
            JoinRoom();
        }
    }
    #endregion

    #region Photon Join Room
    public void JoinRoom()
    {
        Debug.Log("Room name: " + roomName);
        Debug.Log("Joined Room..." + PhotonNetwork.InLobby);
        Debug.Log("Player count: " + PhotonNetwork.CountOfPlayers);
        if (!GameStateUtility.GetJoinedRoomStatus())
        {
            // Start up UI setting - haven't already joined an existing room
            roomName = textInput.text;
            startButton.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            startButton.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
        PhotonNetwork.JoinRoom(roomName);
    }
    /// <summary>
    /// 
    /// </summary>
    public override void OnJoinedRoom() //Callback function for when we successfully create or join a room.
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        // PhotonNetwork.LoadLevel(1);  
        if (!GameStateUtility.GetJoinedRoomStatus())
        {
            // Start up scene building - not called when rejoining from connection lost event
            //Instantiate(Resources.Load("Prefabs/SceneBuilder"));
            //Instantiate(Resources.Load("Prefabs/SpatialMapping"));
            //VuforiaRuntime.Instance.InitVuforia();
            //Camera.main.GetComponent<VuforiaBehaviour>().enabled = true;
            GameStateUtility.SetRoomName(roomName);
            GameStateUtility.SetJoinedRoomStatus(true);
            //PhotonNetwork.Instantiate("Prefabs/PhotonPlayer", Vector3.zero, Quaternion.identity);
            SceneManager.LoadScene(1);
        }
        //Destroy(GameObject.Find("Keyboard"));
        //Destroy(GameObject.Find("HomeUIPanel"));
        PhotonNetwork.Instantiate("Prefabs/PhotonPlayer", Vector3.zero, Quaternion.identity);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Could not join room. " + returnCode + ": " + message);
        if (SceneManager.GetActiveScene().buildIndex != 0)
            SceneManager.LoadScene(0);
        failureText.text = "Could not join room. " + message;
        startButton.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        startButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void ActivateJoinButton()
    {
        if (textInput.text != null)
        {
            startButton.interactable = true;
        }   
    }
    #endregion
}