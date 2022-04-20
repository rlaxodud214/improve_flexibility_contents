using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject Bullet2;
    public GameObject Bullet_Line;
    public Transform FirePos;
    public GameObject clone;
    public float speed;
    public float speed_backup;
    public bool changeCamera = false;
    public bool fireActive;
    public LineRenderer Line_Renderer;
    public List<Vector3> LineRenderer_position;

    public static Fire instance;
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
        speed = 0;
        fireActive = true;
        // gameObject.AddComponent<LineRenderer>();
        Line_Renderer = transform.GetComponent<LineRenderer>();
        Line_Renderer.SetWidth(0.1f, 0.1f);
        // Line_Renderer.SetColors()
    }

    // Update is called once per frame
    void Update()
    {   
        if(fireActive == true)
        {
            if(Input.GetKey("space"))
            {   //누를수록 멀리 나가도록
                speed += Time.deltaTime*3f;
            }

            if(Input.GetKeyUp("space"))
            {   //포탄 복제
                clone = Instantiate(Bullet2, FirePos.transform.position, FirePos.transform.rotation);
                SoundManager.instance.FireSound();
                changeCamera = true;
                fireActive = false;
            }
        }
        // if(speed_backup != speed)
        //     LineRenderer_Setup();

        speed_backup = speed;

    }

    // public void LineRenderer_Setup() {
    //     //포탄 복제
    //     clone = Instantiate(Bullet_Line, FirePos.transform.position, FirePos.transform.rotation);
    //     SoundManager.instance.FireSound();
    //     changeCamera = true;
    //     fireActive = false;
    // }
    
    // 포탄 발사하고 라인렌더러 그려줌
    public void removeBullet()
    {
        var LineRenderer_position_remove = new List<Vector3>();
        for(int i=0; i< LineRenderer_position.Count; i+=10) {
            LineRenderer_position_remove.Add(LineRenderer_position[i]);
            // print(LineRenderer_position[i]);
        }
        Line_Renderer.positionCount = LineRenderer_position_remove.Count;
        Line_Renderer.SetPositions(LineRenderer_position_remove.ToArray());
        LineRenderer_position.Clear();
        LineRenderer_position_remove.Clear();
        
        Destroy(clone, 6f);
    }

}
