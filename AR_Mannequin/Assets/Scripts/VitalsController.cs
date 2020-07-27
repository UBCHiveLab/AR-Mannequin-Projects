using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VitalsController : MonoBehaviour
{
    [SerializeField]
    private bool grayOut=true;
    public Vitals vital;
    public UnityEvent vitalAction;
    private Button vitalButton;
    private Toggle vitalToggle;
    

    private void Awake()
    {
        vitalButton = GetComponent<Button>();
        vitalToggle = GetComponent<Toggle>();
    }

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
        if (vitalButton != null)
        {
            vitalButton.onClick.AddListener(call);
        }
        
    }
    public void AddListener(UnityAction<bool> call)
    {
         if (vitalToggle != null)
        {
            vitalToggle.onValueChanged.AddListener(call);
            
        }
    }
}
