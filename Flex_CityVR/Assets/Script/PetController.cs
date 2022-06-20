using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : MonoBehaviour
{
    private Transform target;
    private float speed;
    private float distance;
    public Animator anim;


    private void OnEnable()
    {
        target = Player.instance.CenterEyeAnchor.transform.GetChild(2);
        anim = transform.GetComponent<Animator>();
        anim.applyRootMotion = false;
        speed = 1f;
        distance = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Pet.instance.isCall)
        {
            //print("Vector3.Distance(target.position, transform.position) : " + Vector3.Distance(target.position, transform.position));
            if(Vector3.Distance(target.position, transform.position) >= distance)
            {
                anim.SetBool("IsRun", true);
                var temp = new Vector3(target.position.x, transform.position.y, target.position.z);
                transform.position = Vector3.Lerp(transform.position, temp, Time.deltaTime * speed);
                var backup = transform.eulerAngles;
                transform.rotation = Quaternion.LookRotation(target.position - transform.position);
                var backup1 = transform.eulerAngles;
                transform.eulerAngles = new Vector3(backup.x, backup1.y, backup1.z);
            }
            else
            {
                anim.SetBool("IsRun", false);
            }
        }
    }
}
