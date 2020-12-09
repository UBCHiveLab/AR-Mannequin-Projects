﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VitalsController : MonoBehaviour
{
    [SerializeField]
    private bool grayOut=true;
    public float countdownTime=10;
    public VitalActions vitalAction;

    //Actions that will be invoke before timer starts, i.e sending message to facilitator
    public UnityEvent vitalActionBeforeTimer;
    //Actions that will be invoke after timer ends, i.e apply ecg cuff
    public UnityEvent vitalActionAfterTimer;
    private Button vitalButton;
    private Toggle vitalToggle;

    private StudentCommandSend cmdSend;
    private object newObject;

    private void Start()
    {
        cmdSend = StudentCommandSend.Instance;
    }
    private void SendMessageCommand(string message)
    {
        newObject = message;
        Command cmd = new Command((byte)EventCodeUtility.STUDENT_MESSAGE, new object[] { newObject });
        if (cmdSend != null)
            cmdSend.ApplyCommand(cmd);
    }

    public void InvokeActionBeforeTimer()
    {
        //send log message to server
        SendMessageCommand("started " + vitalAction.ToName() + ". Finish in " + countdownTime.ToString() + " seconds");
        vitalActionBeforeTimer.Invoke();
    }
    public void InvokeChestCompressionActionBeforeTimer()
    {
        //send log message to server
        SendMessageCommand("started " + vitalAction.ToName());
        vitalActionBeforeTimer.Invoke();
    }
    public void InvokeActionAfterTimer()
    {
        SendMessageCommand("did " + vitalAction.ToName()+" successfully.");
        vitalActionAfterTimer.Invoke();
        DisableButton();
    }
        public void InvokeChestCompressionActionAfterTimer()
    {
        SendMessageCommand("did " + countdownTime.ToString() + " seconds of " +  vitalAction.ToName() );
        vitalActionAfterTimer.Invoke();
        DisableButton();
    }
    public void DisableButton()
    {
        if (vitalButton != null&&grayOut)
            ButtonGrayOut.DisableButton(vitalButton);
    }
    public void AddListener(UnityAction call)
    {
        vitalButton = GetComponent<Button>();
        if (vitalButton != null)
        {
            vitalButton.onClick.AddListener(call);
            Debug.Log("added listener"+vitalButton);
        }
        
    }
    public void AddListener(UnityAction<bool> call)
    {
        vitalToggle = GetComponent<Toggle>();
        if (vitalToggle != null)
        {
            vitalToggle.onValueChanged.AddListener(call);
            
        }
    }
}
