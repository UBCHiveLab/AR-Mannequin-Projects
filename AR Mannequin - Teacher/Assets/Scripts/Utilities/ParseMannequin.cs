using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Kimberly Burke 2019
/// 
/// Parses text file with a list of the empty parent/group objects and the children/part game objects
/// </summary>
public class ParseMannequin : MonoBehaviour
{
    public Dictionary<string, List<string>> parts;

    // Start is called before the first frame update
    void Start()
    {
        parts = ParseMannequinFile();
    }

    private Dictionary<string, List<string>> ParseMannequinFile()
    {
        Dictionary<string, List<string>> parsedGroups = new Dictionary<string, List<string>>();
        TextAsset groupsTextAsset = Resources.Load<TextAsset>("Configs/MannequinParts");
        string rawText = groupsTextAsset.text;
        string[] groupArr = rawText.Split('\n');
        foreach (string bodyPart in groupArr)
        {
            // TODO
        }
        return parsedGroups;
    }
}
