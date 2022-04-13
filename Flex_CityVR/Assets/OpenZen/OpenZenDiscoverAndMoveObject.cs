using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Example Behaviour which applies the measured OpenZen sensor orientation to a 
 * Unity object.
 */
public class OpenZenDiscoverAndMoveObject : MonoBehaviour
{
    public int ObjectNum;
    struct SensorListResult
    {
        public string IoType;     // 입출력 타입
        public string Identifier; // 식별자 - 장치 이름
        public uint BaudRate;     // 통신 속도? ex) 921600
    }

    public GameObject mObjConnectButton;   // Connect 버튼 오브젝트
    public Button mConnectButton;          // Connect 버튼 오브젝트의 Button 컴포넌트
    public GameObject mConnectCanvas;      // Home UI 창
    public GameObject mErrorConnect;       // 에러 표시하는 텍스트 오브젝트
    public Text mTextTitle;                // 안내 Text오브젝트 Please select Sensor


    List<SensorListResult> mFoundSensors = new List<SensorListResult>(10);
    // 중복되는 변수들 아래와 같이 리스트로 변경함
    public List<Dropdown> mDropdownSensorSelect = new List<Dropdown>(2);// 드롭다운
    public List<GameObject> lpms_cu = new List<GameObject>(2);
    public List<ZenClientHandle_t> mZenHandle = new List<ZenClientHandle_t>(2);
    public List<ZenSensorHandle_t> mSensorHandle = new List<ZenSensorHandle_t>(2);
    public int type = 1;


    // Use this for initialization
    void Start()
    {
        ObjectNum = 2; // 센서 갯수

        // create OpenZen and start asynchronous sensor discovery
        for (int i = 0; i < ObjectNum; i++)
        {
            mZenHandle.Add(new ZenClientHandle_t());
            OpenZen.ZenInit(mZenHandle[i]);
            OpenZen.ZenListSensorsAsync(mZenHandle[i]);
        }
        mConnectButton = mObjConnectButton.GetComponent<Button>();
        mTextTitle = GameObject.Find("txtTitle").GetComponent<Text>();
        mObjConnectButton.SetActive(false);                         // 디바이스 탐색중에 Connect 버튼 비활성화
        mErrorConnect.SetActive(false);                             // 초기화면에서 에러 메시지 비활성화
        mConnectButton.onClick.AddListener(onConnectButtonClicked); // Connect 버튼을 눌렀을 때 onConnectButtonClicked()가 실행되게 추가
    }

    void onConnectButtonClicked() // Find Sensor And Settings
    {
        for (int i = 0; i < ObjectNum; i++)
        {
            int selectedItem = mDropdownSensorSelect[i].value;  // 여러 센서중에 사용자가 선택한 것이 목록중에 몇 번째에 있는지 인덱스(순서) 값 저장하는 거                                                                                                                                                                                                                
            var sensorConnectTo = mFoundSensors[selectedItem]; // mFoundSensors찾은 센서들 중에 선택된 인덱스값으로 센서가져오기
            mSensorHandle.Add(new ZenSensorHandle_t());
            // connect to the first available sensor in the list of found sensors
            print(sensorConnectTo.Identifier + "  센서에 연결중,,,,, ");

            // 디버그로 확인시 아래 var sensorInitError = OpenZen.ZenObtainSensorByName() 실행 후 블루투스 연결됌
            var sensorInitError = OpenZen.ZenObtainSensorByName(mZenHandle[i], sensorConnectTo.IoType, sensorConnectTo.Identifier,
                                                                 sensorConnectTo.BaudRate, mSensorHandle[i]); 

            Sensor_Settings(sensorInitError, mSensorHandle[i], mZenHandle[i], sensorConnectTo); 
        }
    }

    void Sensor_Settings(ZenSensorInitError sensorInitError, ZenSensorHandle_t mSensorHandle, ZenClientHandle_t mZenHandle, SensorListResult sensorConnectTo)
    {
        if (sensorInitError == ZenSensorInitError.ZenSensorInitError_None)
        {
            print(sensorConnectTo.Identifier + "  센서 연결 성공");
            mConnectCanvas.SetActive(false);
            mErrorConnect.SetActive(false);

            ZenComponentHandle_t mComponent = new ZenComponentHandle_t();
            OpenZen.ZenSensorComponentsByNumber(mZenHandle, mSensorHandle, OpenZen.g_zenSensorType_Imu, 0, mComponent);

            // enable sensor streaming, normally on by default anyways 센서 스트리밍을 활성화합니다. 일반적으로 항상 켜져 있습니다.
            OpenZen.ZenSensorComponentSetBoolProperty(mZenHandle, mSensorHandle, mComponent,
               (int)EZenImuProperty.ZenImuProperty_StreamData, true);

            // set the sampling rate to 100 Hz 샘플링 속도를 100Hz로 설정
            OpenZen.ZenSensorComponentSetInt32Property(mZenHandle, mSensorHandle, mComponent,
               (int)EZenImuProperty.ZenImuProperty_SamplingRate, 100);

            // filter mode using accelerometer & gyroscope & magnetometer 가속도계 및 자이로 스코프 및 자력계를 사용한 필터 모드
            OpenZen.ZenSensorComponentSetInt32Property(mZenHandle, mSensorHandle, mComponent,
               (int)EZenImuProperty.ZenImuProperty_FilterMode, 2); // 필터 모드 설정

            // Ensure the Orientation data is streamed out 방향 데이터가 스트리밍되는지 확인
            OpenZen.ZenSensorComponentSetBoolProperty(mZenHandle, mSensorHandle, mComponent,
               (int)EZenImuProperty.ZenImuProperty_OutputGyr0BiasCalib, true);

            print(sensorConnectTo.Identifier + "  센서 구성 완료"); // 센서 구성 완료

            
            
        }
        else // 초기화때 블루투스 연결이 안됐을 때
        {
            mErrorConnect.SetActive(true);
            mSensorHandle = null;
            print(sensorConnectTo.Identifier + "  센서 연결 실패");
        }
    }


    // Update is called once per frame
    void Update()
    {
        zenEvent_Handling(mZenHandle[0], lpms_cu[0]);
        zenEvent_Handling(mZenHandle[1], lpms_cu[1]);
    }

    void zenEvent_Handling(ZenClientHandle_t mZenHandle, GameObject lpms_cu) // zenEvent 처리
    {
        // run as long as there are new OpenZen events to process 처리 할 새 OpenZen 이벤트가있는 한 실행
        while (true)
        {
            ZenEvent zenEvent = new ZenEvent();
            // read all events which are waiting for us - 우리를 기다리고있는 모든 이벤트를 읽습니다.
            // use the rotation from the newest IMU event - 최신 IMU 이벤트의 회전 사용
            if (!OpenZen.ZenPollNextEvent(mZenHandle, zenEvent))
                break;

            // if compontent handle = 0, this is a OpenZen wide event, like sensor search
            // zenEvent.component.handle = 0이면 센서 검색과 같은 OpenZen 전체 이벤트입니다.

            if (zenEvent.component.handle == 0)
            {
                // if the handle is on, its not a sensor event but a system wide - 핸들이 켜져 있으면 센서 이벤트가 아니라 시스템 전체 이벤트이다.
                // event
                switch (zenEvent.eventType)
                {
                    case ZenEventType.ZenEventType_SensorFound:

                        // 연결된 센서의 정보를 출력해줌 센서이름, 식별자, 입출력타입(USB or 블루투스)
                        print("연결된 센서의 정보 - sensor name: " + zenEvent.data.sensorFound.name +
                            " identifier : " + zenEvent.data.sensorFound.identifier + 
                            " IoType : " + zenEvent.data.sensorFound.ioType);

                        // store all found sensors in a local list - 발견된 모든 센서를 로컬 목록에 저장
                        SensorListResult localDesc = new SensorListResult();

                        localDesc.Identifier = zenEvent.data.sensorFound.identifier;  // 식별자 : 디바이스 정보 ex)00:04:3E:9B:A2:A5
                        localDesc.IoType = zenEvent.data.sensorFound.ioType;          // 입출력타입(USB or 블루투스)
                        localDesc.BaudRate = zenEvent.data.sensorFound.baudRate;      // 통신 규격?
                        
                        mFoundSensors.Add(localDesc); // 찾은 센서들을 List에 저장

                        // add the found sensor to the dropdown selection menu - 드롭 다운 선택 메뉴에 발견 된 센서 추가
                        List<string> dropOptions = new List<string>();
                        dropOptions.Add(zenEvent.data.sensorFound.name); // dropOptions : 드롭 다운에 들어갈 옵션들

                        // 3개의 드롭박스에 옵션을 다 넣어줌
                        mDropdownSensorSelect[0].AddOptions(dropOptions);
                        mDropdownSensorSelect[1].AddOptions(dropOptions);

                        break;

                    case ZenEventType.ZenEventType_SensorListingProgress:
                        if (zenEvent.data.sensorListingProgress.complete > 0)
                        {
                            mTextTitle.text = "Please select Sensor";
                            mObjConnectButton.SetActive(true);
                        }
                        break;
                }
            }
            else
            {
                switch (zenEvent.eventType)
                {
                    case ZenEventType.ZenEventType_ImuData:
                        // read acceleration 가속도 읽기 -> 가속도 값과 행동을 라벨링 시켜서 머신러닝 시킨 후 최종적으론 행동 예측하기!
                        OpenZenFloatArray fa = OpenZenFloatArray.frompointer(zenEvent.data.imuData.a);
                        // read euler angles 오일러각 읽기
                        OpenZenFloatArray fr = OpenZenFloatArray.frompointer(zenEvent.data.imuData.r);
                        // read quaternion   쿼터니언 읽기
                        OpenZenFloatArray fq = OpenZenFloatArray.frompointer(zenEvent.data.imuData.q);
                        // read timestamp    시간값 읽기
                        // OpenZenFloatArray ft = OpenZenFloatArray.frompointer(zenEvent.data.imuData.timestamp);

                        // Unity Quaternion constructor has order x,y,z,w
                        // Furthermore, y and z axis need to be flipped to 
                        // convert between the LPMS and Unity coordinate system
                        // 쿼터니언(x, y, z, w)로 이루어지며 각 성분은 축이나 각도를 의미하는 게 아니라, 하나의 벡터(x, y, z)와 하나의 스칼라(w, roll)를 의미한다.
                        // 
                        Quaternion sensorOrientation = new Quaternion(fq.getitem(1), fq.getitem(3), fq.getitem(2), fq.getitem(0));
                            Debug.Log("오일러각.x : " + fr.getitem(0) + "오일러각.y : " + fr.getitem(1) + "오일러각.z : " + fr.getitem(2));
                        lpms_cu.transform.localRotation = sensorOrientation;
                        break;
                }
            }
        }
    }
    void OnDestroy()
    {
        for (int i = 0; i < ObjectNum; i++)
        {
            if (mSensorHandle[i] != null)
            {
                OpenZen.ZenReleaseSensor(mZenHandle[i], mSensorHandle[i]);
            }
        }
    }

    public void Reset()
    {
        lpms_cu[0].transform.rotation = new Quaternion(0, 0, 0, 0);
        lpms_cu[0].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        lpms_cu[1].transform.rotation = new Quaternion(0, 0, 0, 0);
        lpms_cu[1].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}