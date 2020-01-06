using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWave : MonoBehaviour
{

    public GameObject firstWave;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetInstanceID() != firstWave.GetInstanceID())
        {
            Destroy(other.gameObject);
        }
    }
}
