using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chef_TutorialPickUp : MonoBehaviour
{
    public GameObject slotItem;
    public Chef_TutorialInventory inven;
    public bool isDelay;

    private void Start()
    {
        isDelay = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            Chef_TutorialUIManager._Instance.activeinteraction();
            Chef_TutorialUIManager._Instance.activeinteractionExplainOn();
            Invoke("Chef_TutorialUIManager._Instance.activeinteractionExplainOff", 3f);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player")
        {
            Chef_TutorialUIManager._Instance.inactiveinteraction();
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "Player" && Input.GetKey(KeyCode.Space))
        {
            if (!isDelay)
            {
                print("Collision: "+ collision);
                inven = collision.transform.GetComponent<Chef_TutorialInventory>();
                print("for문 전");
                print(inven.slots.Count);
                for (int i = 0; i < inven.slots.Count; i++)
                {
                    if (inven.slots[i].isEmpty)
                    {
                        print("for문 안");
                        isDelay = true;
                        Instantiate(slotItem, inven.slots[i].slotObj.transform, false);
                        //inven.slots[i].foodname = inven.GetComponent<Image>().sprite.name;
                        inven.slots[i].isEmpty = false;
                        StartCoroutine(pickUpDelay());
                        break;
                    }
                }
            }
        }

        //inven.slots[i].slotObj.transform.GetChild(0).gameObject.GetComponent<Image>().sprite.name == ""
        IEnumerator pickUpDelay()
        {
            yield return new WaitForSeconds(2f);
            isDelay = false;
        }
    }
}
