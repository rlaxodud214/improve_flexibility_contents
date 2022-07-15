using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_ObstacleMove : MonoBehaviour
{
    public float speed ;
    public Vector3 StartPosition;
    
    void Start()
    {
        transform.position = StartPosition;
        //print(StartPosition);
    }

    void Update()
    {
        transform.Translate(Vector3.back * Time.deltaTime * speed);
        if(transform.position.z < -100)
        {
            gameObject.SetActive(false);
            transform.position = StartPosition;
        }
    }
}
