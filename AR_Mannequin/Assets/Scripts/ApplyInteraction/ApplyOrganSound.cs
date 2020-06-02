using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplyOrganSound : Singleton<ApplyOrganSound>
{
    AudioSource source;
    Dictionary<string, AudioClip> Organ_name;
    float currentIntensity;
    float transitionTime = .1f; //seconds

    //test volume
    GameObject volTest;
    TextMesh volMesh;

    // Use this for initialization
    void Awake()
    {
        EventManager.Instance.InteractionAudioEvent += OnInteractionAudioEvent;
    }

    private void Start()
    {
        source = Make.Instance.MakeAudioSource(GameObject.Find("MixedRealityCameraParent"));
        source.loop = true;
        source.clip = null;
        source.volume = 0f;
        source.Play();

        Organ_name = new Dictionary<string, AudioClip>();
        List<string> Organ_name_ = new List<string>();
        Organ_name_.Add("Pulmonary");
        Organ_name_.Add("Tricuspid");
        Organ_name_.Add("Mitral");
        Organ_name_.Add("Aortic");

        foreach (string s in Organ_name_)
        {
            if (Resources.Load<AudioClip>(s) == null)
                Debug.Log("oh no no audio clip of " + s);
            Organ_name.Add(s, Resources.Load<AudioClip>(s)); //add more organs
        }


        //test volume
        volTest = GameObject.Find("testVol");
        if (volTest != null) {
            volMesh = volTest.GetComponent<TextMesh>();
            volMesh.text = source.volume.ToString();
        }
    }

    private void OnInteractionAudioEvent(string name, float intensity)
    {
        Debug.Log("interaction audio, incoming: " + intensity + ", current: " + source.volume);
        if((source.clip == null || source.clip.name != name) && Organ_name.ContainsKey(name))
        {
            source.clip = Organ_name[name];
            source.Play();
        }
        if (intensity < source.volume)
        {
            StartCoroutine(FadeOut(intensity));
        } else
        {
            StartCoroutine(FadeIn(intensity));
        }
    }


    //might have problems with updates too fast
    public static IEnumerator FadeOut(float newVolume)
    {
        AudioSource audioSource = Instance.source;
        float startVolume = audioSource.volume;
        float interim = startVolume - newVolume;

        while (audioSource.volume > newVolume)
        {
            Debug.Log("update volume fade out: " + audioSource.volume + " -> " + newVolume);
            audioSource.volume -= interim * Time.deltaTime / Instance.transitionTime;
            // todo: check if more than 1, less than 0
            //test volume
            /////
            //Instance.volMesh.text = audioSource.volume.ToString();
        }
        Debug.Log("after change: " + audioSource.volume);
        yield return null;
        //audioSource.volume = startVolume;
    }

    public static IEnumerator FadeIn(float newVolume)
    {
        AudioSource audioSource = Instance.source;
        float startVolume = audioSource.volume;
        float interim = newVolume - startVolume;

        while (audioSource.volume < newVolume)
        {
            Debug.Log("update volume fade in: " + audioSource.volume + " -> " + newVolume);
            audioSource.volume += interim * Time.deltaTime / Instance.transitionTime;
            //Instance.volMesh.text = audioSource.volume.ToString();
        }
        Debug.Log("after change: " + audioSource.volume);
        yield return null;
        //audioSource.volume = startVolume;
    }
}
