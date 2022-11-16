using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensorTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        OpenZenMoveObject.Instance.OpenZenIdentifier = "00:04:3E:9B:A2:A5";
        OpenZenMoveObject.Instance.runstart();
    }

    private void Update()
    {
        Debug.LogErrorFormat("X: {0}   |   Y: {1}   |   Z: {2}"
            , OpenZenMoveObject.Instance.sensorEulerData.x
            , OpenZenMoveObject.Instance.sensorEulerData.y
            , OpenZenMoveObject.Instance.sensorEulerData.z);
        
    }
}
