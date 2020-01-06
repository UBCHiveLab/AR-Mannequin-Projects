using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// Created by Dante Cerron 2019, modified by Kimberly Burke
/// Scalable sprite animation for toggle events.
/// Controls sprite animations. Currently used for dilation and constrction of left and right eye
/// </summary>
public class SpriteAnimation : MonoBehaviour
{

    public GameObject target;
    public string sourceFolder;
    public List<string> names;

    private Image targetImage;
    private List<Sprite> sprites;
    private float totalTime = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        if (target != null && sourceFolder != null)
        {
            targetImage = target.GetComponent<Image>();
            sprites = Resources.LoadAll<Sprite>(sourceFolder).ToList();

            EventManager.Instance.ToggleAnimationEvent += OnToggleAnimationEvent;
        }
    }

    public void OnToggleAnimationEvent(string name, bool status)
    {
        StopCoroutine(AnimateForwards());
        StopCoroutine(AnimateBackwards());
        foreach (string cur in names)
        {
            if (cur.Equals(name))
            {
                if(status)
                {
                    StartCoroutine(AnimateForwards());
                }
                else
                {
                    StartCoroutine(AnimateBackwards());
                }
                break;
            }
        }
    }

    //toggle defines whether to play animation forward or backwards
    //might want to provide finer animation controls in the future
    private IEnumerator AnimateForwards()
    {
        for (int i = 0; i < sprites.Count; i++)
        {
            targetImage.sprite = sprites[i];

            yield return new WaitForSeconds(totalTime / (float)sprites.Count);
        }
    }

    private IEnumerator AnimateBackwards()
    {
        for (int i = sprites.Count - 1; i > 0; i--)
        {
            targetImage.sprite = sprites[i];

            yield return new WaitForSeconds(totalTime / (float)sprites.Count);
        }
    }
}
