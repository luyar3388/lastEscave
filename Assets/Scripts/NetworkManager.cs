using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PN = Photon.Pun.PhotonNetwork;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject dbManager;
    public GameObject inputNamePanel;
    public GameObject playModePanel;
    public GameObject multiplayLobbyPanel;
    public GameObject makeRoomPanel;
    public GameObject searchRoomPanel;
    public GameObject roomPanel;
    public GameObject randomMatchFailPanel;
    public GameObject joinRoomFailPanel;
    public InputField makeRoomNameInput;
    public InputField searchRoomNameInput;
    public Text makeRoomNameOverlap;
    public Text searchRoomNameOverlap;
    public Text playerCountError;
    public Text roomText;
    public Text player1;
    public Text player2;
    public Text masterPlayer;
    public Text playerCount;
    public byte maxPlayer = 2;          //최대 플레이어 수
    private string gameVersion = "1.0";

    void Awake()
    {
        PN.AutomaticallySyncScene = true;
        PN.GameVersion = this.gameVersion;
    }


    void Update()
    {
        if (PN.InRoom)   //현재 방에 들어와 있는 상태면 텍스트 업데이트
        {
            Text[] arr = new Text[2] { player1, player2 };
            if(PN.PlayerList.Length == 1)
            {
                player1.text = "";
                player2.text = "";
            }
            else
            {
                for (int i = 0; i < PN.PlayerList.Length; i++)
                {
                    arr[i].text = PN.PlayerList[i].NickName;
                    GameManagement.staticMultiPlayerNames += PN.PlayerList[i].NickName + ",";
                }
            }
            playerCount.text = $"{PN.CurrentRoom.PlayerCount} / {PN.CurrentRoom.MaxPlayers}";
        }
        else
        {
            GameManagement.staticMultiPlayerNames = "";
        }
    }




    public void PlayModeCloseButton()
    {
        DBManager db = dbManager.GetComponent<DBManager>();
        db.DBCommand("deleteName", GameManagement.staticPlayerName, "", "");
        GameManagement.staticPlayerName = null;
        playModePanel.SetActive(false);
        inputNamePanel.SetActive(true);
    }
    public void SoloplayButton()
    {
        GameManagement.staticPlaymode = "soloplay";
        SceneManager.LoadScene("Game");
    }
    public void MultiplayButton()       //2인용 버튼을 누르면 OnConnectedToMaster() 함수 콜백
    {
        GameManagement.staticPlaymode = "multiplay";
        PN.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()  //정상적으로 서버에 접속되면 콜백되어 실행되며 OnJoinLobby() 함수 실행
    {
        PN.LocalPlayer.NickName = GameManagement.staticPlayerName;
        PN.JoinLobby();
        multiplayLobbyPanel.SetActive(true);
    }
    public void MultiplayCloseButton()       //로비에서 클로즈 버튼 눌렀을 때 OnDisconnected() 함수 콜백
    {
        PN.Disconnect();
    }
    public override void OnDisconnected(DisconnectCause cause)  //서버에서 나가지면 콜백되어 실행
    {
        GameManagement.staticPlaymode = null;
        multiplayLobbyPanel.SetActive(false);
        playModePanel.SetActive(true);
    }



    public void MakeRoomButton()    //로비에서 MakeRoom버튼 눌렀을 때
    {
        makeRoomPanel.SetActive(true);
        makeRoomNameOverlap.text = "";
    }   
    public void MakeRoomButton2()   //MakeRoom 패널에서 실제 방만들기 버튼 눌렀을때
    {
        if (makeRoomNameInput.text == "")
        {
            makeRoomNameOverlap.text = "방이름을 입력하세요";
            return;
        }

        if (makeRoomNameInput.text.Length > 10)
        {
            makeRoomNameOverlap.text = "10글자 이하로 입력해주세요";
            return;
        }
        CreateRoom();
    }
    public void CreateRoom()    //방만들기 버튼을 눌렀을 때 호출되는 함수, OnCreatedRoom 함수 콜백
    {
        PN.JoinOrCreateRoom(makeRoomNameInput.text, new RoomOptions { MaxPlayers = maxPlayer }, null);
    }
    public void MakeRoomCancel()    //MakeRoom 패널에서 취소버튼 눌렀을 때
    {
        makeRoomPanel.SetActive(false);
    }
    public override void OnJoinedRoom()     //방에 참가됐을때 콜백되는 함수
    {
        roomPanel.SetActive(true);
        roomText.text = $"방 이름:  {PN.CurrentRoom.Name}";
        playerCountError.text = "";
        makeRoomNameInput.text = "";
        makeRoomNameOverlap.text = "";
        makeRoomPanel.SetActive(false);
        searchRoomNameInput.text = "";
        searchRoomNameOverlap.text = "";
        searchRoomPanel.SetActive(false);
    }



    public void SearchRoomButton()    
    {
        searchRoomPanel.SetActive(true);
        searchRoomNameOverlap.text = "";
    }
    public void SearchRoomButton2()   
    {
        if (searchRoomNameInput.text == "")
        {
            searchRoomNameOverlap.text = "방이름을 입력하세요";
            return;
        }
        if (searchRoomNameInput.text.Length > 10)
        {
            searchRoomNameOverlap.text = "10글자 이하로 입력해주세요";
            return;
        }
        JoinSearchRoom();
    }
    public void JoinSearchRoom()      
    {
        PN.JoinRoom(searchRoomNameInput.text);
    }
    public void SearchRoomCancel()    
    {
        searchRoomPanel.SetActive(false);
    }



    public void RandomMatch()       //랜덤매칭 버튼 클릭 시
    {
        JoinRandomRoom();
    }
    public void JoinRandomRoom()    //생성된 방이 있다면 랜덤으로 매칭된다
    {
        PN.JoinRandomRoom();
    }



    public void RoomOutButton()     //방에서 나오는 버튼
    {
        LeaveRoom();
        roomPanel.SetActive(false);
    }
    public void LeaveRoom()     //방 나가는 함수
    {
        PN.LeaveRoom();
    }



    public override void OnCreateRoomFailed(short returnCode, string message)       //방 만들기 실패했을 때 콜백되는 함수
    {
        makeRoomNameOverlap.text = "중복된 방 이름입니다";
    }
    public override void OnJoinRoomFailed(short returnCode, string message)     //방 참가 실패시 콜백되는 함수
    {
        joinRoomFailPanel.SetActive(true);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)   //랜덤 방 참가
    {
        randomMatchFailPanel.SetActive(true);
    }
    public void RandomMatchFailCloseButton()    //랜덤 매칭 실패되고 나오는 패널 취소 버튼
    {
        randomMatchFailPanel.SetActive(false);
    }
    public void JoinRoomFailCloseButton()    //방 입장에 실패되고 나오는 패널 취소 버튼
    {
        joinRoomFailPanel.SetActive(false);
    }
    static public void StaticNetworkDisconnect()    //게임 실행중에 게임에서 나갈 시 실행될 함수
    {
        PN.Disconnect();
    }



    public void GameStartButton()    //게임 시작
    {
        if (PN.CurrentRoom.PlayerCount == maxPlayer && PN.IsMasterClient)
        {
            //PN.IsMessageQueueRunning = false;
            SceneManager.LoadScene("Game");
        }
        else if(PN.CurrentRoom.PlayerCount == maxPlayer && !PN.IsMasterClient)
        {
            playerCountError.text = "방장만 게임을 시작할 수 있습니다";
        }
        else if (PN.CurrentRoom.PlayerCount != PN.CurrentRoom.MaxPlayers)
        {
            playerCountError.text = "플레이어 수가 2명이 되어야 게임이 시작됩니다";
        }
    }
}