using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Dante Cerron
/// 
/// Uses Parse to iterate through the config files. Used for creating button UI on student client view.
/// </summary>
public class SwitchButtonsGenerator : MonoBehaviour {

    GameObject SwitchButton;
    float horizontalLocation = 0f;
    int verticalLocation = 0;

    // Use this for initialization
    void Start () {
        // SwitchButton = transform.Find("SwitchButton").gameObject;
        foreach (KeyValuePair<string, List<string>> buttonGroups in Parse.Instance.SwitchButtons)
        {
            foreach (string s in buttonGroups.Value)
            {
                makeSwitch(s, buttonGroups.Key);
                verticalLocation++;
            }
            verticalLocation = 0;
            horizontalLocation += 1.2f;
        }
    }

    private void makeSwitch(string name, string group)
    {
        GameObject btn = Instantiate(SwitchButton, SwitchButton.transform.parent);
        RectTransform defaultRect = btn.GetComponent<RectTransform>();
        Vector2 defaultPosition = defaultRect.anchoredPosition;
        btn.GetComponent<RectTransform>().anchoredPosition = new Vector2(defaultPosition.x + horizontalLocation * defaultRect.rect.width, defaultPosition.y - verticalLocation * defaultRect.rect.height);
        btn.SetActive(true);
        //btn.transform.position = new Vector3(btn.transform.position.x, btn.transform.position.y - location * btn.GetComponent<RectTransform>().rect.height);
        btn.GetComponentInChildren<Text>().text = name;
        btn.GetComponent<Button>().onClick.AddListener(delegate { EventManager.Instance.publishSwitchTriggerEvent(name, group); });
    }

    // Update is called once per frame
    void Update () {
		
	}
}
