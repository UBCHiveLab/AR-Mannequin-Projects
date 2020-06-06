using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// HR wave spawner. Created by Silver Xu 2020.05.01 to replace the old hr wave script
/// spawner will spawn a new wave everytime current wave moves outside the spanwer
/// </summary>
public class HRWaveSpawner : MonoBehaviour
{
    [SerializeField]private float WaveSpeed = 0.2f;
    public static float WaveTranslateSpeed;
    [SerializeField]private GameObject HRWavePrefab;
    private BoxCollider box;
    private GameObject currentWave;
    private SpriteRenderer currentWaveSprite;
    private SpriteRenderer sprite;
    private Vector3 rightMostPosition;
    private bool isInit = true;
    

    // Start is called before the first frame update
    void Start()
    {
        //get current sprite component
        sprite = this.GetComponent<SpriteRenderer>();

        box = this.GetComponent<BoxCollider>();
        WaveTranslateSpeed = WaveSpeed;

        isInit = true;
    
    }


    // Update is called once per frame
    void Update()
    {


            //initialize the first wave
            if (isInit)
            {
                currentWave = Instantiate(HRWavePrefab, this.transform.position, new Quaternion(0f, 0f, 0f, 0f), this.transform.parent);
                currentWaveSprite = currentWave.GetComponent<SpriteRenderer>();
                currentWaveSprite.sprite = UpdateECG.currentSprite;
                //update the wave sprite
                sprite.sprite = UpdateECG.currentSprite;
                //every hr wave sprite has different width, adjust the box collider size to the corresponding width
                if (box != null)
                box.size = new Vector3(sprite.sprite.bounds.size.x, box.size.y, box.size.z);
                currentWave.GetComponent<BoxCollider>().size = box.size;
                isInit = false;
            }
    }

    private void OnEnable()
    {
        //set the spawner to initial a new wave everytime it's been initialed
        isInit = true;
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject == currentWave)
        {

            //instatiate a new wave
            currentWave = Instantiate(HRWavePrefab,this.transform.position, new Quaternion(0f, 0f, 0f, 0f), this.transform.parent);
            currentWaveSprite = currentWave.GetComponent<SpriteRenderer>();
            currentWaveSprite.sprite = UpdateECG.currentSprite;

            sprite.sprite = UpdateECG.currentSprite;
            box.size = new Vector3(sprite.sprite.bounds.size.x, box.size.y, box.size.z);
            currentWave.GetComponent<BoxCollider>().size = box.size;

        }

    }
   
   
}
