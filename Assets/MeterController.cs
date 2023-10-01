using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeterController : MonoBehaviour
{
    [SerializeField] public Image MeterImage;
    [SerializeField] public TMP_Text Text;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(Color color)
    {
        MeterImage.color = color;
    }

    public void SetMeter(float percentage)
    {
        MeterImage.transform.localScale = new Vector3(percentage * .01f * .95f, 1, 1);
    }

    public void SetText(string text)
    {
        Text.text = text;
    }
}
