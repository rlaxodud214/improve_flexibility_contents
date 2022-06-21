using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DBManager
{
    public static bool isDone = false;  // 비동기함수 컨트롤
    public static bool isSaveDone = false;

    // List <T> 리스트 직렬화를 위한 클래스
    [Serializable]
    public class Serialization<T>
    {
        [SerializeField]
        List<T> target;
        public List<T> ToList() { return target; }

        public Serialization(List<T> target)
        {
            this.target = target;
        }
    }

    // Dictionary <TKey, TValue> 딕셔너리 직렬화를 위한 클래스
    // ISerializationCallbackReceiver 인터페이스 -> OnBeforSerialize(), OnAfterDeserialize() 메소드를 구현하여
    // 직렬화를 지원하지 않는 자료구조를 커스텀 직렬화 할 수 있도록 함
    [Serializable]
    public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
    {
        [SerializeField]
        List<TKey> keys;
        [SerializeField]
        List<TValue> values;

        Dictionary<TKey, TValue> target;
        public Dictionary<TKey, TValue> ToDictionary() { return target; }

        public Serialization(Dictionary<TKey, TValue> target)
        {
            this.target = target;
        }

        // 직렬화 전에 호출 -> 딕셔너리의 keys와 values값들을 List에 저장(List는 직렬화가 가능하기 때문!!)
        public void OnBeforeSerialize()
        {
            keys = new List<TKey>(target.Keys);
            values = new List<TValue>(target.Values);
        }

        // 역직렬화 전에 호출 -> JSON형식의 string에서 key와 value를 출하여 Dictionary에 넣어줌
        public void OnAfterDeserialize()
        {
            var count = Math.Min(keys.Count, values.Count);
            target = new Dictionary<TKey, TValue>(count);
            for (var i = 0; i < count; ++i)
            {
                target.Add(keys[i], values​​[i]);
            }
        }
    }

    /// <summary>
    /// User객체 생성
    /// </summary>
    /// <param name="email">이메일</param>
    /// <param name="pw">비밀번호</param>
    /// <param name="name">이름</param>
    /// <param name="age">나이</param>
    /// <param name="gender">성별 true: 남성 / false: 여성</param>
    /// <param name="regDate">회원가입 날짜</param>
    /// <returns></returns>
    public static User CreateUser(string email, string pw, string name, int age, bool gender, DateTime regDate)
    {
        User user = new User();
        user.email = email;
        user.password = pw;
        user.name = name;
        user.age = age;
        user.gender = gender;
        user.registrationDate = regDate.ToString("yyyy-MM-dd");
        user.money = 0;
        user.pets = null;

        return user;
    }

    /// <summary>
    /// Measurement객체 생성
    /// </summary>
    /// <param name="flexion">굴곡</param>
    /// <param name="extension">신전</param>
    /// <param name="leftFlexion">좌측굴곡</param>
    /// <param name="rightFlexion">우측굴곡</param>
    /// <param name="leftRotation">좌측회전</param>
    /// <param name="rightRotation">우측회전</param>
    /// <param name="date">저장일(default는 null)</param>
    /// <returns></returns>
    public static Measurement CreateMeasurementData(
        int flexion, int extension, 
        int leftFlexion, int rightFlexion, 
        int leftRotation, int rightRotation,
        string date = null)
    {
        Measurement measurement = new Measurement();
        measurement.date = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
        if (date != null)
            measurement.date = date;
        measurement.flexion = flexion;
        measurement.extension = extension;
        measurement.leftFlexion = leftFlexion;
        measurement.rightFlexion = rightFlexion;
        measurement.leftRotation = leftRotation;
        measurement.rightRotation = rightRotation;
        measurement.totalFlexibility = flexion + extension + leftFlexion + rightFlexion + leftRotation + rightRotation;

        return measurement;
    }

    /// <summary>
    /// GameResult 객체 생성
    /// </summary>
    /// <param name="gameID">게임ID(어떤 게임인지)</param>
    /// <param name="playtime">플레이타임(시:분:초)</param>
    /// <param name="score">점수</param>
    /// <param name="reward">리워드</param>
    /// <param name="date">저장일(default는 null)</param>
    /// <returns></returns>
    public static GameResult CreateGameResult(string gameID, string playtime, int score, int reward, string date = null)
    {
        GameResult gameResult = new GameResult();
        gameResult.date = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
        if (date != null)
            gameResult.date = date;
        gameResult.gameID = gameID;
        gameResult.playtime = playtime;
        gameResult.score = score;
        gameResult.reward = reward;

        return gameResult;
    }

    // 초단위의 시간을 넣으면 string타입으로 변환(format: HH-mm-ss)
    public static string TimeToString(int sec)
    {
        string time = "";
        int hour, min;

        min = sec / 60;
        hour = min / 60;
        min = min % 60;

        time = hour.ToString("D2") + min.ToString("D2") + sec.ToString("D2");

        return time;
    }

    // User객체를 Json형식으로
    public static string UserToJson(User user)
    {
        string userInfo = "";
        userInfo = JsonUtility.ToJson(user);

        return userInfo;
    }

    // User 객체 로드
    public static IEnumerator LoadUser(Action<User> callback = null)
    {
        User user = new User();
        string info = "";

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = null
        }, result =>
        {
            if (result.Data == null || !result.Data.ContainsKey("User"))
            {
                Debug.Log("No User Data");
            }
            else
            {
                info = result.Data["User"].Value;
                user = JsonUtility.FromJson<User>(info);
                isDone = true;
            }
        }, (error) =>
        {
            Debug.Log("User 가져오기 실패");
        });

        yield return new WaitUntil(() => isDone == true);
        isDone = false;

        callback.Invoke(user);
    }

    // Measurement 객체 저장
    public static IEnumerator SaveMeasurement(Measurement measurement, string key = null)
    {
        Dictionary<string, Measurement> measurements = new Dictionary<string, Measurement>(); // 새로운 데이터
        Dictionary<string, Measurement> DBData = new Dictionary<string, Measurement>(); // DB 데이터
        string info = "";

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = null
        }, (result) =>
        {
            if (result.Data == null || !result.Data.ContainsKey("Measurement"))
            {
                Debug.Log("No Measurement Data");
            }
            else
            {
                info = result.Data["Measurement"].Value;
                Debug.Log(info);

                try
                {
                    DBData = JsonUtility.FromJson<Serialization<string, Measurement>>(info).ToDictionary();
                }
                catch (Exception e)
                {
                    Debug.Log(e.StackTrace);
                }
                finally
                {
                    isDone = true;
                    //Debug.Log("isDone : " + isDone);
                }
            }
        }, (error) =>
        {
            Debug.Log("Measurement 가져오기 실패");
        });

        yield return new WaitUntil(() => isDone == true);
        isDone = false;

        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        if (key != null)
            date = key;

        measurements.Add(date, measurement);   // 현재 날짜와 측정데이터를 key/value로 묶어서 딕셔너리에 추가
        // measurements 딕셔너리에 tempData 딕셔너리를 추가하는 코드 (역순으로 하기 위함)
        DBData.ToList().ForEach(x => measurements.Add(x.Key, x.Value));
        string resultInfo = JsonUtility.ToJson(new Serialization<string, Measurement>(measurements)); // 딕셔너리 직렬화

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>() { { "Measurement", resultInfo } },
            Permission = UserDataPermission.Public
        };

        PlayFabClientAPI.UpdateUserData(request, (finalResult) => {
            Debug.Log(resultInfo);
            isDone = true;
        }, (error) => { Debug.LogError("error occured: " + error.Error + " error msg: " + error.Error); });

        yield return new WaitUntil(() => isDone == true);
        isDone = false;
    }

    // Dictionary<string, Measurement> -> 측정값 전체 로드
    public static IEnumerator LoadMeasurement(Action<Dictionary<string, Measurement>> callback = null)
    {
        Dictionary<string, Measurement> measurements = new Dictionary<string, Measurement>();
        string info = "";

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = null
        }, result =>
        {
            if (result.Data == null || !result.Data.ContainsKey("Measurement"))
            {
                Debug.Log("No Measurement Data");
            }
            else
            {
                info = result.Data["Measurement"].Value;
                measurements = JsonUtility.FromJson<Serialization<string, Measurement>>(info).ToDictionary();
                isDone = true;
            }
        }, (error) =>
        {
            Debug.Log("Measurement 가져오기 실패");
        });

        yield return new WaitUntil(() => isDone == true);
        isDone = false;

        callback.Invoke(measurements);
    }

    // GaemResult 객체 저장
    public static IEnumerator SaveGameResult(GameResult gameResult, string key = null)
    {
        Dictionary<string, GameResult> gameResults = new Dictionary<string, GameResult>();
        string info = "";

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = null
        }, (result) =>
        {
            if (result.Data == null || !result.Data.ContainsKey("GameResult"))
            {
                Debug.Log("No GameResult Data");
            }
            else
            {
                info = result.Data["GameResult"].Value;
                Debug.Log(info);
                gameResults = JsonUtility.FromJson<Serialization<string, GameResult>>(info).ToDictionary();
                isDone = true;
            }
        }, (error) =>
        {
            Debug.Log("GameResult 가져오기 실패");
        });

        yield return new WaitUntil(() => isDone == true);
        isDone = false;

        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        if (key != null)
            date = key;
        gameResults.Add(date, gameResult);   // 현재 날짜와 측정데이터를 key/value로 묶어서 딕셔너리에 추가
        string resultInfo = JsonUtility.ToJson(new Serialization<string, GameResult>(gameResults)); // 딕셔너리 직렬화

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>() { { "GameResult", resultInfo } },
            Permission = UserDataPermission.Public
        };

        PlayFabClientAPI.UpdateUserData(request, (finalResult) => {
            Debug.Log(resultInfo);
        }, (error) => Debug.Log("GameResult 업데이트 실패"));

        yield return new WaitUntil(() => isDone == true);
        isDone = false;
    }

    // Dictionary<string, GaemResult> -> 게임결과 전체 로드
    public static IEnumerator LoadGameResult(Action<Dictionary<string, GameResult>> callback = null)
    {
        Dictionary<string, GameResult> gameResults = new Dictionary<string, GameResult>();
        string info = "";

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = null
        }, result =>
        {
            if (result.Data == null || !result.Data.ContainsKey("Measurement_test"))
            {
                Debug.Log("No GameResult Data");
            }
            else
            {
                info = result.Data["GameResult"].Value;
                gameResults = JsonUtility.FromJson<Serialization<string, GameResult>>(info).ToDictionary();
                isDone = true;
            }
        }, (error) =>
        {
            Debug.Log("GameResult 가져오기 실패");
        });

        yield return new WaitUntil(() => isDone == true);
        isDone = false;

        callback.Invoke(gameResults);
    }


    /*public enum Form{
    Measurement = 1,
    GameResult,
    Pet,
    }*/

    /*public static void SendRequest(string jsonString, Form form)
    {
        switch (form)
        {
            case Form.Measurement:
                var request1 = new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string>() { { "Measurement_test", jsonString } },
                    Permission = UserDataPermission.Public
                };
                PlayFabClientAPI.UpdateUserData(request1, (finalResult) => {
                    Debug.Log("성공적으로 Measurement를 업데이트했습니다.");
                }, (error) => Debug.Log("Measurement를 업데이트하지 못했습니다."));
                break;

            case Form.GameResult:
                var request2 = new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string>() { { "GameResult", jsonString } },
                    Permission = UserDataPermission.Public
                };
                PlayFabClientAPI.UpdateUserData(request2, (finalResult) => {
                    Debug.Log("성공적으로 GameResult를 업데이트했습니다.");
                }, (error) => Debug.Log("GameResult를 업데이트하지 못했습니다."));
                break;

            case Form.Pet:
                var request3 = new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string>() { { "Pet", jsonString } },
                    Permission = UserDataPermission.Public
                };
                PlayFabClientAPI.UpdateUserData(request3, (finalResult) => {
                    Debug.Log("성공적으로 Pet을 업데이트했습니다.");
                }, (error) => Debug.Log("Pet을 업데이트하지 못했습니다."));
                break;
        }
        
    }*/
}
