using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundRepo : MonoBehaviour
{
    enum SoundType { HEART, LUNG, AMBIENT, MONITOR, VOMIT };

    private class Sound
    {
        public string type;
        public string name;
        public Sound(SoundType type, string name)
        {
            //convert enum type to string for easy reference in dictionary
            this.type = Enum.GetName(typeof(SoundType), type);
            this.name = name;
        }
    }

    private static readonly List<Sound> availableSounds = new List<Sound>{
        new Sound(SoundType.HEART, "Aortic"),
        new Sound(SoundType.HEART, "heart_placeholder"),
        new Sound(SoundType.HEART, "heart_placeholder_notLoud"),
        new Sound(SoundType.HEART, "Mitral"),
        new Sound(SoundType.HEART, "Aortic"),
        new Sound(SoundType.HEART, "Pulmonary"),
        new Sound(SoundType.HEART, "Tricuspid"),
        new Sound(SoundType.HEART, "Aortic"),
        new Sound(SoundType.LUNG, "NormalBreathSound"),
        new Sound(SoundType.MONITOR, "ar-cpr-ekg-sound_deadBeep"),
        new Sound(SoundType.MONITOR, "ar-cpr-ekg-sound_singleBeep"),
        new Sound(SoundType.MONITOR, "ar-cpr-ekg-sound_warningBeep"),
        new Sound(SoundType.AMBIENT, "hospitalNoise"),
        new Sound(SoundType.VOMIT,"breviceps__gagging-vomit-throwing-up")
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
            result[item.type].Add(item.name);
        }

        return result;
    }
}
