using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class ImageDropdownOptions : MonoBehaviour
{
    //not using image type for further ordering for now
    Dropdown ImageDropdown;
    [SerializeField] ScanRepo.ScanType type;

    void Start()
    {
        ImageDropdown = GetComponent<Dropdown>();
        List<Dropdown.OptionData> imageOptions = new List<Dropdown.OptionData>();
        Dictionary<string, List<string>> images = ScanRepo.GetImageLookup();
        imageOptions.Add(new Dropdown.OptionData("None"));

        // Currently only functional for CT scan type images.
        foreach(List<string> list in images.Values)
        {
            foreach(string item in list)
            {
                imageOptions.Add(new Dropdown.OptionData(item));
            }
        }
        ImageDropdown.AddOptions(imageOptions);
    }

    public ScanRepo.ScanType GetScanType()
    {
        return type;
    }
}
