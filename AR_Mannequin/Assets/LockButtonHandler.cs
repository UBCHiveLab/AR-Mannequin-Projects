using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockButtonHandler : MonoBehaviour
{
    public Button button;
    public Lean.Touch.LeanPinchScale scaleScript;
    public Lean.Touch.LeanDragTranslate dragScript;
    bool isItLocked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LockMechanism()
    {
        Text txt = transform.Find("Text").GetComponent<Text>();
        ColorBlock colors = GetComponent<Button>().colors;
        if (isItLocked == false)
        {   
            isItLocked = true;
            txt.text = "Locked";
            colors.normalColor = Color.red;
            colors.highlightedColor = Color.red;
            colors.pressedColor = Color.green;
            button.colors = colors;
            scaleScript.enabled = false;
            dragScript.enabled = false;
        }
        else
        {
            isItLocked = false;
            txt.text = "Unlocked";
            colors.normalColor = Color.blue;
            colors.highlightedColor = Color.blue;
            colors.pressedColor = Color.green;
            button.colors = colors;
            scaleScript.enabled = true;
            dragScript.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
