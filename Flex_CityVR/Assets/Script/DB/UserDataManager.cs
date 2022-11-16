using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager instance;          // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static 선언으로 어디서든 참조가 가능함

    public User user;
    public string openZenIdentifier;
    public Dictionary<string, GameObject> myPets = new Dictionary<string, GameObject>();    // 사용자가 보유한 펫
    public Dictionary<string, Measurement> measurements;
    public Dictionary<string, GameResult> gameResults;
    public InventorySlot inventory;
    public Measurement recentData;
    public List<List<Measurement>> GraphData;
    public List<Measurement> temp = new List<Measurement>(); // [n주 데이터]
    public List<List<Measurement>> temps = new List<List<Measurement>>(); // [[temp], [temp], [temp], [temp], [temp], [temp]] = [[1주 데이터], [2주 데이터], [3주 데이터], [4주 데이터], [5주 데이터], [6주 데이터]]

    // 데이터 들어갈 변수들 string 타입으로 3/15, 40.0 형식으로 넣으면 아래서 Split으로 둘을 분리하여
    // groups과 data라는 List에 각각 값을 저장하고 이값을 토대로 그래프를 만들어줌
    public List<string> series1Data1, series1Data2, series1Data3, series1Data4;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
            Destroy(this);
    }

    void Start()
    {
        series1Data1 = new List<string>();
        series1Data2 = new List<string>();
        series1Data3 = new List<string>();
        series1Data4 = new List<string>();
        StartCoroutine(InitData());      
    }

    // 딕셔너리에서 가장 최근 데이터를 찾는 메소드(날짜기준)
    public Measurement FindRecentData(Dictionary<string, Measurement> measurements)
    {
        DateTime recentDateKey = DateTime.MinValue; // 최솟값으로 초기화
        DateTime compareDateKey = DateTime.MinValue; // 최솟값으로 초기화

        // 여러 key 값중 Max값 찾는 알고리즘 느낌으로 구현
        foreach (string key in measurements.Keys) // 키 값들중 하나씩 빼서 쓰기
        {
            if (recentDateKey.Equals(DateTime.MinValue)) // 제일 처음 한 번만 작동함
            {
                // DateTime.ParseExact(변환할 string 값, format, provider)
                // "yyyy-MM-dd HH:mm:ss" 타입의 string 객체를 DateTime 객체로 형변환
                recentDateKey = DateTime.ParseExact(key, "yyyy-MM-dd HH:mm:ss", null);
                continue;
            }
            // DateTime.ParseExact(변환할 string 값, format, provider)
            // "yyyy-MM-dd HH:mm:ss" 타입의 string 객체를 DateTime 객체로 형변환
            compareDateKey = DateTime.ParseExact(key, "yyyy-MM-dd HH:mm:ss", null);

            // 이전 recent값보다 compare하고자 하는 값이 더 최근이라면
            if (DateTime.Compare(recentDateKey, compareDateKey) < 0)
            {
                recentDateKey = compareDateKey;
            }
        }

        string result = recentDateKey.ToString("yyyy-MM-dd HH:mm:ss");
        // Debug.Log(result);

        return measurements[result];
    }

    // 그래프 데이터 셋
    public List<List<Measurement>> FindGraphData(Dictionary<string, Measurement> measurements)
    {
        DateTime recentDateKey = DateTime.MinValue; // 최솟값으로 초기화
        DateTime compareDateKey = DateTime.MinValue; // 최솟값으로 초기화
        DateTime nowDate = DateTime.MinValue; // 최솟값으로 초기화

        temps.Clear();
        temp.Clear();

        var prekey = "";
        DateTime PrecalculationDate = DateTime.MinValue;
        DateTime PrecalculationDate1 = new DateTime(PrecalculationDate.Year, PrecalculationDate.Month, 1); // 기준일
        // 여러 key 값중 Max값 찾는 알고리즘 느낌으로 구현
        var c = 0;
        foreach (string key in measurements.Keys) // 키 값들중 하나씩 빼서 쓰기
        {
            DateTime calculationDate = DateTime.ParseExact(key, "yyyy-MM-dd HH:mm:ss", null); // 주차를 구할 일자
            DateTime calculationDate1 = new DateTime(calculationDate.Year, calculationDate.Month, 1); // 기준일
            if(c != 0)
            {
                PrecalculationDate = DateTime.ParseExact(prekey, "yyyy-MM-dd HH:mm:ss", null); // 주차를 구할 일자
                PrecalculationDate1 = new DateTime(PrecalculationDate.Year, PrecalculationDate.Month, 1); // 기준일
            }
            
            // DateTime.ParseExact(변환할 string 값, format, provider)
            // "yyyy-MM-dd HH:mm:ss" 타입의 string 객체를 DateTime 객체로 형변환
            compareDateKey = DateTime.ParseExact(key, "yyyy-MM-dd HH:mm:ss", null);
            Debug.Log("prekey : " + prekey + ", " + "Key : " + key + ", (int)compareDateKey.DayOfWeek : " + (int)compareDateKey.DayOfWeek);
            Calendar calenderCalc = CultureInfo.CurrentCulture.Calendar;
            // DayOfWeek.Sunday 인수는 기준 요일
            int usWeekNumber = calenderCalc.GetWeekOfYear(PrecalculationDate1, CalendarWeekRule.FirstDay, DayOfWeek.Sunday) -
                               calenderCalc.GetWeekOfYear(calculationDate, CalendarWeekRule.FirstDay, DayOfWeek.Sunday) + 1;

            prekey = key;
            print("usWeekNumber : " + usWeekNumber);
            // 09/01 : 9월 데이터 하나 이후에 7월 데이터 들어오면 오류나서 추가함 -> 저장할 조건
            // temp 개수가 1개 이상이고, 2주 이상 차이가 날 때
            if (c != 0 && temp.Count >= 1 && usWeekNumber > 1)
            {
                Debug.Log("--------------------------------------------------------------------");
                foreach (Measurement m in temp)
                    Debug.Log("1. 데이터 : " + m.date + ", " + m.flexion + ", " + m.extension + ", " + m.leftFlexion + ", " + m.rightFlexion + ", " + m.leftRotation + ", " + m.rightRotation + ", " + m.totalFlexibility + ", " + (int)compareDateKey.DayOfWeek);
                Debug.Log("--------------------------------------------------------------------");

                temps.Add(temp);
                temp = new List<Measurement>(); // [n주 데이터]
            }

            string result = compareDateKey.ToString("yyyy-MM-dd HH:mm:ss");
            temp.Add(measurements[result]);

            // compareDateKey.DayOfWeek : 일요일이 0, 월요일이 1, 토요일이 6
            if ((int)compareDateKey.DayOfWeek == 1)
            {
                Debug.Log("--------------------------------------------------------------------");
                foreach (Measurement m in temp)
                    Debug.Log("2. 데이터 : " + m.date + ", " + m.flexion + ", " + m.extension + ", " + m.leftFlexion + ", " + m.rightFlexion + ", " + m.leftRotation + ", " + m.rightRotation + ", " + m.totalFlexibility + ", " + (int)compareDateKey.DayOfWeek);
                Debug.Log("--------------------------------------------------------------------");

                temps.Add(temp);
                temp = new List<Measurement>(); // [n주 데이터]
            }
            c++;
            if(temps.Count >= 6)
                return temps;
        }
        return temps;
    }


    public IEnumerator InitData()
    {
        GC.Collect();

        // 초기화
        Debug.Log("InitData() 호출댐");
        series1Data1.Clear();
        series1Data2.Clear();
        series1Data3.Clear();
        series1Data4.Clear();
        myPets.Clear();

        yield return StartCoroutine(DBManager.LoadUser((result) => user = result)); // User 객체 로드
        yield return StartCoroutine(DBManager.LoadMeasurement((result) => measurements = result));  // Dictionary<string, Measurement> 딕셔너리 로드(측정결과)
        yield return StartCoroutine(DBManager.LoadGameResult((result) => gameResults = result));    // Dictionary<string, GameResult> 딕셔너리 로드(게임결과)
        yield return StartCoroutine(DBManager.LoadInventory((result) => inventory = result));   // InventorySlot 객체 로드

        // 09/01 : myPets이 2번 셋되어 수정함 -> 초기화 코드 추가로 if문 뺌
        foreach (string pet in user.pets)
        {
            switch (pet)
            {
                case "검은 고양이":
                    myPets.Add("검은 고양이", PetPrefabs.instance.petPrefab[0]);
                    break;
                case "메인쿤":
                    myPets.Add("메인쿤", PetPrefabs.instance.petPrefab[1]);
                    break;
                case "회색 고양이":
                    myPets.Add("회색 고양이", PetPrefabs.instance.petPrefab[2]);
                    break;
                case "주황 고양이":
                    myPets.Add("주황 고양이", PetPrefabs.instance.petPrefab[3]);
                    break;
                case "페르시안":
                    myPets.Add("페르시안", PetPrefabs.instance.petPrefab[4]);
                    break;
                case "러시안블루":
                    myPets.Add("러시안블루", PetPrefabs.instance.petPrefab[5]);
                    break;
                case "샴":
                    myPets.Add("샴", PetPrefabs.instance.petPrefab[6]);
                    break;
                case "호랑무늬 고양이":
                    myPets.Add("호랑무늬 고양이", PetPrefabs.instance.petPrefab[7]);
                    break;
                case "턱시도 고양이":
                    myPets.Add("턱시도 고양이", PetPrefabs.instance.petPrefab[8]);
                    break;

            }
        }

        recentData = FindRecentData(measurements);
        GraphData = FindGraphData(measurements);
        var dayofweek = (int)DateTime.Now.DayOfWeek;
        var week = 0; // 6주 -> 0~5의 값을 가짐
        var day = 0; // 7일 -> 0~6의 값을 가짐
        List<int> data_sum = new List<int>() { 0, 0, 0, 0, 0, 0, 0 };
        var count = 0;
        var measurements_graph = new List<Measurement>();
        var temp_Date_minus = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", null);
        Debug.Log("=====================================================================");
        Debug.Log("GraphData.Count : " + GraphData.Count);
        Debug.Log("GraphData[0].Count : " + GraphData[0].Count);
        Debug.Log("GraphData[1].Count : " + GraphData[1].Count);
        Debug.Log("GraphData[2].Count : " + GraphData[2].Count);
        Debug.Log("GraphData[3].Count : " + GraphData[3].Count);
        Debug.Log("GraphData[4].Count : " + GraphData[4].Count);
        Debug.Log("GraphData[5].Count : " + GraphData[5].Count);
        Debug.Log("=====================================================================");

        var temp = new Measurement();
        var temp_backup = new DateTime();
        var temp_Date = new DateTime();

        // 일요일이 0, 월요일이 1, 토요일이 6
        for (int i=0; i <40 + dayofweek; i++)
        {
            //print("i : " + i + ", index : " + week + ", index1 : " + day);
            temp_backup = temp_Date;
            if (week == 0)
                temp = GraphData[week][day % GraphData[week].Count];
            else
                temp = GraphData[week][day % 7];

            // Debug.Log("temp.date : " + temp.date);
            temp_Date = DateTime.ParseExact(temp.date, "yyyy-MM-dd HH:mm:ss", null);

            /*print("temp_Date_minus.Date : " + temp_Date_minus.Date);
            print("temp_Date.Date : " + temp_Date.Date);*/

            // 만약 DB 데이터가 현재 날짜를 초과할 경우 or DB 데이터에서 같은 날이 있을 경우 제일 처음(그 날짜 제일 마지막) 데이터 사용
            if (DateTime.Compare(temp_Date_minus.Date, temp_Date.Date) > 0) //  || DateTime.Compare(temp_backup.Date, temp_Date.Date) < 0
            {
                //Debug.Log("먼가 이상해");
                i--;
                temp_Date_minus = temp_Date_minus.AddDays(-1);
                continue;
            }

            // 값 더하기 -> 일요일되면 평균내고 measurements_graph에 저장하고 리셋
            if (DateTime.Compare(temp_Date_minus.Date, temp_Date.Date) == 0)
            {
                // Debug.Log("날짜 같음 추가!" + temp_Date_minus.ToString("yyyy-MM-dd"));
                data_sum[0] += temp.flexion;
                data_sum[1] += temp.extension;
                data_sum[2] += temp.leftFlexion;
                data_sum[3] += temp.rightFlexion;
                data_sum[4] += temp.leftRotation;
                data_sum[5] += temp.rightRotation;
                data_sum[6] += temp.totalFlexibility;
                day++;
                count++;
            }

            // 월요일이면
            if ((int)temp_Date_minus.DayOfWeek == 1 && count != 0)
            {
                // data_sum 값 대입
                // Debug.Log("월요일임" + temp_Date_minus.ToString("yyyy-MM-dd"));
                var measurements_backup = new Measurement();
                measurements_backup.date = temp.date;
                measurements_backup.flexion = data_sum[0] / count;
                measurements_backup.extension = data_sum[1] / count;
                measurements_backup.leftFlexion = data_sum[2] / count;
                measurements_backup.rightFlexion = data_sum[3] / count;
                measurements_backup.leftRotation = data_sum[4] / count;
                measurements_backup.rightRotation = data_sum[5] / count;
                measurements_backup.totalFlexibility = data_sum[6] / count;
                measurements_graph.Add(measurements_backup);
                if (measurements_graph.Count >= 6)
                    break;

                // data_sum 초기화
                data_sum = new List<int>() { 0, 0, 0, 0, 0, 0, 0 };

                // 주 변경
                count = 0; // 개수 초기화
                day = 0; // 일 초기화
                week++;
            }
            else if (count == 0)
            {
                // print("i : " + i + " 0으로 나눌 수 없습니다.");
                if (GraphData[week].Count-1 == day)
                {
                    week++;
                    day = 0;
                }
                else
                    day++;
                    
            }
            temp_Date_minus = temp_Date_minus.AddDays(-1);
            
        }
        count = 1;
        if(series1Data1.Count == 0)
        foreach (Measurement m in measurements_graph)
        {
            // 출처: https://akasi.tistory.com/130 [AKASI-STORY:티스토리]
            DateTime calculationDate = DateTime.ParseExact(m.date, "yyyy-MM-dd HH:mm:ss", null); // 주차를 구할 일자
            DateTime calculationDate1 = new DateTime(calculationDate.Year, calculationDate.Month, 1); // 기준일
            Calendar calenderCalc = CultureInfo.CurrentCulture.Calendar;

            // DayOfWeek.Sunday 인수는 기준 요일
            int usWeekNumber = calenderCalc.GetWeekOfYear(calculationDate, CalendarWeekRule.FirstDay, DayOfWeek.Sunday) -
                               calenderCalc.GetWeekOfYear(calculationDate1, CalendarWeekRule.FirstDay, DayOfWeek.Sunday) + 1;


            //Debug.Log("usWeekNumber : " + usWeekNumber);
            //Debug.Log("calculationDate.Month : " + calculationDate.Month);
            //Debug.Log("최근" + (count++) + "주 데이터 : " + m.flexion + ", " + m.extension + ", " + m.leftFlexion + ", " + m.rightFlexion + ", " + m.leftRotation + ", " + m.rightRotation + ", " + m.totalFlexibility);
            /*if(usWeekNumber == 5) // 5주차 인 경우 -> 다음달 1주차로 바꾸는 코드
            {
                series1Data1.Add((calculationDate.Month + 1) + "월 " + 1 + "째주," + (m.totalFlexibility));
                series1Data2.Add((calculationDate.Month + 1) + "월 " + 1 + "째주," + (m.flexion + m.extension));
                series1Data3.Add((calculationDate.Month + 1) + "월 " + 1 + "째주," + (m.leftFlexion + m.rightFlexion));
                series1Data4.Add((calculationDate.Month + 1) + "월 " + 1 + "째주," + (m.leftRotation + m.rightRotation));
            }
            else {
                
            }*/
            series1Data1.Add(calculationDate.Month + "월 " + usWeekNumber + "주차," + (m.totalFlexibility));
            series1Data2.Add(calculationDate.Month + "월 " + usWeekNumber + "주차," + (m.flexion + m.extension));
            series1Data3.Add(calculationDate.Month + "월 " + usWeekNumber + "주차," + (m.leftFlexion + m.rightFlexion));
            series1Data4.Add(calculationDate.Month + "월 " + usWeekNumber + "주차," + (m.leftRotation + m.rightRotation));
        }

        for(int i=0; i< series1Data1.Count; i++)
        {
            print("날짜, 종합 유연성 : " + series1Data1[i]);
        }
        Debug.Log("InitData() 실행끝");
    }
}
