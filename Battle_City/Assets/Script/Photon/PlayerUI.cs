using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    #region Private Fields

    [SerializeField]
    private Text playerNameText;
    private Player target;

    #endregion

    #region Public Fields

    [SerializeField]
    private Vector3 screenOffset = new Vector3(0f, 50f, 0f);

    [SerializeField]
    float characterControllerHeight = 0f;
    [SerializeField]
    Transform targetTransform;
    [SerializeField]
    Vector3 targetPosition;

    #endregion

    #region MonoBehaviour Callbacks
    // Start is called before the first frame update
    void Awake()
    {
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);   // 인스턴스된 UI는 Canvas에 위치해야하므로
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어가 제거되었을 때 UI만 덩그러니 남겨지는 것을 방지
        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    #endregion

    #region Public Methods

    // SendMessage를 통해 호출되는 메소드
    public void SetTarget(Player _target)
    {
        if (_target == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PlayerUI:SetTarget() target이 존재하지 않습니다.");
            return;
        }

        target = _target;   // 타겟을 받아옴

        CharacterController characterController = _target.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterControllerHeight = characterController.height;
        }

        targetTransform = target.transform;
        
        if (playerNameText != null)
        {
            playerNameText.text = target.photonView.Owner.NickName;
        }
    }

    public void LateUpdate()
    {
        // UI가 플레이어를 따라다니도록
        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
            targetPosition.y += characterControllerHeight;
            this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
        }
        else
        {
            Debug.LogError("플레이어의 Transform이 null입니다.");
        }
    }

    #endregion
}
