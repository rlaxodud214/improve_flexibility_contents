using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Public Fields

    public static GameManager Instance;

    public GameObject playerPrefab;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> 플레이어 프리팹이 존재하지 않습니다. " +
                "\nHierarchy의 GameManager에서 설정해주세요.");
        }
        else
        {
            // 기존 LocalPlayer 인스턴스가 존재하지 않을 때만 플레이어 인스턴스 생성
            if (Player.LocalPlayerInstance == null)
            {
                Debug.Log("플레이어 인스턴스 생성");
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(175.19f, -5.57f, 810.1f), Quaternion.identity, 0); // 플레이어 인스턴스 생성
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Photon Callbacks

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Main");
    }

 
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
            LoadCity();
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

            LoadCity();
        }
    }

    #endregion

    #region Public Methods

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region Private Methods

    void LoadCity()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not master Client");
        }
        Debug.LogFormat("PhotonNetwork : Loading Main City {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("City");    // 마스터 클라이언트인 경우에만 호출 가능
    }

    #endregion
}
