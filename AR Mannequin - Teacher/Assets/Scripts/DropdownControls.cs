using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownControls : MonoBehaviour
{
    private UIElement element;
    [SerializeField] Image display;
    private Sprite selectedImage;

    private void Start()
    {
        element = GetComponent<UIElement>();
    }

    public void SendValues(Dropdown change)
    {
        Debug.Log("sending image change event with: " + change.captionText.text);
        element.ChangeStringValue();
    }

    public void SendSoundValues(Dropdown change)
    {
        string type = GetComponent<SoundDropdownOptions>().GetSoundType();
        string fileName = SoundRepo.GetFileName(change.captionText.text, type);
        Debug.Log("Sending audio clip event with: " + fileName);
        element.ChangeStringValue(fileName);
    }

    public void GetScanDisplay(Dropdown change)
    {
        ScanRepo.ScanType type = GetComponent<ImageDropdownOptions>().GetScanType();
        selectedImage = ScanRepo.GetImage(Enum.GetName(typeof(ScanRepo.ScanType), type), change.captionText.text);
        display.sprite = selectedImage;
    }
}
