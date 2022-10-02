using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DBManager : MonoBehaviour
{
	public string url;

	public GameObject connectFailPanel;
	public GameObject inputNamePanel;
	public GameObject playModePanel;

	public Text inputNameWarning;
	public Text text0;
	public Text text1;
	public Text text2;
	public Text text3;
	public Text text4;
	public Text text5;
	public Text text6;
	public Text text7;
	public Text text8;
	public Text text9;
	public Text text10;
	public Text text11;
	public Text text12;
	public Text text13;
	public Text text14;
	public Text text15;
	public Text text16;
	public Text text17;
	public Text text18;
	public Text text19;

	//command 목록 { insertName, deleteName, updateTime, select, rank, init }
	public void DBCommand(string command, string name, string var1, string var2)	
    {
		StartCoroutine(DBConnect(command, name, var1, var2));
	}

	IEnumerator DBConnect(string command, string name, string var1, string var2)
	{
		WWWForm form = new WWWForm();
		form.AddField("command", command);
		form.AddField("name", name);
		form.AddField("var1", var1);
		form.AddField("var2", var2);
		UnityWebRequest www = UnityWebRequest.Post(url, form);      //url에 명령문 매핑
		yield return www.SendWebRequest();
		string dbResult = www.downloadHandler.text;
		Debug.Log(dbResult);

		if (dbResult == "{\"message\": \"Internal server error\"}")
        {
			dbResult = "connectFail";
		}

		if(command == "rank" && dbResult != "connectFail")
        {
			Text[] textArr = new Text[20] {text0, text1, text2, text3, text4, text5, text6, text7, text8, text9,
							text10, text11, text12, text13, text14, text15, text16, text17, text18, text19};
			string[] rankSplit = dbResult.Split(',');
			for (int i = 0; i < 20; i++)
			{
				textArr[i].text = rankSplit[i];
			}
		}
		else if(command == "rank" && dbResult == "connectFail")
        {
			connectFailPanel.SetActive(true);
		}

		if (command == "updateTime")
		{
			SceneManager.LoadScene("Escape");
			Destroy(GameObject.Find("SoundManager"));
		}

		if (command == "insertName" && dbResult == "success")
		{
			GameManagement.staticPlayerName = name;
			inputNameWarning.text = "";
			playModePanel.SetActive(true);
			inputNamePanel.SetActive(false);
			Destroy(GameObject.Find("SoundManager"));
		}
		else if (command == "insertName" && dbResult == "connectFail")
		{
			GameManagement.staticPlayerName = name;
			inputNameWarning.text = "";
			connectFailPanel.SetActive(true);
			inputNamePanel.SetActive(false);
			Destroy(GameObject.Find("SoundManager"));
		}
		else if (command == "insertName" && dbResult == "overlap")
		{
			inputNameWarning.text = "중복된 이름입니다";
		}
		else if (command == "insertName" && dbResult == "fail")
		{
			inputNameWarning.text = "잠시 후 다시시도 바랍니다";
		}
	}
}
