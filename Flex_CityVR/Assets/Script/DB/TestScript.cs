using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // 코드 사용 예시
    void Test()
    {
        Measurement measurement = DBManager.CreateMeasurementData(50, 50, 50, 50, 50, 50);
        StartCoroutine(DBManager.SaveMeasurement(measurement));

        // Measurement 불러오기 예시
        // STEP1 측정결과들을 담을 Dictionary<string, Measurement> 생성(빈 딕셔너리)
        Dictionary<string, Measurement> measurements = new Dictionary<string, Measurement>();

        // STEP2 LoadMeasurement의 매개변수로 콜백함수(람다함수) 넣음 -> measurements에 리턴되는 값 result 넣음
        // StartCoroutine(DBManager.LoadMeasurement((result) => { measurements = result; }));



        // GameResult 저장 예시
        // STEP0 플레이타임 변환(초단위의 시간 -> format이 (HH:mm:ss)인 string)
        int sec = 341;
        string playtime = DBManager.TimeToString(sec);

        // STEP1 게임결과를 담은 GameResult 객체 생성
        GameResult gameResult = DBManager.CreateGameResult("battle_city", playtime, 1080, 390);

        // STEP2 SaveGameResult의 매개변수로 gameResult 전달
        StartCoroutine(DBManager.SaveGameResult(gameResult));

        // GameResult 불러오기는 Measurement 불러오기와 동일하게 수행
    }
}
