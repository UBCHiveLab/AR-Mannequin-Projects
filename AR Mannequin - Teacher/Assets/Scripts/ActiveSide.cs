using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSide : MonoBehaviour
{
    public Toggle LeftToggle;
    public Toggle RightToggle;
    public Toggle BothToggle;

    private Dictionary<int, string> codes;
    private string activeCode;
    
    // Start is called before the first frame update
    void Start()
    {
        activeCode = "both";
        codes = new Dictionary<int, string>()
        {
            {LeftToggle.GetInstanceID(), "l" },
            {RightToggle.GetInstanceID(), "r" },
            {BothToggle.GetInstanceID(), "both" }
        };
    }

    public void ChangeActiveCode(Toggle other)
    {
        activeCode = codes[other.GetInstanceID()];
    }
}
