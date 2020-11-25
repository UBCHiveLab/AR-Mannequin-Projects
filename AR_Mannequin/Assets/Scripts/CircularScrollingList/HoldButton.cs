using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public bool buttonPressed;
    public AudioSource metronome;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        metronome.mute = false;
        Debug.Log("Doing chest compressions");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
        metronome.mute = true;
        Debug.Log("Chest released");
    }
}