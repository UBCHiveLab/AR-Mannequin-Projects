using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockButtonHandler : MonoBehaviour
{
    public Button button;
    public Sprite lockedSprite;
    public Sprite unlockedSprite;
    public GameObject planeFinder;
    public Lean.Touch.LeanPinchScale scaleScript;
    public Lean.Touch.LeanDragTranslate dragScript;
    
    public Lean.Touch.LeanTwistRotateAxis rotateScript;
    bool isItLocked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UnlockInteraction()
    {
        isItLocked = true;
        LockMechanism();
    }
    public void LockMechanism()
    {
        Text txt = transform.Find("Text").GetComponent<Text>();
        ColorBlock colors = GetComponent<Button>().colors;
        Color32 unpressed = new Color32(0,0,0,255);
        Color32 locked = new Color32(255,0,0,100);
        if (isItLocked == false)
        {   
            isItLocked = true;
            //txt.text = "Locked";
            colors.normalColor = locked;
            colors.highlightedColor = locked;
            colors.selectedColor = locked;
            button.colors = colors;
            button.image.sprite = lockedSprite;
            scaleScript.enabled = false;
            dragScript.enabled = false;
            rotateScript.enabled = false;
            planeFinder.SetActive(false);

        }
        else
        {
            isItLocked = false;
            //txt.text = "Unlocked";
            colors.normalColor = unpressed;
            colors.highlightedColor = unpressed;
            colors.selectedColor = unpressed;
            button.colors = colors;
            button.image.sprite = unlockedSprite;
            scaleScript.enabled = true;
            dragScript.enabled = true;
            rotateScript.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
