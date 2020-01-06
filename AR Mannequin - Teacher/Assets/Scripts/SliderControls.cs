using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderControls : MonoBehaviour
{

    private Text numberVal;
    private Slider slider;
    [SerializeField] private float max;
    [SerializeField] private float min;
    [SerializeField] private int evCode;

    private PhotonEvents photonEv;

    // Start is called before the first frame update
    void Start()
    {
        // initalize values
        photonEv = GameObject.FindGameObjectWithTag("Player").GetComponent<PhotonEvents>();
        slider = transform.GetChild(2).gameObject.GetComponent<Slider>();
        numberVal = transform.GetChild(1).gameObject.GetComponent<Text>();
    }

    public void InitializeValues(float value)
    {
        if (value < min) { value = min; }
        if (value > max) { value = max; }
        numberVal.text = value.ToString();
        slider.value = value;
        // photonEv.CallSliderEvent(evCode, value);
    }

    public void IncreaseOne()
    {
        float currValue = slider.value;
        currValue++;
        numberVal.text = currValue.ToString();
        slider.value = currValue;
        // photonEv.CallSliderEvent(evCode, currValue);
    }

    public void DecreaseOne()
    {
        float currValue = slider.value;
        currValue--;
        numberVal.text = currValue.ToString();
        slider.value = currValue;
        // photonEv.CallSliderEvent(evCode, currValue);
    }

    public void ChangeValue()
    {
        float currValue = slider.value;
        numberVal.text = currValue.ToString();
        // photonEv.CallSliderEvent(evCode, currValue);
    }

    public void SendValues()
    {
        float currValue = slider.value;
        photonEv.CallSliderEvent(evCode, currValue);
    }
}
