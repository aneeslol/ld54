using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WinScreenController : MonoBehaviour
{
    [SerializeField] public TMP_Text TimesText;
    
    void Start()
    {
        var timesText = "Well Done!\n\nYour times:\n";
        for(var i = 0; i < 10; i++)
            if (Data.LevelTimes[i] != -1)
                timesText += (i + 1) + " - " + LevelController.GetTimeString(Data.LevelTimes[i]) + "\n";

        timesText += "Total: " + LevelController.GetTimeString(Data.LevelTimes.Sum());
        TimesText.text = timesText;
    }

    void Update()
    {
        
    }
}
