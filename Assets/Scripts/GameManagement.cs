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


        /*if (Input.GetButtonDown("Cancel")) //esc ��ư�� ������ ��
        {
            if (menuSet.activeSelf) //����������
            {
                menuSet.SetActive(false); //��
                isMenu = false;
                Time.timeScale = 1.0f; //�ð��� �ٽ� ���
            }
            else
            {                    //����������
                menuSet.SetActive(true); //Ŵ
                isMenu = true;
                Time.timeScale = 0f; //�ð��� ����
            }

            if (optionSet.activeSelf) //����޴��� ����������
            {
                optionSet.SetActive(false); //��
                Time.timeScale = 0f; //�ð��� ����
            }

            Time.fixedDeltaTime = 0.02f * Time.timeScale; //fixedUpdate ������ �ð��� �Բ� �ٲ������
        }

        if (staticDisplay == "��üȭ��")
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep; // ȭ���� ������ �ʰ� �ϴ� �Լ�
            Screen.SetResolution(1920, 1080, true); //��üȭ��

        }
        else if (staticDisplay == "â���")
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep; //ȭ���� ������ �ʰ� �ϴ� �Լ�
            Screen.SetResolution(1280, 720, false); //â���

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
        // ��������Ʈ ü�� ����
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

        string timeNum = $"{timer.GetCurrentTime():N2}";    //���� Ÿ���� �Ҽ��� �ι�°�ڸ� ������ ������
        float timeFloat = float.Parse(timeNum);           //����ȯ
        int timeInt = Mathf.RoundToInt(timeFloat);        //�ݿø�
        string minute = (timeInt / 60).ToString();      //�� ���
        string second = (timeInt % 60).ToString();      //�� ���
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
                db.DBCommand("updateTime", staticPlayerName, timeStr, timeNum);      //�÷���Ÿ���� ������Ʈ�ϴ� DB�Լ�
                SceneManager.LoadScene("Escape");
            }
        }
        else if(staticPlaymode == "multiplay")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                string PlayerNames =  staticMultiPlayerNames.TrimEnd(',');
                db.DBCommand("updateTime", PlayerNames, timeStr, timeNum);      //�÷���Ÿ���� ������Ʈ�ϴ� DB�Լ�
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
        Screen.sleepTimeout = SleepTimeout.NeverSleep; // ȭ���� ������ �ʰ� �ϴ� �Լ�
        Screen.SetResolution(1920, 1080, true); //��üȭ��
        Debug.Log("��üȭ�� �����Ϸ�");
        staticDisplay = "��üȭ��";
    }

    public void WindowToggleClick()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep; //ȭ���� ������ �ʰ� �ϴ� �Լ�
        Screen.SetResolution(1280, 720, false); //â���
        Debug.Log("â��� �����Ϸ�");
        staticDisplay = "â���";
    }


    public void EasyToggleClick(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("Easy ���̵� �����Ϸ�");
        }
        else
        {

        }
    }

    public void NormalToggleClick(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("Normal ���̵� �����Ϸ�");
        }
        else
        {

        }
    }

    public void HardToggleClick(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("Hard ���̵� �����Ϸ�");
        }
        else
        {

        }
    }
}
