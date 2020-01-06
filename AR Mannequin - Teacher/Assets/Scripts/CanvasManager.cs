using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Kimberly Burke, 2019
/// 
/// Simple canvas switcher
/// </summary>
public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject controlCanvas;

    public enum CanvasState { Menu, Control };

    public void SwitchCanvas(CanvasState target)
    {
        switch (target)
        {
            case CanvasState.Menu:
                menuCanvas.SetActive(true);
                controlCanvas.SetActive(false);
                break;
            case CanvasState.Control:
                menuCanvas.SetActive(false);
                controlCanvas.SetActive(true);
                break;
            default:
                break;
        }
    }
}
