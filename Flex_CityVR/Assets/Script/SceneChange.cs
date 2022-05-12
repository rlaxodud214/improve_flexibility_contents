using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

// 비동기 씬로드 참고 --> 포기
// https://guks-blog.tistory.com/entry/UNITYC%EB%B9%84%EB%8F%99%EA%B8%B0-Scene-%EB%A1%9C%EB%93%9C%ED%95%98%EA%B8%B0
// https://blog.naver.com/developer_hyw/221043427652

public class SceneChange : MonoBehaviour
{
    public static SceneChange instance;   // 싱글톤 
    void Awake()
    {
        SceneChange.instance = this;
    }
    //private bool changeScene;
/*    void Awake()
    {
        changeScene = false;
        StartCoroutine(MoveScene("3-1.Flexibility_Measurement"));
    }*/

    /*    IEnumerator MoveScene(string moveSceneName)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(moveSceneName);

            async.allowSceneActivation = false; 
            while (!(async.isDone))
            {
                float progress = async.progress * 100f;
                Debug.Log("async progress : " + async.progress + "%");

                if (changeScene && async.progress >= 0.9f)
                    async.allowSceneActivation = true; // true 가 되면 씬을 로드함
            }
            yield return null;
        }*/

    void Start()
    {
        //Invoke("test", 5f);
    }

/*    void OnMouseDown() // Coliider를 마우스로 클릭하면 발생하는 함수 (기존에 있는 이벤트함수 이용)
    {
        if (gameObject.name == "Monitor_screen")
        {
            //Debug.Log("클릭된 오브젝트 : " + gameObject.name);
            //Player.instance.SavePosition();
            SceneManager.LoadScene("3-3.Result"); // 3-1. 씬 사용 할 것 (IMU 미연동 방지로 3-3으로 해둠)
            //changeScene = true;
        }
    }*/

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
                StartCoroutine(Teleport.instance.TeleportMeasure());
                break;
            case "T_soccer":
                SceneManager.LoadScene("GoalKeeper");
                break;
            case "T_limbo":
                break;
            case "T_kayak":
                SceneManager.LoadScene("KayakGame");
                break;
            case "T_fly":
                break;
            case "T_window":
                break;
            case "T_battle":
                break;
            case "T_chef":
                break;
            case "T_arrow":
                break;
            default:
                Debug.Log("<color=Red>입장한 포탈이 없습니다.</color>");
                break;
        }
        Player.instance.dic_contents[get_key] = false;
        //나중에 콘텐츠 수행하고 다시 메인시티로 돌아올 때 해당 Player의 딕셔너리 value false로 바꿔주기 -> 미정
    }
}
