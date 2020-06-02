using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SortStethoscopeAndOrganSounds: Singleton<SortStethoscopeAndOrganSounds> {

    Dictionary<string, Transform> Organ_transform;
    Dictionary<string, Collider> Organ_collider;
    List<string> Organ_name;

    bool visibleOrNot = false;
    string currentOrgan = null;
    string lastOrgan = null;

    float timeBetweenCheck = .1f;
    Vector3 prevPosition = Vector3.one;
    float positionThreshold = .1f; //mm?
    Vector3 prevRotation = Vector3.zero;
    float rotationThreshold = 1f; //deg

     void Awake()
    {

        EventManager.Instance.GeneralVuforiaEvent += OnGeneralVuforiaEvent;
        EventManager.Instance.TriggerEnterEvent += OnTriggerEnterEvent;
        EventManager.Instance.TriggerExitEvent += OnTriggerExitEvent;
        Debug.Log("added general vuf event subscriber");
    }

    void Start () {

        Organ_transform = new Dictionary<string, Transform>();
        Organ_collider = new Dictionary<string, Collider>();

        Organ_name = new List<string>();
        Organ_name.Add("Pulmonary");
        Organ_name.Add("Tricuspid");
        Organ_name.Add("Mitral");
        Organ_name.Add("Aortic");
 
        StartCoroutine(CheckNewDistance());
    }

    private IEnumerator CheckNewDistance()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(timeBetweenCheck);
            Transform MagicStick_ref_transform = SceneTransform.Instance.GetSceneTransform("stethoscope-tip");
            Renderer MagicStick_ref_renderer = SceneRenderer.Instance.GetSceneRenderer("stethoscope-tip");
            if (visibleOrNot && currentOrgan != null && MagicStick_ref_transform != null && MagicStick_ref_renderer != null)
            {
                Debug.Log("current organ not null and visible: " + currentOrgan);
                if (Mathf.Abs(MagicStick_ref_transform.position.x - prevPosition.x) > positionThreshold ||
                    Mathf.Abs(MagicStick_ref_transform.position.y - prevPosition.y) > positionThreshold ||
                    Mathf.Abs(MagicStick_ref_transform.position.z - prevPosition.z) > positionThreshold ||
                    Mathf.Abs(MagicStick_ref_transform.rotation.eulerAngles.x - prevRotation.x) > rotationThreshold ||
                    Mathf.Abs(MagicStick_ref_transform.rotation.eulerAngles.y - prevRotation.y) > rotationThreshold ||
                    Mathf.Abs(MagicStick_ref_transform.rotation.eulerAngles.z - prevRotation.z) > rotationThreshold)
                {
                    float intensity = CalculateCollision.Instance.calculateIntensity(MagicStick_ref_transform.position, Organ_transform[currentOrgan].position, Organ_collider[currentOrgan].bounds);
                    Debug.Log("intensity changing: " + intensity);
                    EventManager.Instance.publishInteractionAudioEvent(currentOrgan, intensity);
                    prevPosition = MagicStick_ref_transform.position;
                    prevRotation = MagicStick_ref_transform.eulerAngles;
                }
            }
            else if (lastOrgan != null)
            {
                EventManager.Instance.publishInteractionAudioEvent(lastOrgan, 0f);
                lastOrgan = null;
            }
                // todo if current organ is null and previously have organ, need to send out intensity = 0
        }
    }

    private void OnGeneralVuforiaEvent(string name, bool foundOrLost)
    {
        Debug.Log("received general vuf event: " + name + " , " + foundOrLost.ToString());
        Transform MagicStick_ref_transform = SceneTransform.Instance.GetSceneTransform("stethoscope-tip");
        Renderer MagicStick_ref_renderer = SceneRenderer.Instance.GetSceneRenderer("stethoscope-tip");
        visibleOrNot = foundOrLost;
        if (foundOrLost)
        {
            if (MagicStick_ref_transform != null)
            {
                EventManager.Instance.publishInteractionOpacityEvent(MagicStick_ref_transform.parent.name, 1f); //opacity hardcoded
            }
        } else
        {
            if (MagicStick_ref_transform != null)
            {
                EventManager.Instance.publishInteractionOpacityEvent(MagicStick_ref_transform.parent.name, 0f); //opacity hardcoded
            }
        }
    }

    private void OnTriggerExitEvent(string collider, string collidee)
    {
        Debug.Log("Sort trigger exit");
        Transform MagicStick_ref_transform = SceneTransform.Instance.GetSceneTransform("stethoscope-tip");
        Renderer MagicStick_ref_renderer = SceneRenderer.Instance.GetSceneRenderer("stethoscope-tip");
        if (MagicStick_ref_transform != null && (MagicStick_ref_transform.name.CompareTo(collider) == 0) &&
            collidee == currentOrgan)
        {
            Debug.Log("unsetting organ");
            lastOrgan = currentOrgan;
            currentOrgan = null;
        }
    }

    private void OnTriggerEnterEvent(string collider, string collidee)
    {
        Transform MagicStick_ref_transform = SceneTransform.Instance.GetSceneTransform("stethoscope-tip");
        Renderer MagicStick_ref_renderer = SceneRenderer.Instance.GetSceneRenderer("stethoscope-tip");
        Debug.Log("Sort trigger enter: " + collider + " " + collidee + " " + MagicStick_ref_transform.name + " " + MagicStick_ref_renderer.name);
        if (MagicStick_ref_transform != null && (MagicStick_ref_transform.name.CompareTo(collider) == 0) &&
            Organ_name.Contains(collidee))
        {
            Debug.Log("setting organ");
            currentOrgan = collidee;
            if(!Organ_transform.ContainsKey(collidee))
            {
                Organ_transform.Add(collidee, SceneTransform.Instance.GetSceneTransform(collidee));
            }
            if(!Organ_collider.ContainsKey(collidee))
            {
                Organ_collider.Add(collidee, SceneCollider.Instance.GetSceneCollider(collidee));
            }
        }
    }
}
