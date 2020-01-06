using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ApplyEyeAnimation : MonoBehaviour {

    //attached to Manikin/body-assembly/Eye

    Image right;
    Image left;
    List<Sprite> frames;

    float totalTime = 3f; // in seconds
    bool dilateState = false; // false = small/constricted eyes, true = big/dilated eyes
    //TODO more complicated current state

	// Use this for initialization
	void Awake () {
        Debug.Log("init eye animation");
        right = GameObject.Find("Right").gameObject.GetComponent<Image>(); // TODO clearer naming
        left = GameObject.Find("Left").gameObject.GetComponent<Image>();
        Debug.Log("found eye images");
        frames = Resources.LoadAll<Sprite>("EyeFrames").ToList();
        Debug.Log("added eye frames 1-98");
        //EventManager.Instance.InteractionAnimationEvent += OnInteractionAnimationEvent;
        //EventManager.Instance.publishInteractionAnimationEvent("", 0f);
        EventManager.Instance.ToggleAnimationEvent += OnToggleAnimationEvent;
        Debug.Log("done init");
    }

    private void OnInteractionAnimationEvent(string name, float frame)
    {
        //TODO do something with name and frame
        Debug.Log("called eye animation");
        StartCoroutine("AnimateEyes");
    }

    private void OnToggleAnimationEvent(string name, bool status)
    {
        if (name.Contains("dilate") || name.Contains("constrict"))
        {
            Debug.Log("Called eye animation");
            StartCoroutine(AnimateEyes(name, status));
        }
    }

    // right now it just goes from big to small
    IEnumerator AnimateEyes(string name, bool toggle)
    {
        dilateState = toggle;
        Debug.Log("animate eyes, cur state: " + dilateState);

        if (dilateState)
        {
            for (int i = 0; i < frames.Count; i++)
            {
                Debug.Log("eye frame: " + i);
                if (name.Contains("right")) { right.sprite = frames[i]; }
                else if (name.Contains("left")) { left.sprite = frames[i]; }

                yield return new WaitForSeconds(totalTime / (float)frames.Count);
            }
        } else
        {
            for (int i = frames.Count - 1; i > -1; i--)
            {
                Debug.Log("eye frame: " + i);
                if (name.Contains("right")) { right.sprite = frames[i]; }
                else if (name.Contains("left")) { left.sprite = frames[i]; }

                yield return new WaitForSeconds(totalTime / (float)frames.Count);
            }
        }
        yield return null;
    }
}
