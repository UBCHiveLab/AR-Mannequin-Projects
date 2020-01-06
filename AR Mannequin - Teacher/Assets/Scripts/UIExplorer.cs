using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OBSOLETE
/// Created by Dante Cerron and Kimberly Burke 2019
/// 
/// Contains reference to all UI elements and list of new commands to CommandSend
/// </summary>
public class UIExplorer : MonoBehaviour
{
    private UIElement[] elements;
    private UIMultiElement[] multiElements;
    [SerializeField] private CommandSend cmdSend;

    // Start is called before the first frame update
    void Start()
    {
        elements = GetComponentsInChildren<UIElement>();
        multiElements = GetComponentsInChildren<UIMultiElement>();
    }

    /// <summary>
    /// Iterates through all UI elements and checks if the values have changed before adding to new command list.
    /// 
    /// Sends new command list to CommandSend
    /// </summary>
    public void OnApplyClicked()
    {
        List<Command> newCommands = new List<Command>();
        // single data elements
        foreach(UIElement el in elements)
        {
            if (el.oldValue != el.newValue)
            {
                newCommands.Add(new Command(el.GetEventCode(), new object[] { el.newValue }));
                el.oldValue = el.newValue; // update the old value to the change
            }
        }
        // multiple data elements
        foreach(UIMultiElement multiEl in multiElements)
        {
            newCommands.Add(new Command(multiEl.GetEventCode(), multiEl.values));
        }
        cmdSend.ApplyCommands(newCommands);
    }
}
