using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class GoalKeeper : MonoBehaviour
{
    const int LEFT = 0;
    const int RIGHT = 1;

    #region Public Fields

    public float speed = 0f;

    #endregion

    #region Private Fields

    float imuVal;
    Measurement playerRange;

    #endregion

    #region MonoBehaviour Callbacks

    // Start is called before the first frame update
    void Start()
    {
        OpenZenMoveObject.Instance.runstart();
        playerRange = UserDataManager.instance.recentData;
        imuVal = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log("IMU Z 데이터 : " + OpenZenMoveObject.Instance.sensorEulerData.z.ToString("N0"));
        //Debug.Log("정규화 데이터 : " + Normalize(0, PLAYER_MAX, OpenZenMoveObject.Instance.sensorEulerData.z));
        

        // 이동방식: 누적
        if (OpenZenMoveObject.Instance.sensorEulerData.z < -5)
        {
            imuVal = Normalize(0, (float)playerRange.leftFlexion * (4f / 5f), OpenZenMoveObject.Instance.sensorEulerData.z);    // 사용자 최대 가동범위의 80%
            //print("보정 값 : " + (float)playerRange.leftFlexion * (4f / 5f));
            transform.Translate(Vector3.left * speed * imuVal);
        }
        else if (OpenZenMoveObject.Instance.sensorEulerData.z > 5)
        {
            imuVal = Normalize(0, (float)playerRange.rightFlexion * (4f / 5f), OpenZenMoveObject.Instance.sensorEulerData.z);
            //print("보정 값 : " + (float)playerRange.rightFlexion * (4f / 5f));
            transform.Translate(Vector3.right * speed * imuVal);
        }
    }

    #endregion

    #region Public Methods

    public float Normalize(float min, float max, float data)
    {
        // 정규화 값이 1보다 작으면 해당 값을, 1보다 크면 1f를 return
        return Mathf.Abs((data - min) / (max - min)) < 1f ? Mathf.Abs((data - min) / (max - min)) : 1f;
    }

    #endregion
}
