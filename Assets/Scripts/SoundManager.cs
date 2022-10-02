//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class SoundManager : MonoBehaviour
//{
//    [SerializeField] private Slider volumeSlider = null;
//    [SerializeField] private Text volumeTextUI = null;

//    public GameObject AudioManager;

//    public void Start()
//    {
//        LoadValues();
//        DontDestroyOnLoad(AudioManager);
//    }
//    public void VolumeSlider(float volume)
//    {
//        volume = volume * 100;
//        volume = (float)Math.Round(volume);
//        volumeTextUI.text = volume.ToString();
//    }

//    public void SaveVolumeButton()
//    {
//        float volumeValue = volumeSlider.value;
//        PlayerPrefs.SetFloat("VolumeValue", volumeValue);
//        LoadValues();
//    }

//    void LoadValues()
//    {
//        float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
//        volumeSlider.value = volumeValue;
//        AudioListener.volume = volumeValue;
//    }

   
//}
