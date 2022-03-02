using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSVInput : MonoBehaviour
{
    // !!Resources에 있는 ageData 파일 위치 옮기지 마시오.
    #region 변수
    private int user_age = 35; //임시 나이 삽입
    private string user_name = "김태영"; //임시 이름 삽입
    public double user_flex; // 임시 유연성
    private int user_percentage = 99; // 백분위
    private Dictionary<int, List<string>> _Index = new Dictionary<int, List<string>>(); // 키 : 연령대 value : 사용할 헤드
    // 헤드 순서 : 종합, 최대치, 평균치
    private List<string> _20Head = new List<string>() { "age20", "20_Max", "20_Mean"};  // 20대 사용 CSV 헤드
    private List<string> _30Head = new List<string>() { "age30", "30_Max", "30_Mean" }; // 30대 사용 CSV 헤드
    private List<string> _65Head = new List<string>() { "age65", "65_Max", "65_Mean" }; // 65이상 사용 CSV 헤드

    List<Dictionary<string, object>> _data; // CSV 오브젝트
    private List<double> data_all = new List<double>(); // 종합 유연성
    private List<double> data_Mean = new List<double>(); // 평균치
    private List<double> data_Max = new List<double>();  // 최대치
    public List<Slider> mean_Slider = new List<Slider>();
    public Text temp_ageText; //임시
    public Text handle, percentage; // 사용자 백분위 표시 
    public Slider percnetageSlider;
    private int pre_index;
    #endregion

    // Start is called before the first frame update

    private void Awake()
    {
        temp_ageText.text = "나이 : " + user_age; //임시
        user_flex = 285; // 임시
        //user_age = int.Parse(User_info_change.Instance.user[1]) / 10; // user 나이
        user_age = user_age / 10;
        user_age = int.Parse(user_age.ToString() + "0"); // user 연령대
        if (user_age >= 60)
            user_age = 65;

        #region 연령별 CSV 헤드 삽입
        _Index.Add(20, _20Head);
        _Index.Add(30, _30Head);
        _Index.Add(65, _65Head);
        #endregion

        _data = CSVReader.Read("ageData");
    }
    void Start()
    {
        if (_Index.ContainsKey(user_age))
        {
            Debug.Log("유효한 데이터");

            for (int i = 0; i < _data.Count; i++)
                data_all.Add(double.Parse(_data[i][_Index[user_age][0]].ToString()));

            for (int i = 0; i < 6; i++)
            {
                data_Max.Add(double.Parse(_data[i][_Index[user_age][1]].ToString()));
                data_Mean.Add(double.Parse(_data[i][_Index[user_age][2]].ToString()));
            }

            inputData(data_Max, data_Mean);

        }
        else
            Debug.LogError("해당 연령대의 데이터가 없습니다.");

        Mypercentage();
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
            if (Abs(data_all[i] - user_flex) < min)
            {
                min = Abs(data_all[i] - user_flex); //최소값 알고리즘
                near = data_all[i]; //최종적으로 가까운 값
                index = i;
            }
        }
        user_percentage = int.Parse(_data[index]["percentage"].ToString());
        percnetageSlider.value = user_percentage;
        handle.text = user_percentage.ToString();
        percentage.text = user_name + "님은 상위 " + user_percentage.ToString() + "% 입니다.";
    }

    private double Abs(double v)
    {
        return (v < 0) ? -v : v;
    }
}
