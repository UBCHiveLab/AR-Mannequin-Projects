using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
/// <summary>
/// Apply eye animation.
/// Modified by Silver Xu 2020, to replace the old animating method to reduce memory overflow issue
/// </summary>
public class ApplyEyeAnimation : MonoBehaviour {

    //attached to Manikin/body-assembly/Eye

   
    [SerializeField] Animator leftEye, rightEye;

    float totalTime = 3f; // in seconds
    bool dilateState = false; // false = small/constricted eyes, true = big/dilated eyes
    //TODO more complicated current state

	// Use this for initialization
	void Awake () {
        Debug.Log("init eye animation");

        EventManager.Instance.ToggleAnimationEvent += OnToggleAnimationEvent;

    }



    private void OnToggleAnimationEvent(string name, bool status)
    {
        if (name.Contains("dilate") || name.Contains("constrict"))
        {
            Debug.Log("Called eye animation");

            string[] arr = name.Split('_');
            Debug.Log("eye:"+arr[1]);
            if (arr[1] == "left")
            {
                DisplayEyeAnimation(leftEye, arr[0], status);
            }
            else if (arr[1] == "right")
            {
                DisplayEyeAnimation(rightEye, arr[0], status);
            }
        }
    }
    private void DisplayEyeAnimation(Animator eye,string animationType,bool status)
    {
        string animationDirection;
        if (status)
            animationDirection = "forward";
        else
            animationDirection = "backward";
        
        eye.Play(animationType + '_' + animationDirection);
    }


}
