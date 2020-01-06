using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinDropdownOptions : MonoBehaviour
{
    Dropdown SkinDropdown;
    // Start is called before the first frame update
    void Start()
    {
        SkinDropdown = GetComponent<Dropdown>();
        List<Dropdown.OptionData> skinOptions = new List<Dropdown.OptionData>();
        List<string> colors = SkinRepo.GetColorLookup();
        foreach (string item in colors)
        {
            skinOptions.Add(new Dropdown.OptionData(item));
        }
        SkinDropdown.AddOptions(skinOptions);
    }
}
