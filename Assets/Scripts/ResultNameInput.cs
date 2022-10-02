using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class ResultNameInput : MonoBehaviour
{
    public GameObject dbManager;
    public GameObject inputNamePanel;
    public GameObject playModePanel;
    public GameObject connectFailPanel;
    public InputField playerNameInput;
    public Text inputNameWarning;
    public string inputName = null;

    void Awake()
    {
        inputName = playerNameInput.GetComponent<InputField>().text;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            InputNameInsertButton();
        }
    }


    public void InputNameInsertButton()
    {
        inputName = playerNameInput.text;
        inputName = Regex.Replace(inputName, @"[^0-9a-z]", "");
        playerNameInput.text = inputName;
        if (inputName == "")
        {
            inputNameWarning.text = "이름을 입력하세요";
            return;
        }
        if (inputName.Length > 10)
        {
            inputNameWarning.text = "10글자 이하로 입력해주세요";
            return;
        }

        inputNameWarning.text = "잠시만 기다려주세요";
        DBManager db = dbManager.GetComponent<DBManager>();
        db.DBCommand("insertName", inputName, "", "");
    }

    public void InputNameCloseButton()
    {
        if (GameManagement.staticPlayerName != null)
        {
            DBManager db = dbManager.GetComponent<DBManager>();
            db.DBCommand("deleteName", GameManagement.staticPlayerName, "", "");
            GameManagement.staticPlayerName = null;
        }
        SceneManager.LoadScene("Main");
    }

    public void ConnectFailCloseButton()
    {
        connectFailPanel.SetActive(false);
        playModePanel.SetActive(true);
    }
}
