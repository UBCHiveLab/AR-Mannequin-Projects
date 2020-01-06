using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingMonitorNumbers : MonoBehaviour {

    private Text monitor;
    private System.Random displayNumber;
    private bool enter;

	// Use this for initialization
	void Start () {
        monitor = this.gameObject.GetComponent<Text>();
        displayNumber = new System.Random();
        monitor.text = "Heart Rate: " + displayNumber.Next(95, 105).ToString() + 
            "\nBlood Pressure: " + displayNumber.Next(95, 105).ToString() + 
            "\nGlucose: " + displayNumber.Next(95, 105).ToString() + 
            "\nRespiratory: " + displayNumber.Next(95, 105).ToString();
	}
	
	// Update is called once per frame
	void Update () {
        if (enter == false)
        {
            StartCoroutine(your_timer());

            monitor.text = "Heart Rate: " + displayNumber.Next(95, 105).ToString() +
            "\nBlood Pressure: " + displayNumber.Next(95, 105).ToString() +
            "\nGlucose: " + displayNumber.Next(95, 105).ToString() +
            "\nRespiratory: " + displayNumber.Next(95, 105).ToString();
        }
    }

    IEnumerator your_timer()
    {
        enter = true;
        yield return new WaitForSeconds(1.0f);
        enter = false;
    }
}
