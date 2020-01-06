// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCommands : MonoBehaviour {

    public bool buttonIsEnabled { get; private set; }

    private ControlsUIManager controlsUI;

    public Sprite hoverState;
    public Sprite activeState;
    public string GAZE_FRAME_NAME = "white-border";
    Color FullOpacityColor;
    Color PartialOpacityColor;
    bool IsPressed;

    public delegate void MultiDelegate();
    public MultiDelegate Commands;

    private void Start()
    {
        controlsUI = transform.GetComponentInParent<ControlsUIManager>();
        
        //disable the white selection frame
    }

    private void Update()
    {
    }

    void OnStartGaze()
    {
        //let the UIManager know that it is being gazed at
        if(controlsUI != null) {
            controlsUI.OnGazeEnteredUI();
        }

        //visual change of the button on gaze over
        //EnableOrDisableFrame(true);
    }

    void OnEndGaze()
    {
        //let the UIManager know that it is no longer being gazed at
        if(controlsUI != null)
        {
            controlsUI.OnGazeExitUI();
        }

        //visual change of the button on gaze over
        //EnableOrDisableFrame(false);
    }

    public void AddCommand(Action command)
    {
        Commands += delegate
        {
            command();
        };
    }

    public void OnSelect()
    {
        if(Commands != null)
        {
            Commands();
        }
    }

}
