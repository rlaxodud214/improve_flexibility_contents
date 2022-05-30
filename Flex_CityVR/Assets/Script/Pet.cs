using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class Pet : MonoBehaviour
{
    public Transform contentRoot;
    public GameObject slotPrefab;
    public List<GameObject> UserPet = new List<GameObject>();
    [HideInInspector]
    public List<PetSlot> slots = new List<PetSlot>();

    public List<AnimationClip> petAnimation = new List<AnimationClip>();

    public static Pet instance;

    private void Awake()
    {
        instance = this;

        // DB 연동 사용자가 가지고 있는 펫 수+4 만큼 생성
        for(int i=0; i<UserPet.Count+2; i++)
        {
            AddSlot();
        }

        // DB 연동 사용자가 가지고 있는 펫이 있으면 펫 슬롯 연동
        if (UserPet != null)
        {
            for(int i=0; i<UserPet.Count; i++)
            {
                PetInfo info = UserPet[i].transform.GetComponent<PetInfo>();
                slots[i].GetComponent<PetSlot>().SetSlot(info);
            }
        }

    }

    // 뽑기 통해 얻은 펫 (중복 방지)
    public void GetPet(GameObject pet)
    {
        bool isExist = false;
        string newName = pet.GetComponent<PetInfo>().Name;

        // slots에 있는 PetSlot 스크립트의 isUse == true 인 것들 중 petInfo의 Name이 pet에 있는 PetInfo 컴포넌트의 Name과 달라야함
        IEnumerable<PetSlot> query = from slot in slots
                                     where slot.isUse == true
                                     select slot;
        foreach (PetSlot slot in query)
        {
            if (slot.petInfo.Name == newName)
            {
                isExist = true;
            }
        }

        if (isExist)
        {
            Debug.Log("이미 보유한 펫입니다.");
        }
        else
        {
            UserPet.Add(pet);
            AddPet();
        }
    }

    // 펫 보관함 슬롯에 추가
    public void AddPet()
    {
        PetSlot emptySlot = slots.Find(t => t.isUse == false);
        PetInfo lastPet = UserPet.Last().transform.GetComponent<PetInfo>();
        print("Last Pet :: "+ lastPet.transform.GetComponent<PetInfo>().Name);

        if (!emptySlot)
        {
            emptySlot = AddSlot();
        }

        emptySlot.SetSlot(lastPet);
    }

    public PetSlot AddSlot()
    {
        GameObject obj = Instantiate<GameObject>(this.slotPrefab, contentRoot);
        PetSlot slot = obj.transform.GetComponent<PetSlot>();
        slot.ResetSlot();
        slots.Add(slot);
        return slot;
    }

    public void SlotClick()
    {
        GameObject obj = EventSystem.current.currentSelectedGameObject.gameObject;
        PetSlot slot = obj.transform.GetComponent<PetSlot>();
        print("SlotClick :: PetName :: " + slot.petInfo.Name);
    }
}
