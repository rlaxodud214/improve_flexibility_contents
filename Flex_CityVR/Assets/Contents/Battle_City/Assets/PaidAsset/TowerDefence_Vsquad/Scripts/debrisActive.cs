using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debrisActive : MonoBehaviour
{
    public bool active = false;

    public static debrisActive instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(active == true)
        {
            gameObject.SetActive(true);
        }
    }
}
