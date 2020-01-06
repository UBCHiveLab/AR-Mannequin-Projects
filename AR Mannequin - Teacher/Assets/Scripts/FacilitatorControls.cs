using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Created by Kimberly Burke, 2019
///
/// Controls the visuals of the facilitator canvas controls (ECG monitor values & active control panels)
/// </summary>
public class FacilitatorControls : MonoBehaviour
{
    [SerializeField] Toggle[] leftEyeToggles; // [constrict, dilate]
    [SerializeField] Toggle[] rightEyeToggles; // [constrict, dilate]
    [SerializeField] Toggle[] scarToggles; // [on, bleed]
    [SerializeField] Toggle[] lipToggles; // [purple, blue]
    [SerializeField] Toggle vomitToggle;

    [SerializeField] Toggle[] muteToggles; // [heart, left lung, right lung]
    [SerializeField] Dropdown[] soundtracks; // [heart, left lung, right lung]

    [SerializeField] private Slider[] sliders; // [bpm, sys, dia, oxy, res, tem]
    [SerializeField] private Text[] sliderValues; // [bpm, sys, dia, oxy, res, tem]

    [SerializeField] private Text[] ecgValues;

    [SerializeField] private GameObject[] contentPanels;

    // Start is called before the first frame update
    void Start()
    {
        SwitchPanel(2); // must start on ECG monitor panel for sliders to be initialized before presets
        ReceiveECGValues();
    }

    // Called only when Apply is clicked for ECG event
    public void ReceiveECGValues()
    {
        for (int i = 0; i < sliders.Length; i++) {
            sliderValues[i].text = sliders[i].value.ToString();
            ecgValues[i].text = sliders[i].value.ToString();
        }
    }

    private void ReceiveECGValues(float[] ecg)
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            sliderValues[i].text = ecg[i].ToString();
            sliders[i].value = ecg[i];
            ecgValues[i].text = ecg[i].ToString();
        }
    }

    public void SwitchPanel(int selectedIndex)
    {
        foreach(GameObject panel in contentPanels)
        {
            panel.SetActive(false);
        }
        contentPanels[selectedIndex].SetActive(true);
    }

    #region Preset JSON
    public void MatchPreset(PresetJSON selectedPreset)
    {
        // visual
        MatchEyeToggle(leftEyeToggles, selectedPreset.left_eye.ToLower());
        MatchEyeToggle(rightEyeToggles, selectedPreset.right_eye.ToLower());
        MatchLipToggle(selectedPreset.lip_color.ToLower());
        MatchScarToggle(selectedPreset.body_scar.ToLower());
        vomitToggle.isOn = selectedPreset.vomit_state;

        // audio
        MatchHeartSetting(selectedPreset.heart_track);
        MatchLungSetting("left", selectedPreset.left_lung_track);
        MatchLungSetting("right", selectedPreset.right_lung_track);

        // monitor
        ReceiveECGValues(selectedPreset.ecg);
    }

    private void MatchEyeToggle(Toggle[] group, string status)
    {
        // set to default
        foreach (Toggle toggle in group)
        {
            toggle.isOn = false;
        }

        switch (status)
        {
            case "normal":
                break;
            case "dilate":
                group[1].isOn = true;
                break;
            case "constrict":
                group[0].isOn = true;
                break;
            default:
                break;
        }
    }

    private void MatchLipToggle(string status)
    {
        // set to default
        foreach (Toggle toggle in lipToggles)
        {
            toggle.isOn = false;
        }
        switch (status)
        {
            case "normal":
                break;
            case "purple":
                lipToggles[0].isOn = true;
                break;
            default:
                break;
        }
    }

    private void MatchScarToggle(string status)
    {
        // set to default
        foreach (Toggle toggle in scarToggles)
        {
            toggle.isOn = false;
        }
        scarToggles[1].interactable = false;

        switch (status)
        {
            case "off":
                break;
            case "on":
                scarToggles[0].isOn = true;
                scarToggles[1].interactable = true;
                break;
            case "bleeding":
                scarToggles[0].isOn = true;
                scarToggles[1].isOn = true;
                scarToggles[1].interactable = true;
                break;
            default:
                break;
        }
    }

    private void MatchHeartSetting(string status)
    {

        if (status == "off")
        {
            muteToggles[0].isOn = true;
        }
        else
        {
            muteToggles[0].isOn = false;
            List<Dropdown.OptionData> list = soundtracks[0].options;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].text.Equals(status)) {
                    soundtracks[0].value = i;
                }

            }
        }
    }

    private void MatchLungSetting(string lung, string status)
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
            case "off":
                muteToggles[index].isOn = true;
                break;
            case "Normal":
                muteToggles[index].isOn = false;
                // TODO - set audio track change
                break;
            default:
                break;
        }
    }
    #endregion

    public void QuitApplication()
    {
        // reloads to start menu on quit
        SceneManager.LoadScene(0);
    }
}
