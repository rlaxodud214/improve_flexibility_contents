using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FoodData
{
    public Sprite FoodObj;   // 음식이름
    public string[] ingredients; // 재료 문자열
}

public class Chef_ReturnFood : MonoBehaviour
{
    public GameObject slotItem;
    public Chef_Inventory inven;
    public Sprite Meat;
    public Sprite LemonMeat;
    public Sprite HotDog;
    public Sprite Sandwich;
    public Sprite Stake;

    #region Singleton 
    private static Chef_ReturnFood instance = null;
    public static Chef_ReturnFood _Instance                // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
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
            food.Add(new FoodData() { FoodObj = Meat, ingredients = new string[] { "고기" } });
            food.Add(new FoodData() { FoodObj = LemonMeat, ingredients = new string[] { "고기", "레몬" } });
            food.Add(new FoodData() { FoodObj = HotDog, ingredients = new string[] { "빵", "소세지" } });
            food.Add(new FoodData() { FoodObj = Sandwich, ingredients = new string[] { "빵", "치즈", "오이" } });
            food.Add(new FoodData() { FoodObj = Stake, ingredients = new string[] { "고기", "브로콜리", "토마토", "버섯" } });
            isDelay = false;
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

                var len = 0;
                for (int i = 0; i < Chef_ReturnFood._Instance.food[Chef_StageManagement._Instance.stageNum].ingredients.Length; i++)
                {
                    var c = inven.slots[i].slotObj.transform.childCount; // slot에 있는 item이 0이 아니면 len 을 증가시키게 함
                    if (c != 0)
                        len++;
                    else
                        continue;
                }

                for (int i = 0; i < food[Chef_StageManagement._Instance.stageNum].ingredients.Length; i++)  // 음식에 필요한 재료만큼 for 문을 반복하게함 
                {
                    for (int j = 0; j < len; j++)
                    {
                        print(inven.slots[j].slotObj.transform.GetChild(0).gameObject.GetComponent<Image>().sprite.name);
                        if (inven.slots[j].slotObj.transform.GetChild(0).gameObject.GetComponent<Image>().sprite.name == food[Chef_StageManagement._Instance.stageNum].ingredients[i])
                        {
                            materialnum++;
                            print(materialnum);
                            break;
                        }
                    }
                }

                if (materialnum == food[Chef_StageManagement._Instance.stageNum].ingredients.Length)
                {
                    Chef_UIManager._Instance.inactivereturn();
                    Chef_UIManager._Instance.GameSuccess();
                    StartCoroutine(returnDelay());
                }
                else if (TryCount > 0)
                {
                    Chef_StageManagement._Instance.tryNum++;
                    TryCount--;
                    Chef_Inventory._Instance.emptyslot();
                    Chef_Inventory._Instance.discountTry();
                    materialnum = 0;
                    StartCoroutine(returnDelay());
                }
                else
                {
                    Chef_UIManager._Instance.inactivereturn();
                    Chef_Inventory._Instance.emptyslot();
                    Chef_Inventory._Instance.discountTry();
                    Chef_UIManager._Instance.GameFail();
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
}




