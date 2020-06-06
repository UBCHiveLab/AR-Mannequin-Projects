using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepositionHandler : MonoBehaviour
{
    public GameObject planeFinder;
    public GameObject mannekinPlane;

    public void EnablePlaneFinder()
    {   
        planeFinder.SetActive(true);
        mannekinPlane.transform.position = new Vector3(0,0,0);
    }
}
