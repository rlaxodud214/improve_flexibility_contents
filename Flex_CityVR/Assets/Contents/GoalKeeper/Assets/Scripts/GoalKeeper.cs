using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class GoalKeeper : MonoBehaviour
{
    const int LEFT = 0;
    const int RIGHT = 1;
    const float SPEED = 0.4f;


    #region Private Fields

    Animator player;

    int playerMax;
    float imuVal;

    #endregion

    #region MonoBehaviour Callbacks

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Animator>();

        playerMax = 35;
        imuVal = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("IMU Z 데이터 : " + OpenZenMoveObject.Instance.sensorEulerData.z.ToString("N0"));
        //Debug.Log("정규화 데이터 : " + Normalize(0, PLAYER_MAX, OpenZenMoveObject.Instance.sensorEulerData.z));
        imuVal = Normalize(0, playerMax, OpenZenMoveObject.Instance.sensorEulerData.z);

        // 이동방식: 누적
        if (OpenZenMoveObject.Instance.sensorEulerData.z < -10)
        {
            player.SetBool("isMove", true);
            player.SetInteger("dir", LEFT);
            player.transform.Translate(Vector3.left * SPEED * imuVal);
        }
        else if (OpenZenMoveObject.Instance.sensorEulerData.z > 10)
        {
            player.SetBool("isMove", true);
            player.SetInteger("dir", RIGHT);
            player.transform.Translate(Vector3.right * SPEED * imuVal);
        }
        else
        {
            player.SetBool("isMove", false);
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
