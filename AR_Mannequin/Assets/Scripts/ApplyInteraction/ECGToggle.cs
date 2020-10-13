using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ECGToggle : MonoBehaviour
{

    [SerializeField] private GameObject ecgMonitor;

    private void ActiveBehaviour(GameObject ecgMonitor)
    {
        ecgMonitor.SetActive(true);
    }

    // default behaviour
    private void InactiveBehaviour(GameObject ecgMonitor)
    {
        ecgMonitor.SetActive(false);
    }
}
