using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BrightnessManager : MonoBehaviour
{
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private Text brightnessTextUI = null;

    public GameObject BrightnessManagers;
    public GameObject BackgroundImage;
    

    public void Start()
    {
        LoadValues();
        Color color = BackgroundImage.GetComponent<Image>().color;
        BackgroundImage.GetComponent<Image>().color = color;
        
    }
    public void BrightnessSlider(float brightness)
    {
        brightness = brightness * 100;
        brightness = (float)Math.Round(brightness);
        brightnessTextUI.text = brightness.ToString();
    }

    public void SaveBrightnessButton()
    {
        float brightnessValue = brightnessSlider.value;
        PlayerPrefs.SetFloat("BrightnessValue", brightnessValue);
        LoadValues();
    }

    void LoadValues()
    {
        float brightnessValue = PlayerPrefs.GetFloat("BrightnessValue");
        brightnessSlider.value = brightnessValue;
        Color color = BackgroundImage.GetComponent<Image>().color;
        color.a = brightnessValue;
        BackgroundImage.GetComponent<Image>().color = color;
    }


}
