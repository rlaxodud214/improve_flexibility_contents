using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Measurement_btn_change : MonoBehaviour
{
    // 순서대로 측면 굴곡/신전, 굴곡/신전, 측면 회전

    public Text PLANE; // CORONAL PLANE
    public Text Bottom_Angle; // 몇 도

    public Button[] buttons; // CORONAL, SAGITTAL, TRANSVERSE, RESULT 버튼 4가지
    public GameObject Change_Origin;
    public Vector3 sensorEulerData;
    public Vector3 offset;
    public int Angle; // data
    public int[] Angle_Value; // Max_Value_에서 1이 굴곡, 측면 굴곡, 좌측 회전에 해당함
    public int num;
    public bool isstart;
    public GameObject[] panels;
    public GameObject[] Left, Right; // [4] - 0 : 캐릭터 Image, 1 : 텍스트, 2 : 막대바_Up, 3 : 막대바_Down
    public Sprite[] Image; // 1_1, 1_2, 2_1, 2_2, 3_1, 3_2

    public User user;   // 현재 user 객체
    public Measurement measurement;
    public int flexion, extension, leftFlexion, rightFlexion, leftRotation, rightRotation;

    public GameObject M_Sportsman;
    public Button ResultBtn;

    //연령별 평균 데이터
    //0굴곡,1신전,2좌측굴,3우측굴,4좌측회전,5우측회전
    private int user_age;
    private float[] age20_mean = { 95f, 36.6f, 48.2f, 48.1f, 34.4f, 34.6f };
    private float[] age30_mean = { 94f, 34.7f, 47.4f, 47.5f, 33.8f, 34.5f };
    private float[] age65_mean = { 94f, 32.1f, 39.2f, 39.5f, 34.3f, 33.6f };
    private float[] use_mean;

    #region Singleton                                         // 싱글톤 패턴은 하나의 인스턴스에 전역적인 접근을 시키며 보통 호출될 때 인스턴스화 되므로 사용하지 않는다면 생성되지도 않습니다.
    private static Measurement_btn_change _Instance;          // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static 선언으로 어디서든 참조가 가능함

    public static Measurement_btn_change Instance                    // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
    {
        get { return _Instance; }                         // _sceneManager이 변수값을 리턴받을 수 있음.
    }
    #endregion

    private void Awake()
    {
        user = UserDataManager.instance.user;
    }

    // Start is called before the first frame update
    void Start()
    {
        // print("User_info_temp.user_age : "+ User_info_temp.user_age);

        user_age = user.age;
        user_age = user_age / 10;
        user_age = int.Parse(user_age.ToString() + "0"); // user 연령대 구하기
        if (user_age >= 60)
            user_age = 65;

        // 연령대에 해당하는 mean 값
        if (user_age == 20) use_mean = (float[])age20_mean.Clone();
        else if (user_age == 30) use_mean = (float[])age30_mean.Clone();
        else if (user_age == 65) use_mean = (float[])age65_mean.Clone();
        else Debug.LogError("해당 연령대의 데이터가 없습니다.");

        set_init();
        num = 1;
        isstart = false;
        Angle_Value = new int[6] { 0, 0, 0, 0, 0, 0 };
    }

    // Update is called once per frame
    void Update()
    {
        if (!isstart)
            return;

        offset = transform.GetComponent<OpenZenMoveObject>().offset;
        sensorEulerData = transform.GetComponent<OpenZenMoveObject>().sensorEulerData;
        switch (num)
        {
            case 1:
                Angle = (int)sensorEulerData.z;
                break;

            case 2:
                Angle = (int)sensorEulerData.y;
                break;

            case 3:
                Angle = (int)sensorEulerData.x;
                break;
        }
        Change_Origin.GetComponent<Image>().fillAmount = Math.Abs((float)Angle) / 360;
        // 좌측굴곡, 굴곡, 좌측회전
        if (Angle < 0)
        {
            Change_Origin.transform.eulerAngles = new Vector3(0, 0, 180 - (float)Angle);
            Angle = Math.Abs(Angle);
            switch (num)
            {
                case 1:
                    if (Angle > Angle_Value[0])
                    {
                        Angle_Value[0] = Angle;
                        leftFlexion = Angle_Value[0];
                        Left[2].GetComponent<Image>().fillAmount = (float)Angle / 90;
                    }

                    break;

                case 2:
                    if (Angle > Angle_Value[2])
                    {
                        Angle_Value[2] = Angle;
                        flexion = Angle_Value[2];
                        Right[2].GetComponent<Image>().fillAmount = (float)Angle / 90;
                    }
                    break;

                case 3:
                    if (Angle > Angle_Value[4])
                    {
                        Angle_Value[4] = Angle;
                        leftRotation = Angle_Value[4];
                        Left[2].GetComponent<Image>().fillAmount = (float)Angle / 90;  // 수정
                    }
                    break;
            }
        }
        // 우측굴곡, 신전, 우회전
        else
        {
            Change_Origin.transform.eulerAngles = new Vector3(0, 0, 180);
            Angle = Math.Abs(Angle);
            switch (num)
            {
                case 1:
                    if (Angle > Angle_Value[1])
                    {
                        Angle_Value[1] = Angle;
                        rightFlexion = Angle_Value[1];
                        Right[2].GetComponent<Image>().fillAmount = (float)Angle / 90;
                    }
                    break;

                case 2:
                    if (Angle > Angle_Value[3])
                    {
                        Angle_Value[3] = Angle;
                        extension = Angle_Value[3];
                        Left[2].GetComponent<Image>().fillAmount = (float)Angle / (90 * 1.2f);
                    }
                    break;

                case 3:
                    if (Angle > Angle_Value[5])
                    {
                        Angle_Value[5] = Angle;
                        rightRotation = Angle_Value[5];
                        Right[2].GetComponent<Image>().fillAmount = (float)Angle / 90;
                    }

                    break;
            }
        }
        Bottom_Angle.text = Angle.ToString("N1") + "°";
    }
    public void Text_Change(int num)
    {
        if (1 <= num && num <= 3)
        {
            M_Sportsman.GetComponent<Animator>().SetBool("ischanged", true);
            this.num = num;
        }

        switch (num)
        {
            case 1: // CORONAL
                set_init();
                for (int i = 0; i < 5; i++)
                    offset += sensorEulerData;

                PLANE.text = "좌/우측 굴곡";
                Left[0].GetComponent<Image>().sprite = Image[0];
                Right[0].GetComponent<Image>().sprite = Image[1];
                Left[1].GetComponent<Text>().text = "좌측 굴곡";
                Right[1].GetComponent<Text>().text = "우측 굴곡";
                Left[3].GetComponent<Image>().fillAmount = use_mean[2] / 90;
                Right[3].GetComponent<Image>().fillAmount = use_mean[3] / 90;
                M_Sportsman.GetComponent<Animator>().SetTrigger(num.ToString());
                break;

            case 2: // SAGITTAL
                set_init();
                for (int i = 0; i < 5; i++)
                    offset += sensorEulerData;

                PLANE.text = "굴곡/신전";
                Left[0].GetComponent<Image>().sprite = Image[2];
                Right[0].GetComponent<Image>().sprite = Image[3];
                Left[1].GetComponent<Text>().text = "굴곡";
                Right[1].GetComponent<Text>().text = "신전";
                Left[3].GetComponent<Image>().fillAmount = use_mean[0] / (90 * 1.2f);
                Right[3].GetComponent<Image>().fillAmount = use_mean[1] / 90;
                M_Sportsman.GetComponent<Animator>().SetTrigger(num.ToString());
                break;

            case 3: // TRANSVERSE
                set_init();
                for (int i = 0; i < 5; i++)
                    offset += sensorEulerData;

                PLANE.text = "좌/우측 회전";
                Left[0].GetComponent<Image>().sprite = Image[4];
                Right[0].GetComponent<Image>().sprite = Image[5];
                Left[1].GetComponent<Text>().text = "좌측 회전";
                Right[1].GetComponent<Text>().text = "우측 회전";
                Left[3].GetComponent<Image>().fillAmount = use_mean[4] / 90;
                Right[3].GetComponent<Image>().fillAmount = use_mean[5] / 90;
                M_Sportsman.GetComponent<Animator>().SetTrigger(num.ToString());
                break;

            case 4: // Start
                isstart = true;
                // 패널 2개 띄우기
                panels[0].SetActive(true);
                Invoke("p0_false", 2.0f);
                break;

            case 5: // RESULT
                // 09/01 : 조건 추가 값이 정상적일 때만 추가하도록 변경
                if (flexion == 0 || extension == 0 || leftFlexion == 0 || rightFlexion == 0 || leftRotation == 0 || rightRotation == 0)
                    break;

                measurement = DBManager.CreateMeasurementData(flexion, extension, leftFlexion, rightFlexion, leftRotation, rightRotation);
                StartCoroutine(SaveMeasurementData(measurement));
                ResultBtn.interactable = false;
                break;
        }
    }

    IEnumerator SaveMeasurementData(Measurement data)
    {
        yield return StartCoroutine(DBManager.SaveMeasurement(data));   //  측정한 데이터 저장
        UserDataManager.instance.recentData = data;
        // Debug.Log("UserDataManager.instance.InitData() 호출함");
        // StartCoroutine(UserDataManager.instance.InitData());
        // yield return new WaitForSeconds(2f);
        // Debug.Log("UserDataManager.instance.InitData() 갔다가 다시 돌아옴");
        UnityEngine.SceneManagement.SceneManager.LoadScene("3-2.RecentResult");
    }

    private void set_init()
    {
        Angle = 0;
        Bottom_Angle.text = "0°";
        sensorEulerData = new Vector3(0, 0, 0);
        Left[1].GetComponent<Text>().text = "";
        Right[1].GetComponent<Text>().text = "";
        Left[2].GetComponent<Image>().fillAmount = 0;
        Right[2].GetComponent<Image>().fillAmount = 0;
        Left[3].GetComponent<Image>().fillAmount = 0;
        Right[3].GetComponent<Image>().fillAmount = 0;
        Change_Origin.GetComponent<Image>().fillAmount = 0;
    }

    public void p0_false()
    {
        panels[0].SetActive(false);
        panels[1].SetActive(true);
    }

    public void Measurement_start() // 측정 시작
    {
        panels[1].SetActive(false);
        set_init();
        Text_Change(num);
        OpenZenMoveObject.Instance.runstart();
    }

    public void SportManActive()
    {
        M_Sportsman.GetComponent<Animator>().SetTrigger("4");
    }
}