using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bird_TutorialMove : MonoBehaviour
{
    public Slider MoveSlider;
    void Start()
    {
        MoveSlider = GameObject.FindGameObjectWithTag("slider").GetComponent<Slider>();
        MoveSlider.value = 0;
        MoveSlider.maxValue = 95f;
        MoveSlider.minValue = -36.6f;
    }

    // Update is called once per frame
    void Update()
    {
        if (MoveSlider.value > 0)
        {
            transform.position = new Vector3(0, (-1.5f / MoveSlider.maxValue) * MoveSlider.value, 0);
        }
        else if (MoveSlider.value < 0)
        {
            transform.position = new Vector3(0, (2.7f / MoveSlider.minValue) * MoveSlider.value, 0);
        }
    }
}
