using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_GroundMove : MonoBehaviour
{
    public float speed;
    public Vector3 StartPosition;
    public int ZPosition;
   
    void Start()
    {
        transform.position = StartPosition;
    }

    void Update()
    {
        transform.Translate(Vector3.back * Time.deltaTime * speed);
        if (transform.position.z < ZPosition)
        {
            gameObject.SetActive(false);
            transform.position = StartPosition;
        }
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }
}
