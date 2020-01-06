using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundDropdownOptions : MonoBehaviour
{
    [SerializeField] string type;
    Dropdown SoundDropdown;
    void Start()
    {
        SoundDropdown = GetComponent<Dropdown>();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        Dictionary<string, List<string>> images = SoundRepo.GetSoundLookup();
        if(images[type] != null)
        {
            foreach (string item in images[type])
            {
                item.Replace("_", " ");
                options.Add(new Dropdown.OptionData(item));
            }
        }
        SoundDropdown.AddOptions(options);
    }

    public string GetSoundType()
    {
        return type;
    }
}
