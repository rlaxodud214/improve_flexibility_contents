using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smoothFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 15.4f;
    public float height = 2f;
    private float rotationDamping = 3.0f;
    private float heightDamping = 2.0f;

    void LateUpdate()
    {
        if (!target)
            return;

        var wantedRotationAngle = target.eulerAngles.y;
        var wantedHeight = target.position.y + height;
        var currentRotationAngle = transform.eulerAngles.y;
        var currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
        transform.LookAt(target);
    }
}
