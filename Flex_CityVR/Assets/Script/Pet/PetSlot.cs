using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetSlot : MonoBehaviour
{
    [HideInInspector]
    public bool isUse;
    public PetInfo petInfo;

    public void SetSlot(PetInfo info)
    {
        gameObject.GetComponent<UnityEngine.UI.Button>().enabled = true;
        isUse = true;
        petInfo = info;
        gameObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "" + petInfo.Name;
    }

    public void ResetSlot()
    {
        gameObject.GetComponent<UnityEngine.UI.Button>().enabled = false;
        gameObject.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Pet.instance.SlotClick);
        gameObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "";
        isUse = false;
        petInfo = null;
    }
}
