using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IMUDiagnostics : MonoBehaviour {

    private Text xValue;
    private Text yValue;
    private Text zValue;
    private Text wValue;
    private Text stateValue;

    // Use this for initialization
    void Start () {
        xValue = transform.Find("X").gameObject.GetComponent<Text>();
        yValue = transform.Find("Y").gameObject.GetComponent<Text>();
        zValue = transform.Find("Z").gameObject.GetComponent<Text>();
        wValue = transform.Find("W").gameObject.GetComponent<Text>();
        stateValue = transform.Find("State").gameObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateX(string val)
    {
        xValue.text = val;
    }

    public void UpdateY(string val)
    {
        yValue.text = val;
    }

    public void UpdateZ(string val)
    {
        zValue.text = val;
    }

    public void UpdateW(string val)
    {
        wValue.text = val;
    }

    public void UpdateState(string val)
    {
        stateValue.text = val;
    }
}
