using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DisplayManager : MonoBehaviour
{
    [SerializeField] private Toggle FullDisplay = null;

    public GameObject displayManager;

    void Start()
    {
        LoadValues();
        
    }

    public void SaveToggleButton()
    {
        if (GameManagement.staticDisplay == "전체화면")
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep; // 화면이 꺼지지 않게 하는 함수
            Screen.SetResolution(1920, 1080, true); //전체화면
            PlayerPrefs.SetString("DisplayValue", GameManagement.staticDisplay);
            

        }
        else if (GameManagement.staticDisplay == "창모드")
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep; //화면이 꺼지지 않게 하는 함수
            Screen.SetResolution(1280, 720, false); //창모드
            PlayerPrefs.SetString("DisplayValue", GameManagement.staticDisplay);
            Debug.Log(GameManagement.staticDisplay);

        }
        LoadValues();
    }

    void LoadValues()
    {
        string displayValue = PlayerPrefs.GetString("DisplayValue");
        if(displayValue == "전체화면")
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep; // 화면이 꺼지지 않게 하는 함수
            Screen.SetResolution(1920, 1080, true); //전체화면
            FullDisplay.GetComponent<Toggle>().isOn = true;
        }
        else if (displayValue == "창모드")
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep; //화면이 꺼지지 않게 하는 함수
            Screen.SetResolution(1280, 720, false); //창모드
            FullDisplay.GetComponent<Toggle>().isOn = false;
        }
    }
}
