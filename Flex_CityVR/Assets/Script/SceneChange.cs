using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public static SceneChange instance;   // 싱글톤 
    void Awake()
    {
        SceneChange.instance = this;
    }
    public void Flex_Scene()
    {
        SceneManager.LoadScene("3-0.Main");
    }

    public void Main_Flex()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("mainCity");
    }

    public void contentsTelport()
    {
        // 트리거 통과 여부 확인해서 해당 콘텐츠로 이동시키기
        // value 값이 true인 key 값 찾기 -> using System.Linq 사용
        string get_key;
        get_key = Player.instance.KeySearch();
        switch (get_key)
        {
            case "T_hospital":
                int teleportPosition = Teleport.instance.teleportLocation.transform.childCount - 1; // 측정 장소
                StartCoroutine(Teleport.instance.TeleportLocation(teleportPosition));
                break;
            case "T_soccer":
                SceneManager.LoadScene("GoalKeeper");
                break;
/*            case "T_limbo":
                break;*/
            case "T_kayak":
                SceneManager.LoadScene("KayakGame");
                break;
            case "T_fly":
                SceneManager.LoadScene("Bird_MainScene");
                break; 
            case "T_battle":
                SceneManager.LoadScene("BattleCity");
                break;
/*            case "T_chef":
                SceneManager.LoadScene("Chef_Main");
                break;*/
            case "T_arrow":
                break;
            case "T_gondola":
                UIManager.instance.setInformType(5);
                int teleportPosition2 = Teleport.instance.teleportLocation.transform.childCount - 2; // 측정 장소
                StartCoroutine(Teleport.instance.TeleportLocation(teleportPosition2));
                StartCoroutine(Teleport.instance.GondolarAnimation());
                break;
            case "T_balloon":
                UIManager.instance.setInformType(5);
                StartCoroutine(Teleport.instance.GondolarAnimation());
                break;
            default:
                Debug.Log("<color=Red>입장한 포탈이 없습니다.</color>");
                break;
        }
        if (Player.instance.dic_contents["T_gondola"] || Player.instance.dic_contents["T_balloon"])
        {
            return;
        }
        else
        {
            Player.instance.dic_contents[get_key] = false;
        }
        //나중에 콘텐츠 수행하고 다시 메인시티로 돌아올 때 해당 Player의 딕셔너리 value false로 바꿔주기 -> 미정
    }
}
