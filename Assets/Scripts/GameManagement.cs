using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManagement : MonoBehaviourPunCallbacks
{
    public GameObject helpSet;
    public GameObject rankingSet;
    public GameObject configurationSet;
    public GameObject menuSet;
    public GameObject optionSet;
    public GameObject timeManager;
    public GameObject dbManager;
    //public GameObject Mainmenu;
    public bool isMenu = false;

    public static string staticPlayerName;
    public static string staticMultiPlayerNames;
    public static string staticPlayTime;
    public static string staticDisplay;
    public static string staticPlaymode;
    public static float staticLimitTime = 300.0f;
    public static bool staticGetLighter = false;
    public static bool staticDoorTrigger = false;

    private static GameManagement _instance;
    public static GameManagement Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManagement)) as GameManagement;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }






    private void Awake()
    {

    }
    private void Update()
    {


        /*if (Input.GetButtonDown("Cancel")) //esc 버튼을 눌렀을 때
        {
            if (menuSet.activeSelf) //켜져있으면
            {
                menuSet.SetActive(false); //끔
                isMenu = false;
                Time.timeScale = 1.0f; //시간을 다시 재생
            }
            else
            {                    //꺼져있으면
                menuSet.SetActive(true); //킴
                isMenu = true;
                Time.timeScale = 0f; //시간을 멈춤
            }

            if (optionSet.activeSelf) //사운드메뉴가 켜져있으면
            {
                optionSet.SetActive(false); //끔
                Time.timeScale = 0f; //시간을 멈춤
            }

            Time.fixedDeltaTime = 0.02f * Time.timeScale; //fixedUpdate 프레임 시간도 함께 바꿔줘야함
        }

        if (staticDisplay == "전체화면")
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep; // 화면이 꺼지지 않게 하는 함수
            Screen.SetResolution(1920, 1080, true); //전체화면

        }
        else if (staticDisplay == "창모드")
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep; //화면이 꺼지지 않게 하는 함수
            Screen.SetResolution(1280, 720, false); //창모드

        }*/
    }

    public void SetPlayerName(string name)
    {
        staticPlayerName = name;
    }

    public void GameContinue()
    {
        menuSet.SetActive(false);
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Login");
    }

    public void Dead()
    {
        SceneManager.LoadScene("Die");
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Game")
        {
            menuSet.SetActive(false);
            optionSet.SetActive(false);
        }
    }

    void OnDisable()
    {
        // 델리게이트 체인 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void Manager()
    {
        //Mainmenu.SetActive(false);
        helpSet.SetActive(true);
    }

    public void init()
    {
        NetworkManager.StaticNetworkDisconnect();
        SceneManager.LoadScene("Main");
    }

    public void RePlay()
    {
        SceneManager.LoadScene("Game");
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void GameConfiguration()
    {
        //Mainmenu.SetActive(false);
        configurationSet.SetActive(true);
    }

    public void ConfigurationCloseButton()
    {
        configurationSet.SetActive(false);
        //Mainmenu.SetActive(true);
    }

    public void GameClear()
    {
        DBManager db = dbManager.GetComponent<DBManager>();
        TimeManager timer = timeManager.GetComponent<TimeManager>();

        string timeNum = $"{timer.GetCurrentTime():N2}";    //현재 타임을 소수점 두번째자리 까지만 가져옴
        float timeFloat = float.Parse(timeNum);           //형변환
        int timeInt = Mathf.RoundToInt(timeFloat);        //반올림
        string minute = (timeInt / 60).ToString();      //분 계산
        string second = (timeInt % 60).ToString();      //초 계산
        string timeStr = minute + "m" + second + "s";
        staticPlayTime = timeStr;

        if(staticPlaymode == "soloplay" || staticPlaymode == null)
        {
            if(staticPlayerName == null)
            {
                staticPlayerName = "";
                SceneManager.LoadScene("Escape");
            }
            else
            {
                db.DBCommand("updateTime", staticPlayerName, timeStr, timeNum);      //플레이타임을 업데이트하는 DB함수
                SceneManager.LoadScene("Escape");
            }
        }
        else if(staticPlaymode == "multiplay")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                string PlayerNames =  staticMultiPlayerNames.TrimEnd(',');
                db.DBCommand("updateTime", PlayerNames, timeStr, timeNum);      //플레이타임을 업데이트하는 DB함수
                SceneManager.LoadScene("Escape");
            }
            else
            {
                SceneManager.LoadScene("Escape");
            }
        }

    }

    public void TestCharacter()
    {
        SceneManager.LoadScene("Game");
        //DontDestroyOnLoad(timeManager);
        Destroy(GameObject.Find("SoundManager"));
    }

    public void GameSave()
    {

    }

    public void GameLoad()
    {

    }

    public void GameOption()
    {
        menuSet.SetActive(false);
        optionSet.SetActive(true);
    }

    public void BackButton()
    {
        optionSet.SetActive(false);
        menuSet.SetActive(true);
    }

    public void CloseButton()
    {
        optionSet.SetActive(false);
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    public void HelpCloseButton()
    {
        helpSet.SetActive(false);
        //Mainmenu.SetActive(true);
    }

    public void FullToggleClick()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep; // 화면이 꺼지지 않게 하는 함수
        Screen.SetResolution(1920, 1080, true); //전체화면
        Debug.Log("전체화면 설정완료");
        staticDisplay = "전체화면";
    }

    public void WindowToggleClick()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep; //화면이 꺼지지 않게 하는 함수
        Screen.SetResolution(1280, 720, false); //창모드
        Debug.Log("창모드 설정완료");
        staticDisplay = "창모드";
    }


    public void EasyToggleClick(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("Easy 난이도 설정완료");
        }
        else
        {

        }
    }

    public void NormalToggleClick(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("Normal 난이도 설정완료");
        }
        else
        {

        }
    }

    public void HardToggleClick(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("Hard 난이도 설정완료");
        }
        else
        {

        }
    }
}
