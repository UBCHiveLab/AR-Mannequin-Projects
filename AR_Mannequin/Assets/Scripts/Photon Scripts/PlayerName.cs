﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class PlayerName : MonoBehaviour
{
    public InputField firstname;
    public InputField lastname;
    public Button setNameBtn;

    public void OnTextFieldChange()
    {
        if (firstname.text.Length > 0 && lastname.text.Length > 0)
        {
            setNameBtn.interactable = true;
        }
    }

    public void OnClick_SetName()
    {
        PhotonNetwork.NickName = firstname.text; 
    }
}
