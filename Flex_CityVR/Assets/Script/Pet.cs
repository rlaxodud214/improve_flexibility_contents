using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pet : MonoBehaviour
{
    // 상점 ScrollView
    public GameObject content;
    public GameObject petItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        /// 동적 Item 추가///
        for (int i = 0; i < 3; i++)
        {
            Instantiate<GameObject>(this.petItemPrefab, content.transform);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
