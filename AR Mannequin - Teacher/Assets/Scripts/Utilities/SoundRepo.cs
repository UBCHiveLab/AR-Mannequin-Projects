using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundRepo : MonoBehaviour
{
    enum SoundType { HEART, LUNG, AMBIENT, MONITOR };

    private class Sound
    {
        public string type;
        public string name;
        public string display;
        public Sound(SoundType type, string name, string displayName)
        {
            //convert enum type to string for easy reference in dictionary
            this.type = Enum.GetName(typeof(SoundType), type);
            this.name = name;
            this.display = displayName;
        }
    }

    private static readonly List<Sound> availableSounds = new List<Sound>{
        new Sound(SoundType.HEART, "heart_placeholder", "Normal"),
        new Sound(SoundType.HEART, "Heart - Normal Split S1", "Normal S1"),
        new Sound(SoundType.HEART, "Heart - Normal Split Second Sound", "Normal S2"),
        new Sound(SoundType.HEART, "heart_placeholder_notLoud", "Quiet"),
        new Sound(SoundType.HEART, "Aortic", "Aortic"),
        new Sound(SoundType.HEART, "Mitral", "Mitral"),
        new Sound(SoundType.HEART, "Pulmonary", "Pulmonary"),
        new Sound(SoundType.HEART, "Tricuspid", "Tricuspid"),
        new Sound(SoundType.HEART, "Heart - Diastolic Rumble", "Diastolic Rumble"),
        new Sound(SoundType.HEART, "Heart - Early Systolic Murmur", "Early Systolic Murmur"),
        new Sound(SoundType.HEART, "Heart - Ejection Click", "Ejection Click"),
        new Sound(SoundType.HEART, "Heart - Opening Snap", "Opening Snap"),
        new Sound(SoundType.HEART, "Heart - Pansystolic Murmur", "Pansystolic Murmur"),
        new Sound(SoundType.HEART, "Heart - S3", "S3"),
        new Sound(SoundType.HEART, "Heart - S4", "S4"),
        new Sound(SoundType.LUNG, "Lung - Normal Vesicular", "Normal"),
        new Sound(SoundType.LUNG, "Lung - Wheezing", "Wheezing"),
        new Sound(SoundType.LUNG, "Lung - Pleural Friction", "Pleural Friction"),
        new Sound(SoundType.LUNG, "Lung - Inspiratory Stridor", "Inspiratory Stridor"),
        new Sound(SoundType.LUNG, "Lung - Coarse Crackles", "Coarse Crackles"),
        new Sound(SoundType.MONITOR, "ar-cpr-ekg-sound_deadBeep", "Dead"),
        new Sound(SoundType.MONITOR, "ar-cpr-ekg-sound_singleBeep", "Normal"),
        new Sound(SoundType.MONITOR, "ar-cpr-ekg-sound_warningBeep", "Warning"),
        new Sound(SoundType.AMBIENT, "EmergencyRoomBackgroundNoise", "ER Noise")
    };
    /// <summary>
    /// Beginning of interface for getting images from resources folder
    /// </summary>
    /// <param name="type">subtype of image (ct, xray, etc)</param>
    /// <param name="name">name of specific image within type</param>
    /// <returns>A sprite if the image is found, null otherwise</returns>
    public static AudioClip GetSound(string type, string name)
    {
        try
        {
            return Resources.Load<AudioClip>("AudioClips/" + type + "/" + name);
        }
        catch (Exception e)
        {
            Debug.Log("Error getting image for display: " + e.Message);
            return null;
        }
    }

    public static List<string> GetAvailableTypes()
    {
        List<string> result = new List<string>();
        foreach (string item in Enum.GetNames(typeof(SoundType)))
        {
            result.Add(item);
        }

        return result;
    }

    /// <summary>
    /// Dictionary for easy sound lookup and menu generation
    /// </summary>
    /// <returns>a dictionary with scan types as key, and list of names as val</returns>
    public static Dictionary<string, List<string>> GetSoundLookup()
    {
        Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
        foreach (string type in GetAvailableTypes())
        {
            result.Add(type, new List<string>());
        }

        foreach (Sound item in availableSounds)
        {
            result[item.type].Add(item.display);
        }

        return result;
    }

    public static string GetFileName(string displayName, string type)
    {
        string result = "";
        foreach (Sound item in availableSounds)
        {
            if (item.display == displayName && type == item.type)
            {
                result = item.name;
            }
        }
        return result;
    }
}
