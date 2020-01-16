using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(Image))]
public class ImageDisplayListener : MonoBehaviour
{
    private Image display;
    private VideoPlayer videoPlayer;

    private void Start()
    {
        display = GetComponent<Image>();
        videoPlayer = transform.GetChild(0).GetComponent<VideoPlayer>();
        EventManager.Instance.DisplayImageEvent += OnDisplayImage;
    }

    public void OnDisplayImage(string type, string name)
    {
        Debug.Log("setting image: " + type + " " + name);
        if (name != "None")
        {
            display.gameObject.SetActive(true);
            if (name.Contains("Video"))
            {
                Debug.Log("Playing Video" + name);
                videoPlayer.gameObject.SetActive(true);
                videoPlayer.clip = ScanRepo.GetVideo(type, name);
            }
            else
            {
                videoPlayer.gameObject.SetActive(false);
                display.sprite = ScanRepo.GetImage(type, name);
            }
        } else
        {
            display.gameObject.SetActive(false);
        }
    }
}
