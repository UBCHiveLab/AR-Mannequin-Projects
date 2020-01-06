using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkinRepo : MonoBehaviour
{

    private class SkinColor
    {
        public string name;
        public SkinColor(string name)
        {
            this.name = name;
        }
    }

    private static readonly List<SkinColor> availableColors = new List<SkinColor>{
        new SkinColor("Pale"),
        new SkinColor("Blue"),
        new SkinColor("Regular"),
    };


    public static Material GetColor(string name)
    {
        try
        {
            return Resources.Load<Material>("SkinMaterials/" + name);
        }
        catch (Exception e)
        {
            Debug.Log("Error getting image for display: " + e.Message);
            return null;
        }
    }

    public static List<string> GetColorLookup()
    {
        List<string> result = new List<string>();

        foreach (SkinColor item in availableColors)
        {
            result.Add(item.name);
        }

        return result;
    }
}
