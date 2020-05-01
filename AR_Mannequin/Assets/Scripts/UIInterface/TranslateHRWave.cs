using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateHRWave : MonoBehaviour
{
    private SpriteRenderer background;
    // Start is called before the first frame update
    void Start()
    {
        background = GameObject.Find("Image back").GetComponent<SpriteRenderer>();
        //this.gameObject.AddComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(background.bounds.size.x * Time.deltaTime * HRWaveSpawner.WaveTranslateSpeed, 0, 0);
    }
}
