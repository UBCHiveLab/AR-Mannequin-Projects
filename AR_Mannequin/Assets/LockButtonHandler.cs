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
    bool isItLocked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LockMechanism()
    {
        Text txt = transform.Find("Text").GetComponent<Text>();
        Image img = transform.Find("Image").GetComponent<Image>();
        ColorBlock colors = GetComponent<Button>().colors;
        Color32 unpressed = new Color32(255,255,255,255);
        Color32 locked = new Color32(255,0,0,100);
        if (isItLocked == false)
        {   
            isItLocked = true;
            //txt.text = "Locked";
            colors.normalColor = locked;
            colors.highlightedColor = locked;
            button.colors = colors;
            img.sprite = lockedSprite;
            scaleScript.enabled = false;
            dragScript.enabled = false;
            planeFinder.SetActive(false);

        }
        else
        {
            isItLocked = false;
            //txt.text = "Unlocked";
            colors.normalColor = unpressed;
            colors.highlightedColor = unpressed;
            button.colors = colors;
            img.sprite = unlockedSprite;
            scaleScript.enabled = true;
            dragScript.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
