using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

/// <summary>
/// Created by Dante Cerron. Modified by Kimberly Burke.
/// </summary>

public class EventManager: Singleton<EventManager>{

    public delegate void VuforiaModelDelegate(string foundOrLost, string modelType, Transform parentTransform);
    public event VuforiaModelDelegate MainManikinVuforiaEvent;

    public delegate void RecognitionStateChangedDelegate(string foundOrLost);
    public event RecognitionStateChangedDelegate RecognitionStateChangedEvent;

    public delegate void GeneralVuforiaDelegate(string name, bool foundOrLost);
    public event GeneralVuforiaDelegate GeneralVuforiaEvent;

    public delegate void SetSceneTransformDelegate(string name, Transform t);
    public event SetSceneTransformDelegate SetSceneTransformEvent;

    public delegate void SetSceneRendererDelegate(string name, Renderer r);
    public event SetSceneRendererDelegate SetSceneRendererEvent;

    public delegate void SetSceneColliderDelegate(string name, Collider c);
    public event SetSceneColliderDelegate SetSceneColliderEvent;

    public delegate void SceneTransformChangedDelegate(string name);
    public event SceneTransformChangedDelegate SceneTransformChangedEvent;

    public delegate void SceneRendererChangedDelegate(string name);
    public event SceneRendererChangedDelegate SceneRendererChangedEvent;

    public delegate void InteractionAudioDelegate(string name, float intensity);
    public event InteractionAudioDelegate InteractionAudioEvent;

    public delegate void InteractionOpacityDelegate(string name, float opacity);
    public event InteractionOpacityDelegate InteractionOpacityEvent;

    public delegate void InteractionAnimationDelegate(string name, float frame);
    public event InteractionAnimationDelegate InteractionAnimationEvent;

    // Currently used for eye dilation animation
    public delegate void ToggleAnimationDelegate(string name, bool status);
    public event ToggleAnimationDelegate ToggleAnimationEvent;

    // Currenrtly used for vomit animation - scalable
    public delegate void ToggleMeshAnimationDelegate(string name, bool status);
    public event ToggleMeshAnimationDelegate ToggleMeshAnimationEvent;

    public delegate void TriggerEnterDelegate(string collider, string collidee);
    public event TriggerEnterDelegate TriggerEnterEvent;

    public delegate void TriggerExitDelegate(string collider, string collidee);
    public event TriggerExitDelegate TriggerExitEvent;

    public delegate void IMUTransformUpdateDelegate(string name, Quaternion q);
    public event IMUTransformUpdateDelegate IMUTransformUpdateEvent;

    public delegate void ButtonToggleDelegate(string s);
    public event ButtonToggleDelegate ButtonToggleEvent;

    public delegate void ButtonSwitchDelegate(string s, string g);
    public event ButtonSwitchDelegate ButtonSwitchEvent;

    // Currently used for updating ECG image text
    public delegate void ECGUpdateDelegate(float[] values);
    public event ECGUpdateDelegate ECGUpdateEvent;

    // Currently used for toggling white noise, heart beat and lung audio
    public delegate void AudioToggleDelegate(string sound, bool status);
    public event AudioToggleDelegate AudioToggleEvent;

    // Currently used for heartbeat and lung audio - DEPRECATED
    public delegate void AudioSlideDelegate(string sound, float volume);
    public event AudioSlideDelegate AudioSlideEvent;

    // Currently used for transferring master client ownership when teacher is present
    public delegate void TeacherPresentDelegate();
    public event TeacherPresentDelegate TeacherPresentEvent;

    public delegate void ManikinPositionedDelegate(bool positioned);
    public event ManikinPositionedDelegate ManikinPositionedEvent;

    public delegate void DisplayImageDelegate(string type, string name);
    public event DisplayImageDelegate DisplayImageEvent;

    public delegate void AudioSourceDelegate(string type, string name);
    public event AudioSourceDelegate AudioSourceEvent;

    public delegate void SkinColorDelegate(string name, string color);
    public event SkinColorDelegate SkinColorEvent;

    public void publishVuforiaModelEvent(string foundOrLost, string modelType, Transform parentTransform)
    {
        Debug.Log("publish vuforia model event");
        if (MainManikinVuforiaEvent != null)
        {
            Debug.Log("model: " + parentTransform.GetHashCode());
            MainManikinVuforiaEvent(foundOrLost, modelType, parentTransform);
        }
    }

    public void publishRecognitionStateChangedEvent(string foundOrLost)
    {
        try
        {
            if (RecognitionStateChangedEvent != null)
            {
                RecognitionStateChangedEvent(foundOrLost);
            }
        }
        catch (System.NullReferenceException e)
        {
            Debug.Log("Nothing listens to Vuforia recognition state changed. \nIgnore if this is thrown during scene loading (some items gets initialized after Vuforia targets). \nDebug otherwise. \n");
        }
    }

    public void publishGeneralVuforiaEvent(string name, bool foundOrNot)
    {
        //Debug.Log("yes1");
        if (GeneralVuforiaEvent != null)
        {
            //Debug.Log("ahhhhh");
            GeneralVuforiaEvent(name, foundOrNot);
        }
    }

    public void publishSetSceneTransform(string name, Transform t)
    {
        if(SetSceneTransformEvent != null)
        {
            SetSceneTransformEvent(name, t);
        }
    }

    public void publishSetSceneRenderer(string name, Renderer r)
    {
        if(SetSceneRendererEvent != null)
        {
            SetSceneRendererEvent(name, r);
        }
    }

    public void publishSetSceneCollider(string name, Collider c)
    {
        if(SetSceneColliderEvent != null)
        {
            SetSceneColliderEvent(name, c);
        }
    }

    public void publishSceneTransformChanged(string name)
    {
        if(SceneTransformChangedEvent != null)
        {
            SceneTransformChangedEvent(name);
        }
    }

    public void publishSceneRendererChanged(string name)
    {
        if(SceneRendererChangedEvent != null)
        {
            SceneRendererChangedEvent(name);
        }
    }

    public void publishIMUTransformUpdate(string name, Quaternion q)
    {
        IMUTransformUpdateEvent(name, q);
    }

    public void publishInteractionAudioEvent(string name, float intensity)
    {
        if(InteractionAudioEvent != null)
        {
            InteractionAudioEvent(name, intensity);
        }
    }


    public void publishInteractionOpacityEvent(string name, float opacity)
    {
        if(InteractionOpacityEvent != null)
        {
            InteractionOpacityEvent(name, opacity);
        }
    }

    public void publishInteractionAnimationEvent(string name, float frame)
    {
        if(InteractionAnimationEvent != null)
        {
            InteractionAnimationEvent(name, frame);
        }
    }

    public void publishTriggerEnterEvent(string collider, string collidee)
    {
        if(TriggerEnterEvent != null)
        {
            TriggerEnterEvent(collider, collidee);
        }
    }

    public void publishTriggerExitEvent(string collider, string collidee)
    {
        if(TriggerExitEvent != null)
        {
            TriggerExitEvent(collider, collidee);
        }
    }

    public void publishButtonTriggerEvent(string name)
    {
        if(ButtonToggleEvent != null)
        {
            ButtonToggleEvent(name);
        }
    }

    public void publishSwitchTriggerEvent(string name, string group)
    {
        if(ButtonSwitchEvent != null)
        {
            Debug.Log("Switching...");
            ButtonSwitchEvent(name, group);
        }
    }

    // currently used for toggling eye dilation animation
    public void publishToggleAnimationEvent(string name, bool status)
    {   
        if (ToggleAnimationEvent != null)
        {
            Debug.Log("Switching sprite animation...");
            ToggleAnimationEvent(name, status);
        }
    }

    // currently used for toggling vomit and blood animation
    public void publishToggleMeshAnimationEvent(string name, bool status)
    {
        Debug.Log("Switching mesh animation...");
        if (ToggleMeshAnimationEvent != null)
        {
            ToggleMeshAnimationEvent(name, status);
        }
    }

    public void publishECGUpdateEvent(float[] values)
    {
        Debug.Log("Updating ECG...");
        if (ECGUpdateEvent != null) { ECGUpdateEvent(values); }
    }

    // currently used for white noise audio toggling
    public void publishAudioToggleEvent(string name, bool status)
    {
        Debug.Log("Switching audio...");
        if (AudioToggleEvent != null) { AudioToggleEvent(name, status); }
    }

    // currently used for volume levels - DEPRECATED
    public void publishAudioSlideEvent(string name, float volume)
    {
        Debug.Log("Adjusting audio level...");
        if (AudioSlideEvent != null) { AudioSlideEvent(name, volume); }
    }

    public void publishTeacherPresentEvent()
    {
        Debug.Log("Making teacher master client of room.");
        if (TeacherPresentEvent != null) { TeacherPresentEvent(); }
    }

    public void publishManikinPositionedEvent(bool positioned)
    {
        Debug.Log("Manikin positioned at calibrated coordinate.");
        if (ManikinPositionedEvent != null) { ManikinPositionedEvent(positioned); }
    }

    public void publishDisplayImageEvent(string type, string name)
    {
        if(DisplayImageEvent != null)
        {
            DisplayImageEvent(type, name);
        }
    }

    public void publishAudioSourceEvent(string type, string name)
    {
        if(AudioSourceEvent != null) {
            AudioSourceEvent(type, name);
        }
    }

    public void publishSkinColorEvent(string name, string color)
    {
        if(SkinColorEvent != null)
        {
            Debug.Log("calling skin color event");
            SkinColorEvent(name, color);
        }
    }
}