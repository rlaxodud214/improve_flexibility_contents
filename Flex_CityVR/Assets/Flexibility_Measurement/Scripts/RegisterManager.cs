using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour
{
    public InputField EmailInput, PasswordInput, VerifyInput, NameInput, AgeInput;
    public Button RegisterButton;
    public Text ErrorText;
    public bool gender;    // true: male, false: female

    public bool emailVerified, passwordVerified, nameVerified, ageVerified;

    void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) PlayFabSettings.TitleId = "12497";

        gender = true;  // 초기 성별값은 남성으로
        RegisterButton.interactable = false;    // form의 모든 내용을 기입하지 않으면 Register 불가하도록
        emailVerified = passwordVerified = nameVerified = ageVerified = false;  // form의 내용을 기입하면 true
    }

    void Update()
    {
        // form의 모든 내용을 기입하면 Register 버튼 활성화
        if (emailVerified && passwordVerified && nameVerified && ageVerified)
        {
            RegisterButton.interactable = true;
        }
        else
        {
            RegisterButton.interactable = false;
        }
    }

    // 토글에 따라 남성 / 여성 여부 결정
    public void OnToggleValueChanged(bool isMale)
    {
        gender = isMale;
    }

    // form 완성 여부 -> 완성 시 Register 버튼 활성화
    public void TextInputChanged(int num)
    {
        switch (num)
        {
            case 0:
                if (EmailInput.text.Length > 0)
                    emailVerified = true;
                else
                    emailVerified = false;
                break;
            case 1:
                if (PasswordInput.text.Length > 0 && PasswordInput.text.Equals(VerifyInput.text))
                {
                    passwordVerified = true;
                    ErrorText.text = "";
                }
                else
                {
                    passwordVerified = false;
                    ErrorText.text = "비밀번호가 일치하지 않습니다.";
                }
                break;
            case 2:
                if (NameInput.text.Length > 0)
                    nameVerified = true;
                else
                    nameVerified = false;
                break;
            default:
                if (AgeInput.text.Length > 0)
                    ageVerified = true;
                else
                    ageVerified = false;
                break;
        }
    }

    // 회원가입 메소드 -> 성공 시 OnRegisterSuccess 메소드 호출
    public void Register()
    {
        // 정보를 가지고 playfab 회원가입 시도 RequireBothUsernameAndEmail = false -> 이메일만으로 가입 가능
        var request1 = new RegisterPlayFabUserRequest
        {
            Email = EmailInput.text,
            Password = PasswordInput.text,
            DisplayName = EmailInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request1, OnRegisterSuccess, ErrorHandler);
    }

    // 회원정보 저장을 위해 먼저 입력된 정보를 가지고 로그인 시도 -> 성공 시 OnLoginSuccess 메소드 호출
    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        var request2 = new LoginWithEmailAddressRequest { Email = EmailInput.text, Password = PasswordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request2, OnLoginSuccess, ErrorHandler);

    }

    // 회원정보를 저장 -> 성공 시 로그아웃 후 메인으로
    void OnLoginSuccess(LoginResult result)
    {
        User user = DBManager.CreateUser(
            EmailInput.text, PasswordInput.text, NameInput.text,
            Int32.Parse(AgeInput.text), gender, DateTime.Now
            );
        string userInfo = JsonUtility.ToJson(user);

        InventorySlot inventory = DBManager.CreateInventorySlot(0, 0, 0);
        string inventoryInfo = JsonUtility.ToJson(inventory);

        var request3 = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>() { 
                { "User", userInfo },
                { "Measurement", "{}" }, 
                { "GameResult", "{}" },
                { "Inventory", inventoryInfo }
            },       
            Permission = UserDataPermission.Public
        };

        PlayFabClientAPI.UpdateUserData(request3, (finalResult) => {
            PlayFabClientAPI.ForgetAllCredentials();    // 로그아웃
            SceneManager.LoadScene("1.Login");
        }, ErrorHandler);


    }

    // Register 관련 Error 핸들러
    void ErrorHandler(PlayFabError error)
    {
        switch (error.Error)
        {
            case PlayFabErrorCode.InvalidParams:
                foreach (string k in error.ErrorDetails.Keys)
                {
                    if (k.Equals("Email"))
                    {
                        foreach (string s in error.ErrorDetails[k])
                        {
                            if (s.Equals("Email address is not valid."))
                                ErrorText.text = "이메일의 형식이 잘못되었습니다.";
                            else if (s.Equals("Email address already exists. "))
                                ErrorText.text = "이미 가입되어 있는 이메일입니다.";
                        }
                    }
                    else if (k.Equals("Password"))
                    {
                        ErrorText.text = "비밀번호의 형식이 잘못되었습니다.(6~13자 사이)";
                    }
                    else
                    {
                        Debug.LogErrorFormat("Error: {0}, ErrorMessage: {1}", error.Error, error.ErrorMessage);
                        ErrorText.text = "예기치 못한 오류가 발생했습니다. 다시 시도해주세요.";
                    }
                }
                break;
            case PlayFabErrorCode.EmailAddressNotAvailable:
                ErrorText.text = "이미 가입되어 있는 이메일입니다.";
                break;
            default:
                Debug.LogErrorFormat("Error: {0}, ErrorMessage: {1}", error.Error, error.ErrorMessage);
                ErrorText.text = "예기치 못한 오류가 발생했습니다. 다시 시도해주세요.";
                break;
        }

    }
}