using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Log : MonoBehaviour
{
    public string logText { set; private get; }
    public TextMeshProUGUI tmp;

    public void SetLogText(string text)
    {
        logText = text;
        tmp.text = text;
    }

}
