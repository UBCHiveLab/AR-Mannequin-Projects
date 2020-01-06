using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsUIManager : MonoBehaviour
{

    private const float Z_DISTANCE_FROM_PIVOT = 1610.0f;
    private bool gazeIsOnUI;
    private bool isPinned;
    private int gazeDelayCounter;
    private AudioSource soundFX;

    // Use this for initialization
    void Start()
    {
        Debug.Log("UI Controls manager start");
        gazeIsOnUI = false;
        isPinned = false;
        gazeDelayCounter = 30;
        soundFX = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame

    void Update()
    {
        if (!isPinned)
        {
            transform.position = Camera.main.transform.position;
            if (gazeDelayCounter == 0)
            {
                if (!gazeIsOnUI)
                {
                    transform.rotation = Quaternion.Lerp(
                        new Quaternion(0, Camera.main.transform.rotation.y, 0, Camera.main.transform.rotation.w),
                        transform.rotation, 0.9f);
                }
            }
            else
            {
                gazeDelayCounter--;
            }
        }
    }


    public void TogglePinUI()
    {
        isPinned = !isPinned;
        soundFX.Play();
    }

    public void OnGazeEnteredUI()
    {
        gazeIsOnUI = true;
    }

    public void OnGazeExitUI()
    {
        gazeIsOnUI = false;
        gazeDelayCounter = 30;
    }

    public bool GetMenuPinState()
    {
        return isPinned;
    }
}