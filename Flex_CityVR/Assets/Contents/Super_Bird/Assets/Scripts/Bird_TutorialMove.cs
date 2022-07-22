using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bird_TutorialMove : MonoBehaviour
{
/*    public Slider TutorialSlider;*/
    private float value;
    public List<float> weights = new List<float>();
    void Start()
    {
/*        TutorialSlider.value = 0;
        TutorialSlider.maxValue = 95f;
        TutorialSlider.minValue = -36.6f;*/
        weights.Add(0.5f);
        weights.Add(11);
        OpenZenMoveObject.Instance.runstart();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        value = OpenZenMoveObject.Instance.sensorEulerData.y;
        if (value > 4.4)
        {
            // transform.position = new Vector3(0, (-1.5f / TutorialSlider.maxValue) * TutorialSlider.value, 0);
            transform.position = new Vector3(0, (-1.5f / 95f / weights[0]) * value, 7);
            
        }
        else if (value < -4.4)
        {
            // transform.position = new Vector3(0, (2.7f / TutorialSlider.minValue) * TutorialSlider.value, 0);
            transform.position = new Vector3(0, (2.7f / -36.6f / weights[1]) * value, 7);
        }
    }
}
