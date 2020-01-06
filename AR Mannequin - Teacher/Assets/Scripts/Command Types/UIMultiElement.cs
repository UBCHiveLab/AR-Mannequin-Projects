using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Kimberly Burke 2019
/// 
/// For UI elements that will pass more than one data value for a single command event
/// </summary>
public class UIMultiElement : MonoBehaviour
{
    [SerializeField] private byte evCode;
    [SerializeField] private int count;
    [SerializeField] private GameObject[] uiElements;
    public object[] values;

    private void Start()
    {
        values = new object[count];
        for (int i = 0; i < count; i++)
        {
            GameObject el = uiElements[i];
            // TODO: Currently only applicable for Slider ui
            if (el.GetComponent<Slider>() != null) { values[i] = el.GetComponent<Slider>().value; }
        }
    }

    public void ChangeIntValue(int index)
    {
        values[index] = uiElements[index].GetComponent<Slider>().value;
    }

    public byte GetEventCode()
    {
        return evCode;
    }
}
