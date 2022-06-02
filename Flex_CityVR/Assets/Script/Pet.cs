﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class Pet : MonoBehaviour
{
    public Transform contentRoot;
    public GameObject slotPrefab;

    public List<GameObject> UserPet = new List<GameObject>();     // 보유 중인 펫 목록
    Transform PetSpawnPoint;

    public UnityEngine.UI.Text petName;

    [HideInInspector]
    public List<PetSlot> slots = new List<PetSlot>();

    private GameObject previousPrefab;

    // 펫 랜덤 애니메이션
    public List<string> animArray = new List<string>();
    public Animation anim;
    int randNum;

    public static Pet instance;

    private void Awake()
    {
        instance = this;

        #region 변수 초기화
        previousPrefab = null;
        PetSpawnPoint = ItemUse.instance.PetSpawnPoint;
        #endregion
        //RandomAnimation();
        // DB 연동 사용자가 가지고 있는 펫 수+4 만큼 생성
        for (int i=0; i<UserPet.Count+2; i++)
        {
            AddSlot();
        }

        // DB 연동 사용자가 가지고 있는 펫이 있으면 펫 슬롯 연동
        if (UserPet != null)
        {
            for(int i=0; i<UserPet.Count; i++)
            {
                GameObject newPet = Instantiate<GameObject>(UserPet[i], PetSpawnPoint);
                newPet.transform.GetComponent<PetInfo>().prefab = newPet;
                newPet.transform.position = PetSpawnPoint.GetChild(0).position;
                newPet.transform.localEulerAngles = new Vector3(0, -180, 0);

                PetInfo info = newPet.transform.GetComponent<PetInfo>();
                slots[i].GetComponent<PetSlot>().SetSlot(info);
                newPet.SetActive(false);
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

        if (!emptySlot)
        {
            emptySlot = AddSlot();
        }

        emptySlot.SetSlot(lastPet);
    }

    // 슬롯이 부족한 경우 추가
    public PetSlot AddSlot()
    {
        GameObject obj = Instantiate<GameObject>(this.slotPrefab, contentRoot);
        PetSlot slot = obj.transform.GetComponent<PetSlot>();
        slot.ResetSlot();
        slots.Add(slot);
        return slot;
    }

    // 슬롯 클릭 시 선택한 슬롯의 펫 TextureRender
    public void SlotClick()
    {
        GameObject obj = EventSystem.current.currentSelectedGameObject.gameObject;
        PetSlot slot = obj.transform.GetComponent<PetSlot>();
        anim = slot.petInfo.prefab.GetComponent<Animation>();
        petName.text = ""+ slot.petInfo.Name;
        if (previousPrefab != null)
        {
            previousPrefab.SetActive(false);
        }
        slot.petInfo.prefab.gameObject.SetActive(true);
        RandomPetAnimation();
        previousPrefab = slot.petInfo.prefab;
    }

    public void RandomPetAnimation()
    {
        animArray.Clear();
        randNum = -1;
        foreach (AnimationState state in anim)
        {
            animArray.Add(state.name);
        }
        randNum = Random.Range(0, animArray.Count);
        anim.Play(animArray[randNum]);
        anim.wrapMode = WrapMode.Once;
    }
}
