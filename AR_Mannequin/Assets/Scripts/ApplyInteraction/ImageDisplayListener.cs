using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageDisplayListener : MonoBehaviour
{
    private Image display;

    private void Start()
    {
        display = GetComponent<Image>();
        EventManager.Instance.DisplayImageEvent += OnDisplayImage;
    }

    public void OnDisplayImage(string type, string name)
    {
        Debug.Log("setting image: " + type + " " + name);
        if (name != "None")
        {
            display.gameObject.SetActive(true);
            display.sprite = ScanRepo.GetImage(type, name);
        } else
        {
            display.gameObject.SetActive(false);
        }
    }
}
