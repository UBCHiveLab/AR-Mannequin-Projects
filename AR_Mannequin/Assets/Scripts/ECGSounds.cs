using System.Collections;
using UnityEngine;

public class ECGSounds : MonoBehaviour
{
    private bool dead;
    private float heartRate;
    [SerializeField] AudioClip[] ecgSounds; // [single beep, warning, dead]

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        dead = true;
    }

    /// <summary>
    /// update internal state based on []float (changes hearRate field, and conditionally starts hear rate sound or plays "dead" beep
    /// </summary>
    /// <param name="values">expects a []float where index 0 is the hear rate</param>
    public void OnHeartbeatUpdate(float[] values)
    {
        heartRate = values[0];
        if (heartRate <= 0)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(ecgSounds[2]);
            dead = true;
        }
        else if (dead && heartRate > 0)
        {
            StartCoroutine("PlayECGHeartbeat");
            dead = false;
        }
    }

    /// <summary>
    /// Coroutine for continuously playing the heart rate beep at the appropriate interval (hearRate)
    /// </summary>
    /// <returns>standard coroutine IEnumerator WaitForSeconds</returns>
    IEnumerator PlayECGHeartbeat()
    {
        while (heartRate > 0)
        {
            audioSource.clip = ecgSounds[0];
            audioSource.Play();
            yield return new WaitForSeconds(60f / heartRate);
        }
    }
}
