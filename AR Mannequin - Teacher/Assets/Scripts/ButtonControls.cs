using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach to button groups
/// </summary>
public class ButtonControls : MonoBehaviour
{
    [SerializeField] Toggle[] activateGroup;
    private bool activated;

    /// <summary>
    /// Used for activating another button when button is pressed
    /// </summary>
    public void ActivateButton()
    {
        activated = !activated;
        foreach (Toggle toggle in activateGroup)
        {
            toggle.interactable = activated;
            if (!activated && toggle.isOn)
            {
                toggle.isOn = false;
            }
        }
    }
}
