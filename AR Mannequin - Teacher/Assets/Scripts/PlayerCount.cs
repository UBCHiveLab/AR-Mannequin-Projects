using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCount : MonoBehaviour
{
    private List<PlayerListing> _listings = new List<PlayerListing>();
    public Text numberOfPlayers; 

    private void Update()
    {
        numberOfPlayers.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
    }
}
