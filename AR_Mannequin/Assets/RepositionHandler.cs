using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Common;

namespace Lean.Touch
{
        public class RepositionHandler : MonoBehaviour
    {
        public GameObject planeFinder;
        public GameObject mannekin;
        public GameObject groundPlane;

        public void EnablePlaneFinder()
        {   
            planeFinder.SetActive(true);
            mannekin.transform.localScale = new Vector3(1,1,1);
        }
    }
}