using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGrayOut
{
    public static readonly Color DISABLED_TINT = new Color(106.0f / 255.0f, 106.0f / 255.0f, 106.0f / 255.0f, 1f);
    public static void DisableButton(Button button, bool applyTint = true)
    {
        button.interactable = false;
        if (applyTint)
        {
            button.targetGraphic.color = DISABLED_TINT;
        }
    }
}
