using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharManager : MonoBehaviour
{
    public static CharManager instance;
    public UnityEngine.UI.Button LeftBtn, RightBtn;
    private int index;
    public GameObject[] characters; // 캐릭터들 Transform 배열
    public List<float> positionX = new List<float>(); //캐릭터들 Transfrom position의 X 값 리스트
    public Camera RenderCam;
    [HideInInspector]
    public GameObject currentCharacter;
    public Transform PlayerCharacter;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) return;


        #region 변수 초기화
        index = 0;
        for (int i = 0; i < characters.Length; i++)
        {
            positionX.Add(characters[i].transform.localPosition.x);
        }
        #endregion
    }

    public void LeftButton()
    {
        index -= 1;
        if(index < 0)
        {
            index = positionX.Count-1;
        }
        RenderCam.transform.localPosition = new Vector3(positionX[index], 0f, 4f);
    }

    public void RightButton()
    {
        index += 1;
        if(index > positionX.Count - 1)
        {
            index = 0;
        }
        RenderCam.transform.localPosition = new Vector3(positionX[index], 0f, 4f);
    }

    public void SelectCharacter()
    {
        currentCharacter = characters[index];
        if(PlayerCharacter.childCount > 1)
        {
            Destroy(PlayerCharacter.transform.GetChild(1).gameObject);
        }
        GameObject obj = Instantiate<GameObject>(currentCharacter, PlayerCharacter);
        obj.transform.localPosition = new Vector3(0f, 0f, -0.5f);
        obj.transform.localScale = new Vector3(0.44f, 0.44f, 0.44f);

        print("선택된 캐릭터는 : "+ currentCharacter.name);
    }
}
