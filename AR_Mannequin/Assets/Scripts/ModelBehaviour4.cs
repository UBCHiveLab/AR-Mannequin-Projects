using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using HoloToolkit.Unity;

/// <summary>
/// Created by Dante Cerron, 2019. Modified by Kimberly Burke
/// 
/// Positions and rotates the body-assembly.
/// </summary>
public class ModelBehaviour4 : MonoBehaviour
{

    //Most updated ModelBehaviour. Attach to surface level GO that has all relevant GOs as children.
    //works with customtrackabledeventhandler

    Transform modelObjectTransform;

    TRACKINGSTATE currentTrackingState;
    enum TRACKINGSTATE { NO, YES }
    private Dictionary<Transform, Vector3> imageTargetsPos;
    private Dictionary<Transform, Quaternion> imageTargetsRot;

    private ConnectionUIManager uiManager;

    public bool calibrate;

    Vector3 nullVector;

    private IMUControl imu;

    StringBuilder update;

    // Use this for initialization

    void Start()
    {
        modelObjectTransform = this.GetComponent<Transform>();
        
        currentTrackingState = TRACKINGSTATE.NO;
        
        imu = GameObject.Find("IMUControl").GetComponent<IMUControl>();
        
        imageTargetsPos = new Dictionary<Transform, Vector3>();
        imageTargetsRot = new Dictionary<Transform, Quaternion>();
        
        nullVector = new Vector3(9999f, 9999f, 9999f); // initializes the the transform completely out of view 

        uiManager = GetComponentInChildren<ConnectionUIManager>();

        Debug.Log("showing up 6");

        if (GameObject.Find("ImageTargets").transform == null)
        {
            Debug.Log("wtf");
        }

        Debug.Log("img targets total: " + imageTargetsPos.Count);
        Debug.Log("added image targets");

        calibrate = false;
        GameObject.Find("WorldAnchorManager").GetComponent<WorldAnchorManager>().AttachAnchor(modelObjectTransform.gameObject);
        SetCalibrateStatus();
        //EventManager.Instance.MainManikinVuforiaEvent += OnVuforiaModelEvent;
    }

    /// <summary>
    /// Sets image targets to null vector
    /// </summary>
    public void ClearImageTargDictionary()
    {
        foreach (Transform child in GameObject.Find("ImageTargets").transform)
        {
            var inner = child.GetChild(0);
            Debug.Log("showing up 7 " + child.GetChild(0).GetHashCode());
            if (imageTargetsPos.ContainsKey(inner)) {
                imageTargetsPos[inner] = nullVector;
                imageTargetsRot[inner] = Quaternion.identity;
            } else {
                imageTargetsPos.Add(inner, nullVector);
                imageTargetsRot.Add(inner, Quaternion.identity);
            }
            Debug.Log("image target: " + child.GetChild(0).GetHashCode() + " " + child.GetChild(0).name);
        }
    }

    /// <summary>
    /// Uses toggle button to subscribe and unsubscribe to Vuforia event for calibration
    /// </summary>
    public void SetCalibrateStatus()
    {
        calibrate = !calibrate;
        if (uiManager.calibrateToggle.isOn != calibrate)
        {
            uiManager.AdjustCalibrateStatus(calibrate);
        }
        if (calibrate)
        {
            unparent();
            ClearImageTargDictionary();
            EventManager.Instance.MainManikinVuforiaEvent += OnVuforiaModelEvent;
        } else
        {
            EventManager.Instance.MainManikinVuforiaEvent -= OnVuforiaModelEvent;
        }
    }

    /// <summary>
    /// When IMUs found, calibrates the body-assembly parent transform 
    /// </summary>
    /// <param name="foundOrLost"></param>
    /// <param name="modelType"></param>
    /// <param name="updatedParentTransform"></param>
    public void OnVuforiaModelEvent(string foundOrLost, string modelType, Transform updatedParentTransform)
    {
        //Debug.Log("found?: " + foundOrLost + " , type: " + modelType);

        TRACKINGSTATE updatedTrackingState = (TRACKINGSTATE)Enum.Parse(typeof(TRACKINGSTATE), foundOrLost);

        if (updatedTrackingState == TRACKINGSTATE.YES)
        {
            imageTargetsPos[updatedParentTransform] = updatedParentTransform.position;
            imageTargetsRot[updatedParentTransform] = updatedParentTransform.rotation;
            Debug.Log("img targets total: " + imageTargetsPos.Count);
            bool allImagesTracked = true;
            foreach (KeyValuePair<Transform, Vector3> cur in imageTargetsPos)
            {
                Debug.Log("img target: " + cur.Key.GetHashCode() + ", tracking status: " + cur.Value.ToString());
                if (cur.Value == nullVector)
                {
                    allImagesTracked = false;
                }
            }
            Debug.Log("all image tracked?: " + allImagesTracked.ToString());
            if (allImagesTracked)
            {
                parent(updatedParentTransform);
            }
        }
    }

    //NOTE: this function uses slerp to average rotations and positions, but if more than 2 
    //image targets are used then a more involved average function is needed.
    private void parent(Transform updatedParentTransform)
    {
        SetCalibrateStatus();
        Vector3 pos = Vector3.zero;
        foreach (KeyValuePair<Transform, Vector3> cur in imageTargetsPos)
        {
            Debug.Log("target position: " + cur.Value.ToString("F3"));
            pos += cur.Value;
        }
        pos = pos / imageTargetsPos.Count;

        Quaternion[] rots = new Quaternion[imageTargetsRot.Count];
        imageTargetsRot.Values.CopyTo(rots, 0);
        Quaternion rot = averageQuaternion(rots);

        modelObjectTransform.SetPositionAndRotation(pos, rot);
        //modelObjectTransform.SetParent(updatedParentTransform, true);
        if (imu != null)
        {
            Debug.Log("parented, calibrating imu");
            imu.IMUCalibrated();
        }
        Debug.Log("model position: " + this.gameObject.transform.position.ToString("F3"));
        GameObject.Find("WorldAnchorManager").GetComponent<WorldAnchorManager>().AttachAnchor(modelObjectTransform.gameObject);
        EventManager.Instance.publishManikinPositionedEvent(true);
    }

    private void unparent()
    {
        modelObjectTransform.position = new Vector3(0f, 0f, -1f);
        GameObject.Find("WorldAnchorManager").GetComponent<WorldAnchorManager>().RemoveAnchor(modelObjectTransform.gameObject);
        EventManager.Instance.publishManikinPositionedEvent(false);
        if (imu != null)
        {
            imu.ResetIMU();
        }
    }

    /*
    //credit for average code: https://forum.unity.com/threads/average-quaternions.86898/
    private Quaternion averageQuaternion(Quaternion[] quats)
    {
        int addAmount = 0;
        Quaternion addedRotation = Quaternion.identity;
        Quaternion averageRotation = Quaternion.identity;
        foreach (Quaternion singleRotation in quats)
        {
            float w, x, y, z;
            addAmount++;
            float addDet = 1.0f / (float)addAmount;
            addedRotation.w += singleRotation.w;
            w = addedRotation.w * addDet;
            addedRotation.x += singleRotation.x;
            x = addedRotation.x * addDet;
            addedRotation.y += singleRotation.y;
            y = addedRotation.y * addDet;
            addedRotation.z += singleRotation.z;
            z = addedRotation.z * addDet;
            
            float D = 1.0f / (w * w + x * x + y * y + z * z);
            w *= D;
            x *= D;
            y *= D;
            z *= D;

            averageRotation = new Quaternion(x, y, z, w);
        }
        return averageRotation;
    }
    */

    //credit: http://wiki.unity3d.com/index.php/Averaging_Quaternions_and_Vectors
    public Quaternion averageQuaternion(Quaternion[] rotations)
    {
        Vector4 cumulative = Vector4.zero;
        Quaternion firstRotation = rotations[0];
        int addAmount = rotations.Length;

        float w = 0.0f;
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;


        foreach (Quaternion newRotation in rotations)
        {
            Quaternion newRotationChecked = newRotation;
            //Before we add the new rotation to the average (mean), we have to check whether the quaternion has to be inverted. Because
            //q and -q are the same rotation, but cannot be averaged, we have to make sure they are all the same.
            if (!AreQuaternionsClose(newRotation, firstRotation))
            {
                newRotationChecked = InverseSignQuaternion(newRotation);
            }

            //Average the values
            float addDet = 1f / (float)addAmount;
            cumulative.w += newRotationChecked.w;
            w = cumulative.w * addDet;
            cumulative.x += newRotationChecked.x;
            x = cumulative.x * addDet;
            cumulative.y += newRotationChecked.y;
            y = cumulative.y * addDet;
            cumulative.z += newRotationChecked.z;
            z = cumulative.z * addDet;

        }
        //note: if speed is an issue, you can skip the normalization step
        return NormalizeQuaternion(x, y, z, w);
    }

    public static Quaternion NormalizeQuaternion(float x, float y, float z, float w)
    {

        float lengthD = 1.0f / (w * w + x * x + y * y + z * z);
        w *= lengthD;
        x *= lengthD;
        y *= lengthD;
        z *= lengthD;

        return new Quaternion(x, y, z, w);
    }

    //Changes the sign of the quaternion components. This is not the same as the inverse.
    public static Quaternion InverseSignQuaternion(Quaternion q)
    {

        return new Quaternion(-q.x, -q.y, -q.z, -q.w);
    }

    //Returns true if the two input quaternions are close to each other. This can
    //be used to check whether or not one of two quaternions which are supposed to
    //be very similar but has its component signs reversed (q has the same rotation as
    //-q)
    public static bool AreQuaternionsClose(Quaternion q1, Quaternion q2)
    {

        float dot = Quaternion.Dot(q1, q2);

        if (dot < 0.0f)
        {

            return false;
        }

        else
        {

            return true;
        }
    }
}
