using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Created by Dante Cerron
/// 
/// Reads through text file and outputs a ??? Dictionary/List for all objects that toggle between an active and deactive state
/// </summary>
public class Parse : Singleton<Parse> {

    public Dictionary<string, List<string>> SwitchButtons;

	// Use this for initialization
	void Start () {
        SwitchButtons = ParseLOSDictionary();
	}

    private Dictionary<string, List<string>> ParseLOSDictionary()
    {
        Dictionary<string, List<string>> parsedGroups = new Dictionary<string, List<string>>();
        TextAsset groupsTextAsset = Resources.Load<TextAsset>("Configs/SwitchButtons");
        String groupsRaw = groupsTextAsset.text;
        string[] groups = groupsRaw.Split('\n');
        foreach (string membersAndGroupsRaw in groups)
        {
            string[] membersAndGroupsArray = membersAndGroupsRaw.Split(':');
            parsedGroups.Add(stringTrimmer(membersAndGroupsArray[0]), LOSTrimmer(membersAndGroupsArray[1].Split(',').ToList()));
        }
        return parsedGroups;
    }

    public Dictionary<string, Renderer> Switching(string suffix)
    {
        Dictionary<string, Renderer> info = new Dictionary<string, Renderer>();
        if (SwitchButtons.ContainsKey(suffix))
        {
            foreach (string i in SwitchButtons[suffix])
            {
                var curGo = GameObject.Find(stringTrimmer(i));
                if (curGo != null)
                {
                    info.Add(stringTrimmer(i), curGo.GetComponent<Renderer>());
                }
                else
                {
                    Debug.Log("no gameobject of name " + i + "in scene");
                }
            }
        } else
        {
            Debug.Log("given suffix doesn't exist: " + suffix);
        }
        return info;


        /*
        Dictionary<string, Renderer> info = new Dictionary<string, Renderer>();
        TextAsset infoResources = Resources.Load<TextAsset>("Configs/Switch" + suffix);
        var infoRaw = infoResources.text;
        string[] infoArray = infoRaw.Split(',');
        foreach (string i in infoArray)
        {
            var curGo = GameObject.Find(stringTrimmer(i));
            if (curGo != null)
            {
                info.Add(stringTrimmer(i), curGo.GetComponent<Renderer>());
            } else
            {
                Debug.Log("no gameobject of name " + i + "in scene");
            }
        }
        return info;
        */
    }

    public string stringTrimmer(string str)
    {
        str = new string(str.Where(c => !char.IsControl(c)).ToArray());
        return str;
    }

    public List<string> LOSTrimmer(List<string> los)
    {
        List<string> trimmed_los = new List<string>();
        foreach (string str in los)
        {
            string trimmed_str = stringTrimmer(str);
            trimmed_los.Add(trimmed_str);
        }
        return trimmed_los;
    }
}
