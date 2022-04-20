using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Public Fields
    [Header("Panels")]
    public List<GameObject> Panels;

    [Header("Disconnect Panel")]
    public GameObject DisconnectPanel;
    public InputField NicknameInput;

    [Header("Connect Panel")]
    public GameObject ConnectPanel;
    public Text FeedbackText;
    #endregion

    #region Private Fields

    string gameVersion = "1";

    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        Screen.SetResolution(1920, 1080, false);
    }

    #endregion

    #region Public Methods
    public void Connect()
    {
        FeedbackText.text = "";

        PhotonNetwork.LocalPlayer.NickName = NicknameInput.text;
        // 아이디와 비밀번호는 사용자 인증 요소로 넣어야 할 듯

        ShowPanel(ConnectPanel);

        // Photon 온라인 서버에 연결되었다면 방에 참가하도록, 그렇지 않다면 서버와 연결
        if (PhotonNetwork.IsConnected)
        {
            LogFeedback("Joining Room...");
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            LogFeedback("Connecting...");

            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    public void ShowPanel(GameObject CurPanel)
    {
        foreach (GameObject Panel in Panels)
        {
            Panel.SetActive(false);
        }
        CurPanel.SetActive(true);
    }

    #endregion

    #region Private Methods
    /// <summary>
    /// Logs the feedback in the UI view for the player, as opposed to inside the Unity Editor for the developer.
    /// </summary>
    /// <param name="message">Message.</param>
    void LogFeedback(string message)
    {
        // we do not assume there is a feedbackText defined.
        if (FeedbackText == null)
        {
            return;
        }

        // add new messages as a new line and at the bottom of the log.
        FeedbackText.text += System.Environment.NewLine + message;
    }
    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    // PhotonNetwork.ConnectUsingSettings() 호출 시 발생하는 콜백함수(연결 성공 시)
    public override void OnConnected()
    {
        
    }

    // 마스터 서버에 연결됐을 때의 Callback
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 5 }, null);   // 방 참가 또는 생성
    }

    // PhotonNetwork.JoinLobby 호출 시 발생하는 콜백함수
    public override void OnJoinedLobby()
    {
        
    }

    // Master 서버나 Lobby에 있을 때 방 만들기 가능
    // PhotonNetwork.CreateRoom() 혹은 PhotonNetwork.JoinOrCreateRoom() 호출 시
    public override void OnCreatedRoom()
    {
        
    }

    // 방에 참가했을 때의 Callback
    public override void OnJoinedRoom()
    {      
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    // PhotonNetwork.LeaveLobby() 호출 시 발생하는 콜백함수, 호출 후 마스터 서버에 연결됨
    public override void OnLeftLobby()
    {
        
    }

    // PhotonNetwork.Disconnect() 호출 시 발생하는 콜백함수
    public override void OnDisconnected(DisconnectCause cause)
    {
        
    }
    #endregion


}
