using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPresetManager : MonoBehaviour
{

    [SerializeField] CommandSend commandSend;
    [SerializeField] FacilitatorControls canvasControls;

    Dictionary<string, Preset> availPreset;
    List<Command> commandList;

    // Start is called before the first frame update
    void Start()
    {
        availPreset = new Dictionary<string, Preset>();
        LoadPresets();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Preset Events
    /// <summary>
    /// TODO - take in text file to create new Preset JSON
    /// </summary>
    public void LoadPresets()
    {
        Preset defaultValues = new Preset(Preset.EyeState.Normal, Preset.EyeState.Normal, Preset.LipColor.Normal, false, Preset.ScarState.Off,
            Preset.HeartSounds.Off, Preset.LungSounds.Off, Preset.LungSounds.Off, new float[] { 120, 120, 80, 98, 35, 36 });

        availPreset.Add("default", defaultValues);
    }

    public void ApplyPreset(string name)
    {
        Preset selectedPreset = availPreset[name];
        commandList = new List<Command>(); // clear command list

        ApplyEyeEvent("left", selectedPreset.left_eye);
        ApplyEyeEvent("right", selectedPreset.right_eye);

        ApplyLipEvent(selectedPreset.lip_color);
        commandList.Add(new Command(EventCodeUtility.VOMIT_TOGGLE, new object[] { selectedPreset.vomit_state }));
        ApplyScarEvent(selectedPreset.body_scar);

        ApplyHeartTrack(selectedPreset.heart_track);
        ApplyLungTrack("left", selectedPreset.left_lung_track);
        ApplyLungTrack("right", selectedPreset.right_lung_track);

        // cast float values into type object for ecg
        object[] ecg = new object[] { 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < ecg.Length; i++)
        {
            ecg[i] = (object)selectedPreset.ecgValues[i];
        }
        commandList.Add(new Command(EventCodeUtility.SLIDE_EVENT, ecg));

        // canvasControls.MatchPreset(selectedPreset);
        commandSend.ApplyCommands(commandList);
    }

    /// <summary>
    /// Used for determing what command to send for the eye status preset
    /// </summary>
    /// <param name="eye"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    private void ApplyEyeEvent(string eye, Preset.EyeState status)
    {
        if (eye == "right")
        {
            switch (status)
            {
                case Preset.EyeState.Normal:
                    commandList.Add(new Command(EventCodeUtility.DILATE_TOGGLE_R, new object[] { false }));
                    break;
                case Preset.EyeState.Dilate:
                    commandList.Add(new Command(EventCodeUtility.DILATE_TOGGLE_R, new object[] { true }));
                    break;
                case Preset.EyeState.Constrict:
                    commandList.Add(new Command(EventCodeUtility.CONSTRICT_TOGGLE_R, new object[] { true }));
                    break;
                default:
                    break;
            }
        }
        else if (eye == "left")
        {
            switch (status)
            {
                case Preset.EyeState.Normal:
                    commandList.Add(new Command(EventCodeUtility.DILATE_TOGGLE_L, new object[] { false }));
                    break;
                case Preset.EyeState.Dilate:
                    commandList.Add(new Command(EventCodeUtility.DILATE_TOGGLE_L, new object[] { true }));
                    break;
                case Preset.EyeState.Constrict:
                    commandList.Add(new Command(EventCodeUtility.CONSTRICT_TOGGLE_L, new object[] { true }));
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Used for determining what command to send for lip color preset
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    private void ApplyLipEvent(Preset.LipColor status)
    {
        switch (status)
        {
            case Preset.LipColor.Normal:
                commandList.Add(new Command(EventCodeUtility.PURPLE_TOGGLE, new object[] { false }));
                break;
            case Preset.LipColor.Purple:
                commandList.Add(new Command(EventCodeUtility.PURPLE_TOGGLE, new object[] { true }));
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Used for determining what command to send for scar event
    /// </summary>
    /// <param name="status"></param>
    private void ApplyScarEvent(Preset.ScarState status)
    {
        switch (status)
        {
            case Preset.ScarState.Off:
                commandList.Add(new Command(EventCodeUtility.BODY_SCAR_TOGGLE, new object[] { false }));
                break;
            case Preset.ScarState.On:
                commandList.Add(new Command(EventCodeUtility.BODY_SCAR_TOGGLE, new object[] { true }));
                break;
            case Preset.ScarState.Bleeding:
                commandList.Add(new Command(EventCodeUtility.BODY_SCAR_TOGGLE, new object[] { true }));
                commandList.Add(new Command(EventCodeUtility.BLEED_TOGGLE, new object[] { true }));
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Used to determine what command to send for adjusting heart track
    /// 
    /// TODO - change track/audio clip command
    /// </summary>
    /// <param name="track"></param>
    private void ApplyHeartTrack(Preset.HeartSounds track)
    {
        switch (track)
        {
            case Preset.HeartSounds.Off:
                commandList.Add(new Command(EventCodeUtility.HEARTBEAT_TOGGLE, new object[] { false }));
                break;
            case Preset.HeartSounds.Regular:
                commandList.Add(new Command(EventCodeUtility.HEARTBEAT_TOGGLE, new object[] { false }));
                // TODO - select track to be Regular
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Used to determine what command to send for adjusting lung track
    /// 
    /// TODO - change track/audio clip command
    /// </summary>
    /// <param name="lung"></param>
    /// <param name="track"></param>
    private void ApplyLungTrack(string lung, Preset.LungSounds track)
    {
        if (lung == "left")
        {
            switch (track)
            {
                case Preset.LungSounds.Off:
                    commandList.Add(new Command(EventCodeUtility.LUNG_L_TOGGLE, new object[] { false }));
                    break;
                case Preset.LungSounds.Regular:
                    commandList.Add(new Command(EventCodeUtility.LUNG_L_TOGGLE, new object[] { true }));
                    // TODO - select track to be Regular
                    break;
                default:
                    break;
            }
        }
        else if (lung == "right")
        {
            switch (track)
            {
                case Preset.LungSounds.Off:
                    commandList.Add(new Command(EventCodeUtility.LUNG_R_TOGGLE, new object[] { false }));
                    break;
                case Preset.LungSounds.Regular:
                    commandList.Add(new Command(EventCodeUtility.LUNG_R_TOGGLE, new object[] { true }));
                    // TODO - select track to be Regular
                    break;
                default:
                    break;
            }
        }
    }
    #endregion

    #region Preset UI 
    /*public void MatchPreset(Preset selectedPreset)
    {
        // visual
        MatchEyeToggle(leftEyeToggles, selectedPreset.left_eye);
        MatchEyeToggle(rightEyeToggles, selectedPreset.right_eye);
        MatchLipToggle(selectedPreset.lip_color);
        MatchScarToggle(selectedPreset.body_scar);
        vomitToggle.isOn = selectedPreset.vomit_state;

        // audio
        MatchHeartSetting(selectedPreset.heart_track);
        MatchLungSetting("left", selectedPreset.left_lung_track);
        MatchLungSetting("right", selectedPreset.right_lung_track);

        // monitor
        ReceiveECGValues(selectedPreset.ecgValues);
    }

    private void MatchEyeToggle(Toggle[] group, Preset.EyeState status)
    {
        // set to default
        foreach (Toggle toggle in group)
        {
            toggle.isOn = false;
        }

        switch (status)
        {
            case Preset.EyeState.Normal:
                break;
            case Preset.EyeState.Dilate:
                group[1].isOn = true;
                break;
            case Preset.EyeState.Constrict:
                group[0].isOn = true;
                break;
            default:
                break;
        }
    }

    private void MatchLipToggle(Preset.LipColor status)
    {
        // set to default
        foreach (Toggle toggle in lipToggles)
        {
            toggle.isOn = false;
        }
        switch (status)
        {
            case Preset.LipColor.Normal:
                break;
            case Preset.LipColor.Purple:
                lipToggles[0].isOn = true;
                break;
            default:
                break;
        }
    }

    private void MatchScarToggle(Preset.ScarState status)
    {
        // set to default
        foreach (Toggle toggle in scarToggles)
        {
            toggle.isOn = false;
        }
        scarToggles[1].interactable = false;

        switch (status)
        {
            case Preset.ScarState.Off:
                break;
            case Preset.ScarState.On:
                scarToggles[0].isOn = true;
                scarToggles[1].interactable = true;
                break;
            case Preset.ScarState.Bleeding:
                scarToggles[0].isOn = true;
                scarToggles[1].isOn = true;
                scarToggles[1].interactable = true;
                break;
            default:
                break;
        }
    }

    private void MatchHeartSetting(Preset.HeartSounds status)
    {
        switch (status)
        {
            case Preset.HeartSounds.Off:
                muteToggles[0].isOn = true;
                break;
            case Preset.HeartSounds.Regular:
                muteToggles[0].isOn = false;
                // TODO - set audio track change
                break;
            default:
                break;
        }
    }

    private void MatchLungSetting(string lung, Preset.LungSounds status)
    {
        int index = 0;
        if (lung == "left")
        {
            index = 1;
        }
        else if (lung == "right")
        {
            index = 2;
        }
        switch (status)
        {
            case Preset.LungSounds.Off:
                muteToggles[index].isOn = true;
                break;
            case Preset.LungSounds.Regular:
                muteToggles[index].isOn = false;
                // TODO - set audio track change
                break;
            default:
                break;
        }
    }*/
    #endregion
}


public class Preset
{
    public EyeState left_eye;
    public EyeState right_eye;
    public LipColor lip_color;
    public bool vomit_state;
    public ScarState body_scar;

    public float[] ecgValues = new float[6]; // bpm, sys, dia, oxy, res, temp

    public HeartSounds heart_track;
    public LungSounds left_lung_track;
    public LungSounds right_lung_track;

    public enum ScarState { Off, On, Bleeding };
    public enum EyeState { Normal, Dilate, Constrict };
    public enum LipColor { Normal, Purple };

    public enum AmbientSounds { Off, ER };
    public enum HeartSounds { Off, Regular };
    public enum LungSounds { Off, Regular };

    public Preset(EyeState leftEye, EyeState rightEye, LipColor lips, bool vomit, ScarState scar,
        HeartSounds heart, LungSounds leftLung, LungSounds rightLung, float[] ecg)
    {
        left_eye = leftEye;
        right_eye = rightEye;
        lip_color = lips;
        vomit_state = vomit;
        body_scar = scar;

        heart_track = heart;
        left_lung_track = leftLung;
        right_lung_track = rightLung;

        if (ecg.Length == ecgValues.Length)
        {
            ecgValues = ecg;
        }
        else
        {
            ecgValues = new float[6];
        }
    }
}