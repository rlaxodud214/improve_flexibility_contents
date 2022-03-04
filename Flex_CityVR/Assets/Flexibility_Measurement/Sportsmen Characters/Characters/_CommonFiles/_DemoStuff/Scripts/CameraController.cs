using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform[] view;
    public float transitionSpeed;
    Transform currentView;

    private void Start()
    {
        currentView = view[0];
    }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeed);
    }
    public void ZoomIn()
    {
        currentView = view[1];
        
    }
    public void ZoomOut()
    {
        currentView = view[0];
    }
}
