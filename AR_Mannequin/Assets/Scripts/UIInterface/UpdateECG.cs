using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class UpdateECG : MonoBehaviour
{
    [SerializeField] Text[] valueText;
    [SerializeField] Sprite[] hrSprites;
    [SerializeField] GameObject spriteContainer;
    [SerializeField] SpriteRenderer firstSprite;

    private Vector3 leftmostPosition;
    //private float endBoundary = 4f;
    //[SerializeField] float speed;

    private float bpm;
    public static Sprite currentSprite;
    public static bool isARPlaced;

    // Start is called before the first frame update
    void Awake()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.ECGUpdateEvent += UpdateECGValues;
            EventManager.Instance.ManikinPositionedEvent += HandleManikinPositioned;
        }
        //initial bpm is 85, so the sprite is displaying hrwave 80
        currentSprite = hrSprites[4];

    }

    private void HandleManikinPositioned(bool positioned)
    {
        //if(positioned)
        //{
        //    CaptureLeftMostSpritePosition();
        //    StartCoroutine(AnimateWave(spriteContainer));
        //} else
        //{
        //    StopCoroutine(AnimateWave(spriteContainer));
        //}
        isARPlaced = positioned;
    }

    private void CaptureLeftMostSpritePosition()
    {
        //leftmost sprite position in container coordinates
        leftmostPosition = SpriteLocalCorners(firstSprite)[0];
        Debug.Log("letmost: " + leftmostPosition.x);
    }

    private void UpdateECGValues(float[] values)
    {
        for (int i = 0; i < valueText.Length; i++)
        {
            valueText[i].text = values[i].ToString();
        }
        bpm = values[0];
        ChangeHRWave();
    }

    private void ChangeHRWave()
    {
        currentSprite = hrSprites[0]; 
        foreach (Sprite currSprite in hrSprites)
        {
            //speed equals the digit in currentSprite name
            int speed = int.Parse(Regex.Replace(currSprite.name, "[^0-9]", ""));
            if (bpm > speed)
            {
                currentSprite = currSprite;

            }
        }
    }
    /// <summary>
    /// Old wave animation function, not using anymore
    /// </summary>
    /// <returns>The wave.</returns>
    /// <param name="wave">Wave.</param>
    //public IEnumerator AnimateWave(GameObject wave)
    //{
    //    while(true)
    //    {
    //        wave.transform.Translate(new Vector3(speed, 0, 0));
    //        //wave.transform.Translate(wave.)
    //        if (SpriteLocalCorners(firstSprite)[0].x >= leftmostPosition.x)
    //        {
    //            GameObject newSprite = Instantiate(firstSprite.gameObject, spriteContainer.transform);
    //            newSprite.transform.Translate(-firstSprite.bounds.size.x * WaveTranslateSpeed, 0, 0); /// maybe that 1.2f should be a parameter, but im not sure how to generalize
    //            newSprite.GetComponent<SpriteRenderer>().sprite = currentSprite;
    //            firstSprite = newSprite.GetComponent<SpriteRenderer>();
    //        }
    //        yield return null;
    //    }
    //}


    /// <summary>
    /// found at: https://answers.unity.com/questions/641006/is-there-a-way-to-get-a-sprites-current-widthheigh.html
    /// </summary>
    /// <param name="sp">any Sprite, in this case, the sprite used for HR wave pattern</param>
    /// <returns>array of vector3 where [0] is top left corner of sprite in world coords, [1] is bottom right corner</returns>
    Vector3[] SpriteLocalCorners(SpriteRenderer sp)
    {
        Vector3 pos = sp.bounds.center;
        //Debug.Log("sprite center: " + pos.ToString("F4") + " sprite extents: " + sp.transform.TransformVector(sp.sprite.bounds.extents).ToString("F4"));
        Vector3[] array = new Vector3[2];
        //top left
        array[0] = pos - sp.transform.TransformVector((sp.sprite.bounds.extents));
        // Bottom right
        array[1] = pos + sp.transform.TransformVector((sp.sprite.bounds.extents));
        return array;
    }
}
