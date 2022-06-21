using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSVInput : MonoBehaviour
{
    // !!Resources에 있는 ageData 파일 위치 옮기지 마시오.

    #region 변수
    private int user_percentage = 99; // 백분위
    private Dictionary<int, List<string>> _Index = new Dictionary<int, List<string>>(); // 키 : 연령대 value : 사용할 헤드
    // 헤드 순서 : 종합, 최대치, 평균치
    private List<string> _20Head = new List<string>() { "age20", "20_Max", "20_Mean" };  // 20대 사용 CSV 헤드
    private List<string> _30Head = new List<string>() { "age30", "30_Max", "30_Mean" }; // 30대 사용 CSV 헤드
    private List<string> _65Head = new List<string>() { "age65", "65_Max", "65_Mean" }; // 65이상 사용 CSV 헤드
    List<Dictionary<string, object>> _data; // CSV 오브젝트

    private List<double> data_all = new List<double>(); // 종합 유연성
    private List<double> data_Mean = new List<double>(); // 평균치
    private List<double> data_Max = new List<double>();  // 최대치

    public Text percentageAge;
    public Slider percentageAgeSlider;

    public User user;   // User 정보
    public Measurement recentData;    // 수행동작 별 사용자 측정값

    public List<Slider> mean_Slider = new List<Slider>();
    public List<Slider> userDataSlider = new List<Slider>();    // 수행동작 별 사용자 측정값 슬라이더
    public List<Text> userDataText = new List<Text>();
    public Text nameText, ageText, genderText;
    
    public Text handle, percentage; // 사용자 백분위 표시 
    public Slider percnetageSlider;
    #endregion

    // Start is called before the first frame update

    private void Awake()
    {
        user = UserDataManager.instance.user;
        recentData = UserDataManager.instance.recentData;

        nameText.text = "이름: " + user.name;
        ageText.text = "나이: " + user.age;
        genderText.text = "성별: " + (user.gender ? "남성" : "여성");
        
        // 측정값 슬라이더 초기화 -> 나중에 순서 바꾸던가 해야될 듯
        userDataSlider[0].maxValue = mean_Slider[2].maxValue;
        userDataSlider[1].maxValue = mean_Slider[3].maxValue;
        userDataSlider[2].maxValue = mean_Slider[0].maxValue;
        userDataSlider[3].maxValue = mean_Slider[1].maxValue;
        userDataSlider[4].maxValue = mean_Slider[4].maxValue;
        userDataSlider[5].maxValue = mean_Slider[5].maxValue;

        // 수행동작 별 측정값 슬라이더에 적용
        userDataSlider[0].value = recentData.leftFlexion;
        userDataSlider[1].value = recentData.rightFlexion;
        userDataSlider[2].value = recentData.flexion;
        userDataSlider[3].value = recentData.extension;
        userDataSlider[4].value = recentData.leftRotation;
        userDataSlider[5].value = recentData.rightRotation;

        userDataText[0].text = recentData.leftFlexion.ToString("N1");
        userDataText[1].text = recentData.rightFlexion.ToString("N1");
        userDataText[2].text = recentData.flexion.ToString("N1");
        userDataText[3].text = recentData.extension.ToString("N1");
        userDataText[4].text = recentData.leftRotation.ToString("N1");
        userDataText[5].text = recentData.rightRotation.ToString("N1");

        percentageAge.text = recentData.totalFlexibility.ToString("N0");
        percentageAgeSlider.value = recentData.totalFlexibility;
        //user_age = int.Parse(User_info_change.Instance.user[1]) / 10; // user 나이
        user.age = user.age / 10;
        user.age = int.Parse(user.age.ToString() + "0"); // user 연령대
        if (user.age >= 60)
            user.age = 65;

        for (int i = 0; i < mean_Slider.Count; i++)
        {
            mean_Slider[i].interactable = false;
            userDataSlider[i].interactable = false;
        }

        #region 연령별 CSV 헤드 삽입
        _Index.Add(20, _20Head);
        _Index.Add(30, _30Head);
        _Index.Add(65, _65Head);
        #endregion

        _data = CSVReader.Read("ageData");
    }
    void Start()
    {
        ReadCSVdata();
        Mypercentage();
    }

    void ReadCSVdata()
    {
        //연령대에 맞는 종합유연성, 최대, 평균 유연성 읽기
        if (_Index.ContainsKey(user.age))
        {
            Debug.Log("유효한 데이터");

            for (int i = 0; i < _data.Count; i++)
                data_all.Add(double.Parse(_data[i][_Index[user.age][0]].ToString()));

            for (int i = 0; i < 6; i++)
            {
                data_Max.Add(double.Parse(_data[i][_Index[user.age][1]].ToString()));
                data_Mean.Add(double.Parse(_data[i][_Index[user.age][2]].ToString()));
            }

            inputData(data_Max, data_Mean);

        }
        else
            Debug.LogError("해당 연령대의 데이터가 없습니다.");
    }

    void inputData(List<double> _max, List<double> _mean) // 연령대에 맞는 데이터 최대값, 평균값 삽입
    {
        for (int i = 0; i < mean_Slider.Count; i++)
        {
            mean_Slider[i].maxValue = ((float)_max[i]);
            mean_Slider[i].value = ((float)_mean[i]);
            mean_Slider[i].transform.GetChild(2).GetComponent<Text>().text = "" + ((float)_mean[i]);
        }
    }

    void Mypercentage()
    {
        double near = 0;
        int index = 0;
        double min = Int32.MaxValue;
        for (int i = 0; i < data_all.Count; i++)
        {
            if (Abs(data_all[i] - recentData.totalFlexibility) < min)
            {
                min = Abs(data_all[i] - recentData.totalFlexibility); //최소값 알고리즘
                near = data_all[i]; //최종적으로 가까운 값
                index = i;
            }
        }
        user_percentage = int.Parse(_data[index]["percentage"].ToString());
        percnetageSlider.value = user_percentage;
        handle.text = user_percentage.ToString();
        percentage.text = user.name + "님은 상위 " + user_percentage.ToString() + "% 입니다.";
    }

    private double Abs(double v)
    {
        return (v < 0) ? -v : v;
    }
}
