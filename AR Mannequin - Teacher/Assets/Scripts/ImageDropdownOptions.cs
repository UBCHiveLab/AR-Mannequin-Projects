using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//[RequireComponent(typeof(Dropdown))]
public class ImageDropdownOptions : MonoBehaviour
{
    //not using image type for further ordering for now
    Dropdown ImageDropdown;
    public static ScanRepo.ScanType type = ScanRepo.ScanType.CT;
    [SerializeField]
    private GameObject dropdown;
    private byte evCode;

    public void ChangeDropDownOption(String buttonType)
    {
        //Set image type by click on buttons
        type = (ScanRepo.ScanType)Enum.Parse(typeof(ScanRepo.ScanType), buttonType);
        
        UIElement ImageDropdownUI = dropdown.GetComponent<UIElement>();
        ImageDropdown = dropdown.GetComponent<Dropdown>();
        ImageDropdown.ClearOptions();
        
        //change evCode based on image type seleceted
        switch (buttonType)
        {
            case "CT":
                evCode = 25;
                break;
            case "XRAY":
                evCode = 24;
                break;
            case "ULTRASOUND":
                evCode = 31;
                break;
        }
        ImageDropdownUI.ChangeEventCode(evCode);
        
        List<Dropdown.OptionData> imageOptions = new List<Dropdown.OptionData>();
        Dictionary<string, List<string>> images = ScanRepo.GetImageLookup();
        imageOptions.Add(new Dropdown.OptionData("None"));

        // Shows options with corresponding type
        foreach(KeyValuePair<string, List<string>> image in images)
        {
            if (image.Key == type.ToString())
            {
                foreach(string item in image.Value)
                {
                    imageOptions.Add(new Dropdown.OptionData(item));
                }
            }
        }
        ImageDropdown.AddOptions(imageOptions);
        //reset dropdown option
        ImageDropdown.value=0;
    }
  

 
}
