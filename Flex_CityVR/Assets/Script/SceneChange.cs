using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 비동기 씬로드 참고 --> 포기
// https://guks-blog.tistory.com/entry/UNITYC%EB%B9%84%EB%8F%99%EA%B8%B0-Scene-%EB%A1%9C%EB%93%9C%ED%95%98%EA%B8%B0
// https://blog.naver.com/developer_hyw/221043427652

public class SceneChange : MonoBehaviour
{
    //private bool changeScene;
    void Awake()
    {
        /*changeScene = false;
        StartCoroutine(MoveScene("3-1.Flexibility_Measurement"));*/
    }

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

    void OnMouseDown() // Coliider를 마우스로 클릭하면 발생하는 함수 (기존에 있는 이벤트함수 이용)
    {
        if (gameObject.name == "Monitor_screen")
        {
            //Debug.Log("클릭된 오브젝트 : " + gameObject.name);
            Player.instance.SavePosition();
            SceneManager.LoadScene("3-3.Result"); // 3-1. 씬 사용 할 것 (IMU 미연동 방지로 3-3으로 해둠)
            //changeScene = true;
        }
        

    }

}
