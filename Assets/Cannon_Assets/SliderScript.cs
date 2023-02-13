using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField] TMP_Text sliderValue;
    [SerializeField] Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSliderValueChanged()
    {
        sliderValue.text = System.Math.Round(-slider.value,2).ToString() + "°";
    }
}
