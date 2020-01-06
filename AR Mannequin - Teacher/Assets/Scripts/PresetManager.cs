using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Created by Kimberly Burke, 2019
/// </summary>
public class PresetManager : MonoBehaviour
{
    [SerializeField] CommandSend commandSend;
    [SerializeField] FacilitatorControls canvasControls;
    Dictionary<string, PresetJSON> availPresetJSON;
    // Preset (EyeState leftEye, EyeState rightEye, LipColor lips, bool vomit, ScarState scar, HeartSounds heart, LungSounds leftLung, LungSounds rightLung, float[] ecg)
    List<Command> commandList;

    private void Start()
    {
        string jsonTextFile = Resources.Load<TextAsset>("presets").text;
        PresetList presetList = JsonUtility.FromJson<PresetList>(jsonTextFile);
        availPresetJSON = new Dictionary<string, PresetJSON>();
        foreach (PresetJSON preset in presetList.presetList)
        {
            availPresetJSON.Add(preset.name, preset);
        }
    }

    #region Preset JSON
    public void ApplyPresetJSON(string name)
    {
        PresetJSON selectedPreset = availPresetJSON[name];
        Debug.Log("Running preset: " + selectedPreset.name);
        commandList = new List<Command>(); // clear command list

        ApplyEyeEvent("left", selectedPreset.left_eye.ToLower());
        ApplyEyeEvent("right", selectedPreset.right_eye.ToLower());

        ApplyLipEvent(selectedPreset.lip_color);
        commandList.Add(new Command(EventCodeUtility.VOMIT_TOGGLE, new object[] { selectedPreset.vomit_state }));
        ApplyScarEvent(selectedPreset.body_scar.ToLower());

        ApplyHeartTrack(selectedPreset.heart_track);
        ApplyLungTrack("left", selectedPreset.left_lung_track);
        ApplyLungTrack("right", selectedPreset.right_lung_track);

        // cast float values into type object for ecg
        object[] ecg = new object[] { 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < ecg.Length; i++)
        {
            ecg[i] = (object)selectedPreset.ecg[i];
        }
        commandList.Add(new Command(EventCodeUtility.SLIDE_EVENT, ecg));

        canvasControls.MatchPreset(selectedPreset);
        commandSend.ApplyCommands(commandList);
    }

    /// <summary>
    /// Used for determing what command to send for the eye status preset
    /// </summary>
    /// <param name="eye"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    private void ApplyEyeEvent(string eye, string status)
    {
        if (eye == "right")
        {
            switch (status)
            {
                case "normal":
                    commandList.Add(new Command(EventCodeUtility.DILATE_TOGGLE_R, new object[] { false }));
                    break;
                case "dilate":
                    commandList.Add(new Command(EventCodeUtility.DILATE_TOGGLE_R, new object[] { true }));
                    break;
                case "constrict":
                    commandList.Add(new Command(EventCodeUtility.CONSTRICT_TOGGLE_R, new object[] { true }));
                    break;
                default:
                    Debug.LogWarning("Not applicable right eye event.");
                    break;
            }
        }
        else if (eye == "left")
        {
            switch (status)
            {
                case "normal":
                    commandList.Add(new Command(EventCodeUtility.DILATE_TOGGLE_L, new object[] { false }));
                    break;
                case "dilate":
                    commandList.Add(new Command(EventCodeUtility.DILATE_TOGGLE_L, new object[] { true }));
                    break;
                case "constrict":
                    commandList.Add(new Command(EventCodeUtility.CONSTRICT_TOGGLE_L, new object[] { true }));
                    break;
                default:
                    Debug.LogWarning("Not applicable left eye event.");
                    break;
            }
        }
    }

    /// <summary>
    /// Used for determining what command to send for lip color preset
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    private void ApplyLipEvent(string status)
    {
        switch (status)
        {
            case "normal":
                commandList.Add(new Command(EventCodeUtility.PURPLE_TOGGLE, new object[] { false }));
                break;
            case "purple":
                commandList.Add(new Command(EventCodeUtility.PURPLE_TOGGLE, new object[] { true }));
                break;
            default:
                Debug.LogWarning("Not applicable lip event.");
                break;
        }
    }

    /// <summary>
    /// Used for determining what command to send for scar event
    /// </summary>
    /// <param name="status"></param>
    private void ApplyScarEvent(string status)
    {
        switch (status)
        {
            case "off":
                commandList.Add(new Command(EventCodeUtility.BODY_SCAR_TOGGLE, new object[] { false }));
                break;
            case "on":
                commandList.Add(new Command(EventCodeUtility.BODY_SCAR_TOGGLE, new object[] { true }));
                break;
            case "bleeding":
                commandList.Add(new Command(EventCodeUtility.BODY_SCAR_TOGGLE, new object[] { true }));
                commandList.Add(new Command(EventCodeUtility.BLEED_TOGGLE, new object[] { true }));
                break;
            default:
                Debug.LogWarning("Not applicable body scar event.");
                break;
        }
    }

    /// <summary>
    /// Used to determine what command to send for adjusting heart track
    /// 
    /// TODO - change track/audio clip command
    /// </summary>
    /// <param name="track"></param>
    private void ApplyHeartTrack(string track)
    {
        if (track == "off")
        {
            commandList.Add(new Command(EventCodeUtility.HEARTBEAT_TOGGLE, new object[] { true }));
            // NOTE: true is for turning on mute
        }
        else
        {
            commandList.Add(new Command(EventCodeUtility.HEARTBEAT_TOGGLE, new object[] { false }));
            commandList.Add(new Command(EventCodeUtility.HEARTBEAT_SOURCE, new object[] { SoundRepo.GetFileName(track, "HEART") }));
        }
    }

    /// <summary>
    /// Used to determine what command to send for adjusting lung track
    /// 
    /// TODO - change track/audio clip command
    /// </summary>
    /// <param name="lung"></param>
    /// <param name="track"></param>
    private void ApplyLungTrack(string lung, string track)
    {
        if (lung == "left")
        {
            switch (track)
            {
                case "off":
                    commandList.Add(new Command(EventCodeUtility.LUNG_L_TOGGLE, new object[] { true }));
                    break;
                case "Normal":
                    commandList.Add(new Command(EventCodeUtility.LUNG_L_TOGGLE, new object[] { false }));
                    // TODO - select track to be Regular
                    break;
                default:
                    Debug.LogWarning("Not applicable left lung soundtrack event.");
                    break;
            }
        }
        else if (lung == "right")
        {
            switch (track)
            {
                case "off":
                    commandList.Add(new Command(EventCodeUtility.LUNG_R_TOGGLE, new object[] { true }));
                    break;
                case "Normal":
                    commandList.Add(new Command(EventCodeUtility.LUNG_R_TOGGLE, new object[] { false }));
                    // TODO - select track to be Regular
                    break;
                default:
                    Debug.LogWarning("Not applicable right lung soundtrack event.");
                    break;
            }
        }
    }
    #endregion
}
