using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Chef_TutorialReturnFood : MonoBehaviour
{
    public GameObject slotItem;
    public Chef_Inventory inven;

    #region Singleton 
    private static Chef_TutorialReturnFood instance = null;
    public static Chef_TutorialReturnFood _Instance                // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
    {
        get { return instance; }                     // Chef_SoundManager 객체 리턴

    }
    private void Awake()  //싱글톤 생성
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(instance);
            /*SceneManager.sceneLoaded += OnSceneLoaded;*/
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
    }
    #endregion
    public List<FoodData> food = new List<FoodData>();



    public int materialnum = 0;
    public int TryCount = 2;
    public bool isDelay;
    // Start is called before the first frame update
    void Start()
    {
        isDelay = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            Chef_UIManager._Instance.activereturn();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player")
        {
            Chef_UIManager._Instance.inactivereturn();
        }
    }


    private void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "Player" && Input.GetKey(KeyCode.Space))
        {
            if (!isDelay)
            {
                inven = collision.GetComponent<Chef_Inventory>();
                if (inven.slots[0].slotObj.transform.GetChild(0).gameObject.GetComponent<Image>().sprite.name == "고기")
                {
                    Chef_UIManager._Instance.inactivereturn();
                    Chef_UIManager._Instance.GameSuccess();
                    StartCoroutine(returnDelay());
                }
                else
                {
                    Chef_UIManager._Instance.inactivereturn();
                    Chef_Inventory._Instance.emptyslot();
                    Chef_UIManager._Instance.GameFail();
                }
            }
            
                
        }
            IEnumerator returnDelay()
            {
                isDelay = true;
                yield return new WaitForSeconds(2f);
                isDelay = false;
            }
    }

}





