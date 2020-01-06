using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonsGenerator : MonoBehaviour {

    GameObject ToggleButton;
    int location = 0;

    // Use this for initialization
    void Start()
    {
        ToggleButton = this.transform.Find("ToggleButton").gameObject;
        //ToggleButton.SetActive(false);
        /*List<string> objectsToToggle = Parse.Instance.ToggleOnOff.Keys.ToList();
        foreach (string s in objectsToToggle)
        {
            makeToggle(s);
        }*/
    }

    private void makeToggle(string s)
    {
        GameObject btn = Instantiate(ToggleButton, ToggleButton.transform.parent);
        RectTransform defaultRect = btn.GetComponent<RectTransform>();
        Vector2 defaultPosition = defaultRect.anchoredPosition;
        btn.GetComponent<RectTransform>().anchoredPosition = new Vector2(defaultPosition.x, defaultPosition.y - location * defaultRect.rect.height);
        btn.SetActive(true);
        //btn.transform.position = new Vector3(btn.transform.position.x, btn.transform.position.y - location * btn.GetComponent<RectTransform>().rect.height);
        btn.GetComponentInChildren<Text>().text = s;
        btn.GetComponent<Button>().onClick.AddListener(delegate { EventManager.Instance.publishButtonTriggerEvent(s); } );
        location++;
    }
}
