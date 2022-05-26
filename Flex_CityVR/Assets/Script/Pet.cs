using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    public List<GameObject> UserPet = new List<GameObject>();

    public static Pet instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetPet(GameObject pet)
    {
        if (!UserPet.Contains(pet))
        {
            UserPet.Add(pet);
        }
    }
}
