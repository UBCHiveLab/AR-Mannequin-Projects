using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Kimberly Burke
/// 
/// Scalable Mesh animation using Mesh Animation class. 
/// TODO: More automatic way of adding Mesh Animation and where to attach script
/// </summary>
public class ApplyMeshAnimation : MonoBehaviour
{
    public string animationName;
    public string meshFolderName;

    private List<MeshAnimation> meshAnimations = new List<MeshAnimation>();
    private MeshAnimation activeAnimation;

    // Start is called before the first frame update
    void Start()
    {
        Mesh[] frames;
        try
        {
            frames = Resources.LoadAll<Mesh>(meshFolderName);
        }
        catch (Exception e) {
            Debug.Log("error setting up animation references: " + e.Message);
            return;
        }

        meshAnimations.Add(new MeshAnimation(frames, false, animationName, 3f, gameObject));

        Debug.Log("Mesh animation controller instantiated.");
        EventManager.Instance.ToggleMeshAnimationEvent += OnToggleMeshAnimationEvent;
    }

    private void OnToggleMeshAnimationEvent(string name, bool status)
    {
        Debug.Log("Mesh animatin triggered.");
        foreach(MeshAnimation animation in meshAnimations)
        {
            if (name == animation.name)
            {
                Debug.Log("Called " + name + " animation");
                activeAnimation = animation;
                if (status) {
                    activeAnimation.obj.SetActive(true);
                    StartCoroutine(AnimateMesh());
                } else {
                    // Reset mesh animation if stopping animation 
                    // activeAnimation.obj.GetComponent<MeshFilter>().mesh = animation.originalMesh;
                    StartCoroutine(FadeMesh());
                }
            }
        }
    }

    IEnumerator AnimateMesh()
    {
        Mesh[] frames = activeAnimation.meshFrames;
        MeshRenderer renderer = activeAnimation.obj.GetComponent<MeshRenderer>();
        Color color = renderer.material.color;
        color.a = 1;
        renderer.material.color = color;

        for (int i = 0; i < frames.Length; i++)
        {
            Debug.Log("Frame: " + i);
            activeAnimation.obj.GetComponent<MeshFilter>().mesh = frames[i];
            yield return new WaitForSeconds(activeAnimation.totalTime / (float)frames.Length);
        }
    }

    IEnumerator FadeMesh()
    {
        MeshRenderer renderer = activeAnimation.obj.GetComponent<MeshRenderer>();
        Color color = renderer.material.color;
        while (color.a > 0)
        {
            color.a -= 0.1f;
            renderer.material.color = color;
            yield return new WaitForSeconds(0.1f);
        }
        activeAnimation.obj.SetActive(false);
    }
}

public class MeshAnimation
{
    public Mesh[] meshFrames;
    public bool status;
    public string name;
    public float totalTime;
    public Mesh originalMesh; // TODO
    public GameObject obj;

    public MeshAnimation(Mesh[] frames, bool stat, string animation, float duration, GameObject gameObj)
    {
        meshFrames = frames;
        status = stat;
        name = animation;
        totalTime = duration;
        obj = gameObj;
    }
}
