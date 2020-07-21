using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepositionHandler : MonoBehaviour
{
    public GameObject planeFinder;

    public void EnablePlaneFinder()
    {   
            planeFinder.SetActive(true);
    }
}
