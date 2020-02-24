using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Dante Cerron and Kimberly Burke 2019
/// 
/// Hold values of UI elements. Attached to Unity UI.
/// </summary>
public class UIElement : MonoBehaviour
{
    [SerializeField] private byte evCode;
    public object oldValue;
    public object newValue;
    private CommandSend cmdSend;
    private Command cmd;

    private void Start()
    {
        cmdSend = GameObject.FindGameObjectWithTag("Player").GetComponent<CommandSend>();
        
    }

    public void ChangeToggleValue()
    {
        newValue = GetComponent<Toggle>().isOn;
        ApplyCommand();
    }

    public void ChangeIntValue()
    {
        newValue = GetComponent<Slider>().value;
        ApplyCommand();
    }

    public void ChangeStringValue()
    {
        newValue = GetComponent<Dropdown>().captionText.text;
        ApplyCommand();
    }

    /// <summary>
    /// Used for SoundRepo dropdowns that have a display name separate from file name
    /// </summary>
    /// <param name="name"></param>
    public void ChangeStringValue(string name)
    {
        newValue = name;
        ApplyCommand();
    }

    public byte GetEventCode()
    {
        return evCode;
    }

    /// <summary>
    /// Created by Silver Xu, to allow change of event code in case there are multiple events using the same dropdown UI
    /// </summary>
    /// <param name="eventCode"></param>
    public void ChangeEventCode(byte eventCode)
    {
        evCode = eventCode;
    }

    private void ApplyCommand()
    {
        
        cmd = new Command(evCode, new object[] { newValue });
        if(cmdSend!=null)
        cmdSend.ApplyCommand(cmd); 
    }
}
