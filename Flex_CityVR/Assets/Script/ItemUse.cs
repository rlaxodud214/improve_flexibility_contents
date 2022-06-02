using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class ItemUse : MonoBehaviour
{
    [HideInInspector]
    public Slot slot;
    [HideInInspector]
    public ItemInfo item;

    public List<GameObject> petPrefab = new List<GameObject>();

    // 일반 펫 상자 확률
    public List<float> normalPercentage = new List<float>();
    // 프리미엄 펫 상자 확률
    public List<float> premiumPercentage = new List<float>();
    public Transform PetSpawnPoint;

    public GameObject PetCard;
    public UnityEngine.UI.Text petName;
    public UnityEngine.UI.Text petRank;

    [HideInInspector]
    public GameObject choice;


    public Rito.WeightedRandomPicker<GameObject> normalPicker = new Rito.WeightedRandomPicker<GameObject>();
    public Rito.WeightedRandomPicker<GameObject> premiumPicker = new Rito.WeightedRandomPicker<GameObject>();

    public static ItemUse instance;

    private void Awake()
    {
        instance = this;
        SetPercentage(normalPercentage, 1.8f);
        SetPercentage(premiumPercentage, 7f);
        SettingGacha(normalPicker, normalPercentage);
        SettingGacha(premiumPicker, premiumPercentage);
    }

    // percentage는 프리미엄 펫이 나올 확률
    public void SetPercentage(List<float> percentageList, float percentage)
    {

        float division = (100f - percentage) / 5;
        for(int i=0; i<5; i++)
        {
            percentageList.Add(division);
        }
        for(int i=0; i<4; i++)
        {
            percentageList.Add(percentage/4);
        }
    }

    public void SlotClick(Slot sslot)
    {
        // 눌린 인벤토리 Item slot의 ItemInfo 가져오기
        slot = sslot;
        item = slot.item;
        
        UIManager.instance.informPanel.SetActive(true);
        UIManager.instance.informText.text = item.itemName + "를(을) 사용하시겠습니까?";
    }

    public void UseItem()
    {
        slot.ResetItem();
        if (item.itemName == "일반 펫 상자")
        {
            choice = normalPicker.GetRandomPick();
            PetSetting(choice);
        }
        else if (item.itemName == "프리미엄 펫 상자")
        {
            choice = premiumPicker.GetRandomPick();
            PetSetting(choice);
        }
        else if (item.itemName == "고양이 사료")
        {
            print("사료 먹이기 성공");
        }
    }

    public void SettingGacha(Rito.WeightedRandomPicker<GameObject> picker, List<float> list)
    {
        for(int i=0; i<petPrefab.Count; i++)
        {
            picker.Add(petPrefab[i],list[i]);
        }
    }

    // Render Texture 할 위치에 Prefab 생성 초기화
    public void PetSetting(GameObject prefab)
    {
        GameObject newPet = Instantiate<GameObject>(prefab, PetSpawnPoint);
        newPet.transform.GetComponent<PetInfo>().prefab = newPet;
        Pet.instance.GetPet(newPet);
        newPet.transform.position = PetSpawnPoint.GetChild(0).position;
        newPet.transform.localEulerAngles = new Vector3(0, -180, 0);
        petName.text = newPet.transform.GetComponent<PetInfo>().Name;
        petRank.text = newPet.transform.GetComponent<PetInfo>().Rank + " 등급";
        StartCoroutine(ShowPetCard(newPet));
    }

    IEnumerator ShowPetCard(GameObject newPet)
    {
        PetCard.SetActive(true);
        Pet.instance.anim = newPet.GetComponent<Animation>();
        Pet.instance.RandomPetAnimation();
        yield return new WaitForSeconds(4f);
        newPet.SetActive(false);
        choice = null;
        PetCard.SetActive(false);
    }

}
