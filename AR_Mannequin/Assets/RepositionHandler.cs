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
            Debug.Log("Scene Reset");
            planeFinder.SetActive(true);
            mannekin.transform.localScale = Vector3.zero;
        }

        void OnEnable()
        {
            LeanTouch.OnFingerTap += HandleFingerTap;
        }

        void OnDisable()
        {
            LeanTouch.OnFingerTap -= HandleFingerTap;
        }

        void HandleFingerTap(LeanFinger finger)
        {
            if (finger.Tap)
            {
            Debug.Log("You just tapped the screen");
            //Debug.Log("There are currently " + finger.Index + " at " + finger.ScreenPosition + " and " + finger.IsOverGui);
            //LeanTouch.SimulateTap(new Vector2(0, 0), 1.0f, 3);
            }
        }

    }
}