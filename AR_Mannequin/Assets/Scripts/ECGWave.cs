using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// dual responsibility (maybe should be split but not necessary) update text values based on values passed to
/// UpdateECG values, which changes interal state that then is refleced on our "wave rendering system"
/// </summary>
public class ECGWave : MonoBehaviour
{
    [SerializeField] Text[] valueText;
    [SerializeField] Sprite[] hrSprites;
    [SerializeField] GameObject spriteContainer;
    [SerializeField] SpriteRenderer firstSprite;

    private Vector3 leftmostPosition;
    [SerializeField] float speed;
    private float bpm;
    private Sprite currentSprite;

    // Start is called before the first frame update
    void Awake()
    {
        currentSprite = hrSprites[1];
    }

    private void Start()
    {
        CaptureLeftMostSpritePosition();
    }

    private void Update()
    {
        AnimateWave(spriteContainer);
    }

    /// <summary>
    /// finds the left corner of our first sprite so we know where to renderer upcoming waves
    /// </summary>
    private void CaptureLeftMostSpritePosition()
    {
        //leftmost sprite position in container coordinates
        leftmostPosition = SpriteLocalCorners(firstSprite)[1];
        Debug.Log("letmost: " + leftmostPosition.x);
    }

    /// <summary>
    /// updates internal state (what sprite is will render in the next wave object)
    /// </summary>
    /// <param name="values">[]float where index 0 contains the new heart rate</param>
    public void UpdateECGValues(float[] values)
    {
        for (int i = 0; i < valueText.Length; i++)
        {
            valueText[i].text = values[i].ToString();
        }
        bpm = values[0];
        ChangeHRWave();
    }

    /// <summary>
    /// uses regex to find the right sprite based on new hear rate number
    /// </summary>
    private void ChangeHRWave()
    {
        foreach (Sprite currSprite in hrSprites)
        {
            int speed = int.Parse(Regex.Replace(currSprite.name, "[^0-9]", ""));
            if (bpm > speed)
            {
                currentSprite = currSprite;
            }
        }
    }

    /// <summary>
    /// Moves parent of waves forward, if the last instantiated sprite reaches the appropriate position, instantiate a new one
    /// </summary>
    /// <param name="wave"></param>
    private void AnimateWave(GameObject wave)
    {
        wave.transform.Translate(new Vector3(speed, 0, 0));
        if (SpriteLocalCorners(firstSprite)[0].x >= leftmostPosition.x)
        {
            GameObject newSprite = Instantiate(firstSprite.gameObject, spriteContainer.transform);
            newSprite.transform.Translate(-firstSprite.bounds.size.x * 1.2f, 0, 0); /// 1.2f is a fudge factor (difference between sprite bounds and gameobject size)
            newSprite.GetComponent<SpriteRenderer>().sprite = currentSprite;
            firstSprite = newSprite.GetComponent<SpriteRenderer>();
        }
    }

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
