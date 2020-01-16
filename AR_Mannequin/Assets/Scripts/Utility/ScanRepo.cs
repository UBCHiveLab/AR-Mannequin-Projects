using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Video;

public static class ScanRepo
{
/// <summary>
/// written by Dante Cerron HIVE Lab UBC. 
/// beginning of repository for accessing scans information and retrieving image data
/// </summary>

    enum ScanType { CT, XRAY, ULTRASOUND };

    private class Scan
    {
        public string type;
        public string name;
        public Scan(ScanType type, string name)
        {
            //convert enum type to string for easy reference in dictionary
            this.type = Enum.GetName(typeof(ScanType), type);
            this.name = name;
        }
    }

    /// <summary>
    /// THIS LIST NEEDS TO BE THE SAME BETWEEN FACILITATOR AND STUDENT
    /// </summary>
    private static readonly List<Scan> availableScans = new List<Scan>{
        new Scan(ScanType.CT, "Meningioma"),
        new Scan(ScanType.CT, "SAH2"),
        new Scan(ScanType.CT, "Subdural"),
        new Scan(ScanType.XRAY, "ARDS"),
        new Scan(ScanType.XRAY, "Chest Xray Normal"),
        new Scan(ScanType.XRAY, "Pleural Effusion Pneumothorax"),
        new Scan(ScanType.XRAY, "Pneumonia-LLL Xray"),
        new Scan(ScanType.XRAY, "Pneumothorax"),
        new Scan(ScanType.XRAY, "Tension Pneumothorax"),
        new Scan(ScanType.ULTRASOUND, "Free Fluid"),
        new Scan(ScanType.ULTRASOUND, "Fluid Spleen"),
        new Scan(ScanType.ULTRASOUND, "Free Fluid Video"),
        new Scan(ScanType.ULTRASOUND, "Fluid Spleen Video")
    };

    /// <summary>
    /// Beginning of interface for getting images from resources folder
    /// </summary>
    /// <param name="type">subtype of image (ct, xray, etc)</param>
    /// <param name="name">name of specific image within type</param>
    /// <returns>A sprite if the image is found, null otherwise</returns>
    public static Sprite GetImage(string type, string name)
    {
        try
        {
            return Resources.Load<Sprite>("ImageDisplay/" + type + "/" + name);
        } catch(Exception e)
        {
            Debug.Log("Error getting image for display: " + e.Message);
            return null;
        }
    }
    public static VideoClip GetVideo(string type,string name)
    {
        try
        {
            return Resources.Load<VideoClip>("ImageDisplay/" + type + "/" + name);
        }
        catch (Exception e)
        {
            Debug.Log("Error getting video for display: " + e.Message);
            return null;
        }
    }

    public static List<string> GetAvailableTypes()
    {
        List<string> result = new List<string>();
        foreach(string item in Enum.GetNames(typeof(ScanType)))
        {
            result.Add(item);
        }

        return result;
    }

    /// <summary>
    /// Dictionary for easy image lookup and menu generation
    /// </summary>
    /// <returns>a dictionary with scan types as key, and list of names as val</returns>
    public static Dictionary<string, List<string>> GetImageLookup()
    {
        Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
        foreach(string type in GetAvailableTypes())
        {
            result.Add(type, new List<string>());
        }

        foreach(Scan item in availableScans)
        {
            result[item.type].Add(item.name);
        }

        return result;
    }
}
