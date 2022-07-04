using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoginManager : MonoBehaviour
{
    public InputField EmailInput, PasswordInput;
    public Button LoginButton;
    public Text ErrorText;
    public bool emailValid, passwordValid;

    void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) PlayFabSettings.TitleId = "12497";
        LoginButton.interactable = false;
        LoginButton.interactable = true; // 6/21 테스트로 추가
    }

    private void Update()
    {
        if (emailValid && passwordValid)
            LoginButton.interactable = true;
    }

    public void TextInputChange(int num)
    {
        switch (num)
        {
            case 0:
                if (EmailInput.text.Length > 0)
                    emailValid = true;
                else
                    emailValid = false;
                break;
            default:
                if (PasswordInput.text.Length > 0)
                    passwordValid = true;
                else
                    passwordValid = false;
                break;
        }
    }

    // 로그인 메소드 -> 성공 시 로그 찍음
    public void Login()
    {
        // 테스트용
        var request = new LoginWithEmailAddressRequest { Email = "soun997@naver.com", Password = "123456" };

        // var request = new LoginWithEmailAddressRequest { Email = EmailInput.text, Password = PasswordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, LoginSuccess, ErrorHandler);
    }

    void LoginSuccess(LoginResult result)
    {
        /*Measurement measurement = DBManager.CreateMeasurementData(50, 50, 50, 50, 50, 50);
        StartCoroutine(DBManager.SaveMeasurement(measurement));
        StartCoroutine(LoadMeasurement());  // 저장하는 데에 시간이 걸리므로 읽어오기 전에 모두 저장할 수 있도록 시간지연을 주기 위해 아래에 선언한 LoadMeasuremet 코루틴 호출

        int sec = 341;
        string playtime = DBManager.TimeToString(sec);
        GameResult gameResult = DBManager.CreateGameResult("battle_city", playtime, 1080, 390);
        StartCoroutine(DBManager.SaveGameResult(gameResult));
        StartCoroutine(LoadGameResult());   // 저장하는 데에 시간이 걸리므로 읽어오기 전에 모두 저장할 수 있도록시간지연을 주기 위해 아래에 선언한 LoadGameResult 코루틴 호출*/

        //StartCoroutine(SaveTestValue());  // 테스트 데이터 저장할 떄는 아래 거 주석하고 돌리기
        StartCoroutine(CreateUserData());
    }

    public IEnumerator CreateUserData()
    {
        GameObject userDataManager = new GameObject("UserDataManager");
        userDataManager.AddComponent<UserDataManager>();

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("mainCity");
    }

    public IEnumerator SaveTestValue()
    {
        DateTime date_now = DateTime.Now;
        date_now = date_now.AddDays(-39);
        var list1 = new List<float>() { 80, 85, 86, 89, 92, 94, 95 }; // 굴곡 - 앞
        var list2 = new List<float>() { 20, 23, 24, 31, 33, 34, 37 }; // 신전 - 중간
        var list3 = new List<float>() { 40, 42, 45, 46, 47, 50, 55 }; // 편측 굴곡/신전 - 뒤
        var list4 = new List<float>() { 20, 23, 27, 29, 36, 38, 39 }; // 우측/좌측 회전 - 중간
        var index = 0;
        for (int i = 0; i <= 38; i++)
        {
            date_now = date_now.AddDays(1);
            var key = date_now.ToString("yyyy-MM-dd HH:mm:ss");
            if (i != 0 && i % 7 == 0)
            {
                index++;
                // Debug.Log("index ++, index : " + index);
            }

            // 범위내 무작위 값 산출
            int t1 = (int)UnityEngine.Random.Range(list1[0 + index], list1[1 + index]);
            int t2 = (int)UnityEngine.Random.Range(list2[0 + index], list2[1 + index]);
            int t3 = (int)UnityEngine.Random.Range(list3[0 + index], list3[1 + index]);
            int t4 = (int)UnityEngine.Random.Range(list3[0 + index], list3[1 + index]);
            int t5 = (int)UnityEngine.Random.Range(list4[0 + index], list4[1 + index]);
            int t6 = (int)UnityEngine.Random.Range(list4[0 + index], list4[1 + index]);

            Measurement measurement = DBManager.CreateMeasurementData(t1, t2, t3, t4, t5, t6, key);
            // Debug.Log(i + ", " + key + ", " + t1 + ", " + t2 + ", " + t3 + ", " + t4 + ", " + t5 + ", " + t6);
            // STEP2 SaveMeasurement의 매개변수로 measurement 전달
            yield return StartCoroutine(DBManager.SaveMeasurement(measurement, key));
            yield return new WaitForSeconds(1.5f);  // update rate 초과 error 방지
        }
    }

    public IEnumerator LoadMeasurement()
    {
        Dictionary<string, Measurement> measurements = new Dictionary<string, Measurement>();
        yield return new WaitForSeconds(5f);
        yield return StartCoroutine(DBManager.LoadMeasurement((result) => { measurements = result; }));
        // JSON에서 Dictionary로 잘 변환이 되었는지 key값 확인
        foreach (string key in measurements.Keys)
        {
            Debug.Log(key);
        }
    }

    // TEST
    public IEnumerator LoadGameResult()
    {
        Dictionary<string, GameResult> gameResults = new Dictionary<string, GameResult>();
        yield return new WaitForSeconds(5f);
        yield return StartCoroutine(DBManager.LoadGameResult((result) => { gameResults = result; }));
        // JSON에서 Dictionary로 잘 변환이 되었는지 key값 확인
        foreach (string key in gameResults.Keys)
        {
            Debug.Log(key);
        }
    }

    public void Register()
    {
        SceneManager.LoadScene("2.Register");
    }

    // Login 관련 Error 핸들러
    void ErrorHandler(PlayFabError error)
    {
        switch (error.Error)
        {
            case PlayFabErrorCode.AccountNotFound:
                ErrorText.text = "존재하지 않는 계정입니다.";
                break;
            case PlayFabErrorCode.InvalidParams:
                foreach (string k in error.ErrorDetails.Keys)
                {
                    if (k.Equals("Email"))
                    {
                        foreach (string s in error.ErrorDetails[k])
                        {
                            if (s.Equals("Email address is not valid."))
                                ErrorText.text = "올바른 이메일 형식이 아닙니다.";   // 찐으로 등록되지 않은 이메일일 경우

                        }
                    }
                    else if (k.Equals("Password"))
                    {
                        foreach (string s in error.ErrorDetails[k])
                        {
                            if (s.Equals("Password must be between 6 and 100 characters."))
                                ErrorText.text = "올바른 비밀번호 형식이 아닙니다.(6~13자)";
                        }
                    }
                    else
                    {
                        Debug.LogErrorFormat("Error: {0}, ErrorMessage: {1}", error.Error, error.ErrorMessage);
                        ErrorText.text = "예기치 못한 오류가 발생했습니다. 다시 시도해주세요.";
                    }
                }
                break;
            case PlayFabErrorCode.InvalidEmailOrPassword:
                ErrorText.text = "이메일 혹은 비밀번호가 일치하지 않습니다."; // 말은 이렇게 하지만 걍 비밀번호 틀린거임ㅇㅇ
                break;
            default:
                Debug.LogErrorFormat("Error: {0}, ErrorMessage: {1}", error.Error, error.ErrorMessage);
                ErrorText.text = "예기치 못한 오류가 발생했습니다. 다시 시도해주세요.";
                break;
        }
    }
}