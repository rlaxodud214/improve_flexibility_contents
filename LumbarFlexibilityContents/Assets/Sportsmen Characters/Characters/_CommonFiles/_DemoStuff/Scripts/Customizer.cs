using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
public enum BodyType
{
    Hair,
    Hand,
    RestOfBody,
    VisibleSuit
}
[System.Serializable]
public class BodyParts
{
    public string bodyName = "";
    public List<GameObject> bodyParts;
    public Button nextBodyPart;
    public Button previosBodyPart;
    public Text bodyNameDisplay;
    public int currentIndex = 0;
    public BodyType bodyType = BodyType.RestOfBody;
}

[System.Serializable]
public class SuitsDefinition
{
    public string suitName = "";
    public List<GameObject> suitParts;
    public bool haveHands;
    public bool haveHelmet;
    public int defaultHairIndex = 0;
    public GameObject[] helmetAndAccessories;
}

[System.Serializable]
public class FacialShapes
{
    public string blendShapeName;
    public Slider blendSlider;
    public Text blendShapeText;
}

[ExecuteInEditMode]
public class Customizer : MonoBehaviour
{
    public static Customizer instance;
    [Header("New Prefabs")]
    public GameObject root;
    public InputField prefabName;
    public Button saveButton;
    public string folderToSaveIn;

    [Header("Body Parts")]
    public List<BodyParts> allBodyParts = new List<BodyParts>();

    [Header("Suits")]
    public List<SuitsDefinition> allSuits = new List<SuitsDefinition>();
    public Button nextSuit;
    public Button previosSuit;
    public Toggle helmetToggle;
    public int currentIndex;
    public Text suitText;

    [Header("BlendShape")]
    public GameObject primaryBlendShape;
    public List<FacialShapes> facialShapes = new List<FacialShapes>();
    public List<GameObject> secondaryBlendShapes = new List<GameObject>();
    SkinnedMeshRenderer skinnedMeshRenderer;




    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            return;
        foreach (BodyParts b in allBodyParts)
        {
            if (b.nextBodyPart && b.previosBodyPart)
            {
                b.bodyParts[b.currentIndex].SetActive(true);
                b.nextBodyPart.onClick.AddListener(() => { ChangePart(b, true); });
                b.previosBodyPart.onClick.AddListener(() => { ChangePart(b, false); });
            }
        }

        foreach (FacialShapes fc in facialShapes)
        {
            if (fc.blendSlider)
            {
                fc.blendShapeText.text = fc.blendShapeName;
                fc.blendSlider.maxValue = 100;
                fc.blendSlider.onValueChanged.AddListener((val) => { ChangeFacialStructure(fc.blendShapeName, val); });
            }
        }

        if (nextSuit && previosSuit && suitText)
        {
            nextSuit.onClick.AddListener(() => { ChangeSuit(true); });
            previosSuit.onClick.AddListener(() => { ChangeSuit(false); });
            suitText.text = allSuits[currentIndex].suitName;
        }

        if (saveButton)
        {
            saveButton.onClick.AddListener(() => { OnClickSaveButton(); });
        }
    }

    void OnClickSaveButton()
    {
        string localPath = prefabName.text;
        if (localPath.Length > 0)
        {
            RunChecks(localPath);
        }
        else
        {
            Debug.LogError("Empty Prefab Name");
        }
    }
    void RunChecks(string name)
    {
        string localPath = "Assets/" + folderToSaveIn + "/" + name + ".prefab";
        if (AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)))
        {
            if (EditorUtility.DisplayDialog("Are you sure?", "A prefab of this name already exists,do you want to replace?", "Yes", "No"))
            {
                SavePrefab(localPath);
            }
        }
        else
        {
            SavePrefab(localPath);
        }
    }
    void SavePrefab(string localPath)
    {
        GameObject go = Instantiate(root, Vector3.zero, Quaternion.identity) as GameObject;
        DestroyImmediate(go.GetComponent<Customizer>());
        DestroyImmediate(go.GetComponent<DragToRotate>());
        DestroyImmediate(go.GetComponent<CapsuleCollider>());
        NewMethod(localPath, go);
        Destroy(go);
    }

    private static void NewMethod(string localPath, GameObject go)
    {
        PrefabUtility.SaveAsPrefabAsset(go, localPath);
    }

    void HelmetChange(bool val, SuitsDefinition s)
    {
        RemoveHeadAccessories(val, s.helmetAndAccessories);
        foreach (BodyParts b in allBodyParts)
        {
            if (b.bodyType == BodyType.Hair)
            {
                b.bodyParts[b.currentIndex].SetActive(false);
                b.nextBodyPart.interactable = !val;
                b.previosBodyPart.interactable = !val;
                b.currentIndex = s.defaultHairIndex;
                b.bodyNameDisplay.text = b.bodyParts[b.currentIndex].gameObject.name;
                b.bodyParts[b.currentIndex].SetActive(true);
            }
        }
    }
    void RemoveHeadAccessories(bool visibility, GameObject[] accessories)
    {
        foreach (GameObject g in accessories)
        {
            g.SetActive(visibility);
        }
    }
    void ChangeSuit(bool increase)
    {
        SuitsDefinition suits = allSuits[currentIndex];
        foreach (GameObject go in suits.suitParts)
            go.SetActive(false);

        if (suits.haveHelmet)
        {
            helmetToggle.interactable = false;
            helmetToggle.onValueChanged.RemoveAllListeners();
        }

        currentIndex += increase ? 1 : -1;
        if (currentIndex > allSuits.Count - 1)
            currentIndex = 0;
        else if (currentIndex < 0)
            currentIndex = allSuits.Count - 1;

        suits = allSuits[currentIndex];
        foreach (GameObject go in suits.suitParts)
            go.SetActive(true);


        if (suits.haveHelmet)
        {
            helmetToggle.interactable = true;
            helmetToggle.isOn = suits.haveHelmet;
            helmetToggle.onValueChanged.AddListener((val) => { HelmetChange(val, suits); });
        }
        if (currentIndex == 0)
        {
            SuitEnable(true, true);
        }
        else
        {
            SuitEnable(suits.haveHands, false);
        }
        suitText.text = allSuits[currentIndex].suitName;

    }
    void SuitEnable(bool hasHands, bool enable)
    {
        foreach (BodyParts b in allBodyParts)
        {
            switch (b.bodyType)
            {
                case BodyType.VisibleSuit:
                    continue;
                case BodyType.Hand:
                    if (!hasHands)
                    {
                        b.bodyParts[b.currentIndex].SetActive(true);
                        b.nextBodyPart.interactable = true;
                        b.previosBodyPart.interactable = true;
                        continue;
                    }
                    break;
                case BodyType.Hair:
                    if (allSuits[currentIndex].haveHelmet)
                    {
                        b.nextBodyPart.interactable = false;
                        b.previosBodyPart.interactable = false;
                        b.bodyParts[b.currentIndex].SetActive(false);
                    }
                    int index = allSuits[currentIndex].haveHelmet ? allSuits[currentIndex].defaultHairIndex : b.currentIndex;
                    b.currentIndex = index;
                    b.bodyNameDisplay.text = b.bodyParts[b.currentIndex].gameObject.name;
                    b.bodyParts[b.currentIndex].SetActive(true);
                    if (!allSuits[currentIndex].haveHelmet)
                    {
                        b.nextBodyPart.interactable = true;
                        b.previosBodyPart.interactable = true;
                    }
                    continue;
            }
            /*if(b.bodyType==BodyType.VisibleSuit)
                continue;
            if ((b.bodyType == BodyType.Hand && !hasHands) || (b.bodyType==BodyType.Hair && !allSuits[currentIndex].haveHelmet)){
                b.bodyParts[b.currentIndex].SetActive(true);
                b.nextBodyPart.interactable = true;
                b.previosBodyPart.interactable = true;
                continue;
            }*/
            b.bodyParts[b.currentIndex].SetActive(enable);
            b.nextBodyPart.interactable = enable;
            b.previosBodyPart.interactable = enable;
        }
    }
    void ChangeFacialStructure(string key, float val)
    {
        skinnedMeshRenderer.SetBlendShapeWeight(skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(key), val);
        ChangeSecondaryFacial(key, val);
    }
    void ChangeSecondaryFacial(string key, float val)
    {
        foreach (GameObject g in secondaryBlendShapes)
        {
            SkinnedMeshRenderer smr = g.GetComponent<SkinnedMeshRenderer>();
            Mesh mesh = smr.sharedMesh;
            int index = mesh.GetBlendShapeIndex(key);
            if (index >= 0)
            {
                smr.SetBlendShapeWeight(index, val);
            }
        }
    }
    void ChangePart(BodyParts b, bool increase)
    {
        b.bodyParts[b.currentIndex].SetActive(false);
        b.currentIndex += increase ? 1 : -1;
        if (b.currentIndex > b.bodyParts.Count - 1)
            b.currentIndex = 0;
        if (b.currentIndex < 0)
            b.currentIndex = b.bodyParts.Count - 1;
        b.bodyParts[b.currentIndex].SetActive(true);
        b.bodyNameDisplay.text = b.bodyParts[b.currentIndex].gameObject.name;
    }
    void GetBlendShapes()
    {
        int count = skinnedMeshRenderer.sharedMesh.blendShapeCount;
        if (count == facialShapes.Count)
            return;
        for (int i = 0; i < count; i++)
        {
            FacialShapes facialShape = new FacialShapes();
            facialShape.blendShapeName = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(i);
            facialShapes.Add(facialShape);
        }
    }


    void UpdateInspector()
    {
        foreach (BodyParts b in allBodyParts)
        {
            b.bodyParts.Clear();
            if (b.bodyName.Length > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    GameObject go = transform.GetChild(i).gameObject;
                    bool match = Regex.IsMatch(go.name, "(.+)" + b.bodyName + "*");
                    if (match)
                        b.bodyParts.Add(go);
                }
            }
        }

        if (secondaryBlendShapes.Count == 0 && primaryBlendShape != null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject go = transform.GetChild(i).gameObject;
                SkinnedMeshRenderer rend = go.GetComponent<SkinnedMeshRenderer>();
                if (rend == null || go.name == primaryBlendShape.name)
                    continue;
                if (rend.sharedMesh.blendShapeCount > 0)
                {
                    secondaryBlendShapes.Add(go);
                }
            }
        }

        if (primaryBlendShape != null)
        {
            skinnedMeshRenderer = primaryBlendShape.GetComponent<SkinnedMeshRenderer>();
            GetBlendShapes();
        }

        foreach (SuitsDefinition s in allSuits)
        {
            s.suitParts.Clear();
            if (s.suitName.Length > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    GameObject go = transform.GetChild(i).gameObject;
                    bool match = Regex.IsMatch(go.name, "(.+)" + s.suitName + "*");
                    if (match)
                        s.suitParts.Add(go);
                }
            }
        }
    }


    void OnValidate()
    {
        UpdateInspector();
    }

}
#endif