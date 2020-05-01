using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HRWave : MonoBehaviour
{
    public static float WaveTranslateSpeed=0.2f;
    public GameObject HRWavePrefab;
    //public bool isARPlaced=false;
    private GameObject currentWave;
    private SpriteRenderer currentWaveSprite;
    private SpriteRenderer sprite;
    private Vector3 rightMostPosition;
    private bool isInit = true;
    

    // Start is called before the first frame update
    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();

        CaptureRightMostSpritePosition();

    }
    private void CaptureRightMostSpritePosition()
    {
        //leftmost sprite position in container coordinates
        rightMostPosition = SpriteLocalCorners(sprite)[1];
        Debug.Log("letmost: " + rightMostPosition.x);
    }

    // Update is called once per frame
    void Update()
    {
        if (UpdateECG.isARPlaced)
        {
            if (isInit)
            {
                currentWave = Instantiate(HRWavePrefab, this.transform.position, new Quaternion(0f,0f,0f,0f), this.transform.parent);
                currentWaveSprite = currentWave.GetComponent<SpriteRenderer>();
                currentWaveSprite.sprite = UpdateECG.currentSprite;
                isInit = false;
            }



                

        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject == currentWave)
        {
            //Destroy(currentWave);
            //instatiate a new wave
            currentWave = Instantiate(HRWavePrefab,this.transform.position, new Quaternion(0f, 0f, 0f, 0f), this.transform.parent);
            currentWaveSprite = currentWave.GetComponent<SpriteRenderer>();
            currentWaveSprite.sprite = UpdateECG.currentSprite;

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
