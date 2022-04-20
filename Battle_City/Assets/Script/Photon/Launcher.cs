using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields
    
    [SerializeField]
    byte maxPlayersPerRoom = 10;

    #endregion

    #region Private Fields

    string gameVersion = "1";   // 클라이언트의 버전 넘버, 사용자들은 GameVersion으로 분리될 수 있음
    
    bool isConnecting;  // City에서 Leave 할 경우 자동으로 Master 서버에 연결되므로, OnConnectedToMaster 콜백이 실행되어 자동으로 재참여 됨, 이를 방지하기 위함


    #endregion region

    #region Public Fields

    [Header("Launcher Panel")]
    public GameObject LauncherPanel;

    [Header("Control Panel")]
    public GameObject ControlPanel;
    public Text ProgressText;

    #endregion

    #region MonoBehaviour CallBacks

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;    // PhotonNetwork.LoadLevel() 호출 시, 모든 클라이언트들은 동일한 Scene을 자동으로 로드하게 됨
    }
    // Start is called before the first frame update
    void Start()
    {
        LauncherPanel.SetActive(true);
        ControlPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Public Methods


    /// <summary>
    /// 서버와의 연결
    /// 이미 연결되어 있다면, 랜덤한 방에 입장
    /// 아직 연결되지 않았다면, Photon Cloud Network에 연결
    /// </summary>
    public void Connect()
    {
        isConnecting = true;

        LauncherPanel.SetActive(false);
        ControlPanel.SetActive(true);

        // 서버와 연결되어 있을 때
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom(); // 방이 단 하나도 존재하지 않는다면 OnJoinRandomFailed 콜백
        }
        // 서버와 연결되어 있지 않을 때
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);

        LauncherPanel.SetActive(true);
        ControlPanel.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

        Debug.LogFormat("플레이어 {0}(이)가 방에 참가했습니다.\nActorNumber: {1}", PhotonNetwork.LocalPlayer.NickName, PhotonNetwork.LocalPlayer.ActorNumber);

        PhotonNetwork.LoadLevel("City - Day");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\n" +
            "Calling: PhotonNetwork.CreateRoom");

        PhotonNetwork.CreateRoom("Battle City", new RoomOptions { MaxPlayers = maxPlayersPerRoom });  // 방 참여 실패 시(방이 없을 때), 방을 만듦, OnCreateRoom과 OnJoinedRoom 콜백 호출
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnCreateRoomFailed() was called by PUN. 이미 같은 이름을 가진 방이 생성되어 있습니다.");
    }

    #endregion
}
