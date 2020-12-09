using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// define all the vital actions with a description value
public enum VitalActions
{
    [Description("End Tidal CO2 Detector")]
    EndtidalCO2Detector,
    [Description("Take Temperature")]
    TakeTemperature,
    [Description("ECG Leads")]
    ECGLeads,
    [Description("Femoral Pulse")]
    FemoralPulse,
    [Description("Start IV")]
    StartIV,
    [Description("Abdominal Exam")]
    AbdominalExam,
    [Description("Insert LO")]
    InsertLO,
    [Description("Lung Sounds")]
    LungSounds,
    [Description("Heart Sounds")]
    HeartSounds,
    [Description("Chest Compression")]
    ChestCompression,
    [Description("O2 NP")]
    O2NP,
    [Description("O2 Facemask")]
    O2Facemask,
    BVM,
    [Description("O2 Sat")]
    O2Sat,
    [Description("IV Medication")]
    IVMedication,
    [Description("IM Medication")]
    IMMedication,
    [Description("Draw Blood")]
    DrawBlood,
    Glucose,
    [Description("Defib Pads")]
    DefibPads,
    [Description("Cartoid Pulse")]
    CartoidPulse,
    [Description("Radial Pulse")]
    RadialPulse,
    [Description("Chest Tube L")]
    ChestTubeL,
    [Description("Chest Tube R")]
    ChestTubeR,
    [Description("Needle Decompression R")]
    NeedleDecompressionR,
    [Description("Needle Decompression L")]
    NeedleDecompressionL,
    [Description("Rectal Exam")]
    RectalExam,
    [Description("Blood Pressure Cuff")]
    BloodPressureCuff
}

// define where the student/user is at in relevant to the manikin
public enum UserPosition
{
    none,
    head,
    chest,
    arm,
    bottom
}

/// <summary>
/// Converts enum description to text
/// https://stackoverflow.com/questions/1799370/getting-attributes-of-enums-value
/// </summary>
public static class EnumExtensions
{

    // This extension method is broken out so you can use a similar pattern with 
    // other MetaData elements in the future. This is your base method for each.
    public static T GetAttribute<T>(this System.Enum value) where T : System.Attribute
    {
        var type = value.GetType();
        var memberInfo = type.GetMember(value.ToString());
        var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
        return attributes.Length > 0
          ? (T)attributes[0]
          : null;
    }

    // This method creates a specific call to the above method, requesting the
    // Description MetaData attribute.
    public static string ToName(this System.Enum value)
    {
        var attribute = value.GetAttribute<DescriptionAttribute>();
        return attribute == null ? value.ToString() : attribute.Description;
    }

}

public class VitalsManager : Singleton<VitalsManager>
{
    List<VitalsController> vitalsControllerList;
    List<VitalButtonGroupController> vitalButtonGroupControllers;
    public bool isInTimer = false;
    private bool isVitalUIOn = false;
    [SerializeField]
    private Text timerText,actionNotification;
    [SerializeField]
    private HoldButton holdButton;
    [SerializeField]
    private Button chestCompressionButton;


    //static Dictionary<UserPosition, Vitals[]> positionVitalPairs=new Dictionary<UserPosition, Vitals[]>();

    private UserPosition currentUserPosition = UserPosition.none;

    void Awake()
    {
        vitalsControllerList = GetComponentsInChildren<VitalsController>().ToList();
        vitalButtonGroupControllers = GetComponentsInChildren<VitalButtonGroupController>().ToList();
        foreach (VitalsController vitalsController in vitalsControllerList)
        {
            Debug.Log(vitalsController.name);
            vitalsController.AddListener(() => OnVitalButtonClick(vitalsController));
            vitalsController.AddListener((bool a) => OnVitalButtonClick(vitalsController));
        }

        TurnOffAllVitalUI();

        timerText.gameObject.SetActive(false);
        actionNotification.gameObject.SetActive(false);
        
    }
    #region PUBLIC_METHODS
    // turn on button groups bnased on the user position
    public void VitalsUIControlBasedOnUserPosition(UserPosition userPosition)
    {

        if (userPosition == UserPosition.none)
        {
            TurnOffAllVitalUI();
        }

        if (currentUserPosition != userPosition)
        {
            Debug.Log(userPosition);
            currentUserPosition = userPosition;
            TurnOffAllVitalUI();
            if(isVitalUIOn)
            TurnOnCurrentVital();
            
        }
        
    }
    public void ToggleVitalUI(bool isOn)
    {
        if (isOn)
        {
            TurnOnCurrentVital();
        }
        else
        {
            TurnOffAllVitalUI();
        }
        isVitalUIOn = isOn;
    }
    #endregion
    #region PRIVATE_METHODS
    private void TurnOffAllVitalUI()
    {
        // deactivate all vitals ui
        vitalButtonGroupControllers.ForEach(x => x.gameObject.SetActive(false));
    }
    private void TurnOnCurrentVital()
    {
        if (currentUserPosition != UserPosition.none && !isInTimer)
        {
            vitalButtonGroupControllers.Find(x => x.correspondingUserPosition == currentUserPosition).gameObject.SetActive(true);
        }
    }
    
    #endregion


    #region TIMER_RELATED_METHODS
    public void OnVitalButtonClick(VitalsController vitalsController)
    {
        if (vitalsController.name == "Chest Compression")
        {
            StartCoroutine(ChestTimer(vitalsController));
        }
        else
        {
            StartCoroutine(Timer(vitalsController));
        }
        Debug.Log(vitalsController);
    }
    private IEnumerator Timer(VitalsController vitalsController)
    {
        // activate timer and action notification text
        timerText.gameObject.SetActive(true);
        actionNotification.gameObject.SetActive(true);
        // set action notification text with corresponding action
        actionNotification.text = "OCS is processing action " + '"' + vitalsController.vitalAction.ToName() + '"' + ". Please wait ...";
        // set countdown time with corresponding time
        float countdown = vitalsController.countdownTime;
        // determine whether the countdown time is more than a minute
        string timerFormat = countdown > 60 ? "0:00.00" : "00.00";

        isInTimer = true;
        
        TurnOffAllVitalUI();
        // invoke actions that should be called before timer
        // this is usually to send message to server
        vitalsController.InvokeActionBeforeTimer();


        Debug.Log("Timer Starting");
        while (countdown > 0)
        {
            timerText.text = "Please wait "+ FloatToTime(countdown, timerFormat);
            countdown -= Time.deltaTime;
            yield return null;
        }
        isInTimer = false;
        // hide timer text
        timerText.gameObject.SetActive(false);
        actionNotification.text= '"' + vitalsController.vitalAction.ToName() + '"' + " has been processed";
        // action notification text disappear in a second after the countdown ends
        StartCoroutine(HideActionNotification());
        // invoke actions that should be called after timer
        // this is for action that actually should happen
        vitalsController.InvokeActionAfterTimer();
        // re-turn on current vital buttons
        TurnOnCurrentVital();
    }

    private IEnumerator ChestTimer(VitalsController vitalsController)
    {
        // set countdown time with corresponding time
        float countdown = 0;
        // determine whether the countdown time is more than a minute
        string timerFormat = countdown > 60 ? "0:00.00" : "00.00";

        isInTimer = true;

        TurnOffAllVitalUI();
        chestCompressionButton.gameObject.SetActive(true);
        // invoke actions that should be called before timer
        // this is usually to send message to server
        while (holdButton.buttonPressed == false)
        {
            yield return null;
        }
        vitalsController.InvokeChestCompressionActionBeforeTimer();
        Debug.Log("Timer Starting");
        while (holdButton.buttonPressed == true)
        {
            countdown += Time.deltaTime;
            yield return null;
        }
        isInTimer = false;
        vitalsController.countdownTime = countdown;
        // invoke actions that should be called after timer
        // this is for action that actually should happen
        vitalsController.InvokeChestCompressionActionAfterTimer();
        // re-turn on current vital buttons
        chestCompressionButton.gameObject.SetActive(false);
        TurnOnCurrentVital();
    }
    // action notification text disappear in a second after the countdown ends
    private IEnumerator HideActionNotification()
    {
        yield return new WaitForSeconds(1f);
        actionNotification.gameObject.SetActive(false);
    }
    public string FloatToTime(float toConvert, string format)
    {
        switch (format)
        {
            case "00.0":
                return string.Format("{0:00}:{1:0}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 10) % 10));//miliseconds
            case "#0.0":
                return string.Format("{0:#0}:{1:0}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 10) % 10));//miliseconds
            case "00.00":
                return string.Format("{0:00}:{1:00}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 100) % 100));//miliseconds
            case "00.000":
                return string.Format("{0:00}:{1:000}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
            case "#00.000":
                return string.Format("{0:#00}:{1:000}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 1000) % 1000));//miliseconds

            case "#0:00":
                return string.Format("{0:#0}:{1:00}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60);//seconds

            case "#00:00":
                return string.Format("{0:#00}:{1:00}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60);//seconds

            case "0:00.0":
                return string.Format("{0:0}:{1:00}.{2:0}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 10) % 10));//miliseconds
            case "#0:00.0":
                return string.Format("{0:#0}:{1:00}.{2:0}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 10) % 10));//miliseconds
            case "0:00.00":
                return string.Format("{0:0}:{1:00}.{2:00}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 100) % 100));//miliseconds
            case "#0:00.00":
                return string.Format("{0:#0}:{1:00}.{2:00}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 100) % 100));//miliseconds
            case "0:00.000":
                return string.Format("{0:0}:{1:00}.{2:000}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
            case "#0:00.000":
                return string.Format("{0:#0}:{1:00}.{2:000}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
        }
        return "error";
    }
    #endregion
}

