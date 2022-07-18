using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bird_TutorialMove : MonoBehaviour
{
    public Slider TutorialSlider;
    void Start()
    {
        TutorialSlider.value = 0;
        TutorialSlider.maxValue = 95f;
        TutorialSlider.minValue = -36.6f;
        OpenZenMoveObject.Instance.runstart();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TutorialSlider.value > 0)
        {
            //transform.position = new Vector3(0, (-1.5f / TutorialSlider.maxValue) * TutorialSlider.value, 0);
            transform.position = new Vector3(0, (-1.5f / 95f) * OpenZenMoveObject.Instance.sensorEulerData.y * -1.3f, 0);
        }
        else if (TutorialSlider.value < 0)
        {
            //transform.position = new Vector3(0, (2.7f / TutorialSlider.minValue) * TutorialSlider.value, 0);
            transform.position = new Vector3(0, (2.7f / -36.6f) * OpenZenMoveObject.Instance.sensorEulerData.y * -1.3f, 0);
        }
    }
}
