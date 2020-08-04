using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VitalsController : MonoBehaviour
{
    [SerializeField]
    private bool grayOut=true;
    public float countdownTime=10;
    public Vitals vital;
    public UnityEvent vitalAction;
    private Button vitalButton;
    private Toggle vitalToggle;
    

    public void OnVitalClick()
    {
        vitalAction.Invoke();
        DisableButton();
    }
    public void DisableButton()
    {
        if (vitalButton != null&&grayOut)
            ButtonGrayOut.DisableButton(vitalButton);
    }
    public void AddListener(UnityAction call)
    {
        vitalButton = GetComponent<Button>();
        if (vitalButton != null)
        {
            vitalButton.onClick.AddListener(call);
            Debug.Log("added listener"+vitalButton);
        }
        
    }
    public void AddListener(UnityAction<bool> call)
    {
        vitalToggle = GetComponent<Toggle>();
        if (vitalToggle != null)
        {
            vitalToggle.onValueChanged.AddListener(call);
            
        }
    }
}
