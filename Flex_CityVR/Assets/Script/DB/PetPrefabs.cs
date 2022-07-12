using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetPrefabs : MonoBehaviour
{
    public static PetPrefabs instance;
    public List<GameObject> petPrefab = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}
