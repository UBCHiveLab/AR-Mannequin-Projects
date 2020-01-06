using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class SkinColorListener : MonoBehaviour
{

    [SerializeField] string skinName;
    private Renderer skin;

    private void Start()
    {
        skin = GetComponent<Renderer>();
        EventManager.Instance.SkinColorEvent += OnSkinColorEvent;
    }

    public void OnSkinColorEvent(string name, string color)
    {
        Debug.Log("calling skin color with : " + name + " " + color);
        if(skinName == name)
        {
            Debug.Log("new skin: " + color);
            skin.material = SkinRepo.GetColor(color);
        }
    }
}
