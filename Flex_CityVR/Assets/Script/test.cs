using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public List<string> animArray = new List<string>();
    public Animation anim;
    int randNum;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(testPet());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator testPet()
    {
        AnimationArray();
        anim.Play(animArray[randNum]);
        anim.wrapMode = WrapMode.Once;
        yield return new WaitForSeconds(2f);
        StartCoroutine(testPet());
    }

    public void AnimationArray()
    {
        foreach(AnimationState state in anim)
        {
            animArray.Add(state.name);
        }
        randNum = Random.Range(0, animArray.Count);
    }
}
