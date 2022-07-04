using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Example Behaviour which applies the measured OpenZen sensor orientation to a 
 * Unity object.
 */
public class OpenZenMoveObject : MonoBehaviour
{
    /*
    데이터는 시작시 오프셋값 빼고 (0, 0, 0)에서 시작함 데이터 범위는 0~360도임

    float data = OpenZenMoveObject.Instance.sensorEulerData.x; // 비틀기 - 카약, 담당자 : 노승재
    float data = OpenZenMoveObject.Instance.sensorEulerData.y; // 굴곡/신전 - 림보, 담당자 : 오승연,박소윤
    float data = OpenZenMoveObject.Instance.sensorEulerData.z; // 편측 굴곡/신전 - 골키퍼, 담당자 : 김희영
     */

    #region Singleton                                         // 싱글톤 패턴은 하나의 인스턴스에 전역적인 접근을 시키며 보통 호출될 때 인스턴스화 되므로 사용하지 않는다면 생성되지도 않습니다.
    private static OpenZenMoveObject _Instance;          // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static 선언으로 어디서든 참조가 가능함
    public static OpenZenMoveObject Instance                  // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
    {
        get                       // _sceneManager이 변수값을 리턴받을 수 있음.
        {
            if (_Instance == null)
                _Instance = FindObjectOfType<OpenZenMoveObject>();
            return _Instance;
        }
    }
    #endregion

    ZenClientHandle_t mZenHandle = new ZenClientHandle_t();
    ZenSensorHandle_t mSensorHandle = new ZenSensorHandle_t();

    public Vector3 sensorEulerData; // 데이터 측정값
    public Vector3 offset; // 0, 0, 0으로 만들어주기
    public bool save_offset;

    public enum OpenZenIoTypes { SiUsb, Bluetooth };

    [Tooltip("IO Type which OpenZen should use to connect to the sensor.")]
    public OpenZenIoTypes OpenZenIoType = OpenZenIoTypes.Bluetooth;

    [Tooltip("Idenfier which is used to connect to the sensor. The name depends on the IO type used and the configuration of the sensor.")]
    public string OpenZenIdentifier = "00:04:3E:9B:A2:A5";
    // public string OpenZenIdentifier = "00:04:3E:9B:A2:85";

    // Use this for initialization
    void Start()
    {
        //OpenZenIdentifier = "00:04:3E:9B:A2:A5";
        OpenZenIdentifier = "00:04:3E:9B:A2:85";
        save_offset = false;
        sensorEulerData = new Vector3(0, 0, 0);
        offset = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

        // run as long as there are new OpenZen events to process
        while (true)
        {
            ZenEvent zenEvent = new ZenEvent();
            // read all events which are waiting for us
            // use the rotation from the newest IMU event
            if (!OpenZen.ZenPollNextEvent(mZenHandle, zenEvent))
                break;

            // if compontent handle = 0, this is a OpenZen wide event,
            // like sensor search
            if (zenEvent.component.handle != 0)
            {
                switch (zenEvent.eventType)
                {
                    case ZenEventType.ZenEventType_ImuData:
                        // read quaternion
                        OpenZenFloatArray fq = OpenZenFloatArray.frompointer(zenEvent.data.imuData.r); // 아웃풋 데이터 쿼터니언->오일러로 변경함
                                                                                                        // Unity Quaternion constructor has order x,y,z,w
                                                                                                        // Furthermore, y and z axis need to be flipped to 
                                                                                                        // convert between the LPMS and Unity coordinate system
                        sensorEulerData = new Vector3(fq.getitem(2) * -1f, fq.getitem(0) * -1f, fq.getitem(1));

                        /* Quaternion sensorOrientation = new Quaternion(fq.getitem(1), fq.getitem(3), fq.getitem(2), fq.getitem(0));*/
                        // transform.rotation = sensorOrientation;

                        // offset 최초 1번 따기
                        if (!save_offset)
                        {
                            offset = sensorEulerData;
                            save_offset = true;
                        }

                        // (0, 0, 0) 만들기
                        sensorEulerData -= offset;

                        sensorEulerData.x = (float)dataset(sensorEulerData.x);
                        sensorEulerData.y = (float)dataset(sensorEulerData.y);
                        sensorEulerData.z = (float)dataset(sensorEulerData.z);

                        // print("x : " + sensorEulerData.x.ToString("N0") + ", y : " + sensorEulerData.y.ToString("N0") + ", z : " + sensorEulerData.z.ToString("N0"));
                        break;
                }
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            offset += sensorEulerData;
        }
    }

    // 0~360범위 내의 값만 나오게 하는 코드
    public double dataset(double data)
    {
        if (data > 180)
            data -= 360;
        else if (data < -180)
            data += 360;

        return data;

        /*  if (data > 360)
              data -= 360;
          else if (data < 0)
              data += 360;

          return data;*/
    }
    void OnDestroy()
    {
        if (mSensorHandle != null)
        {
            OpenZen.ZenReleaseSensor(mZenHandle, mSensorHandle);
        }
        OpenZen.ZenShutdown(mZenHandle);
    }

    public void runstart()
    {
        StartCoroutine(sensor_connect());
    }

    IEnumerator sensor_connect()
    {
        var is_connect = false;
        // create OpenZen
        OpenZen.ZenInit(mZenHandle);
        // Hint: to get the io type and identifer for all connected sensor,
        // you cant start the DiscoverSensorScene. The information of all 
        // found sensors is printed in the debug console of Unity after
        // the search is complete.

        print("Trying to connect to OpenZen Sensor on IO " + OpenZenIoType + " with sensor name " + OpenZenIdentifier);
        var sensorInitError = OpenZen.ZenObtainSensorByName(mZenHandle, OpenZenIoType.ToString(), OpenZenIdentifier, 0, mSensorHandle);

        if (sensorInitError != ZenSensorInitError.ZenSensorInitError_None)
        {
            print("Error while connecting to sensor.");
            print("IMU 센서 연결 대기중");
            yield return new WaitForSeconds(1f);
        }

        else
        {
            ZenComponentHandle_t mComponent = new ZenComponentHandle_t();
            OpenZen.ZenSensorComponentsByNumber(mZenHandle, mSensorHandle, 
                OpenZen.g_zenSensorType_Imu, 0, mComponent);

            // enable sensor streaming, normally on by default anyways 스트리밍 활성화
            OpenZen.ZenSensorComponentSetBoolProperty(mZenHandle, mSensorHandle, mComponent,
                (int)EZenImuProperty.ZenImuProperty_StreamData, true);

            // set the sampling rate to 100 Hz // rate 설정
            OpenZen.ZenSensorComponentSetInt32Property(mZenHandle, mSensorHandle, mComponent,
                (int)EZenImuProperty.ZenImuProperty_SamplingRate, 100);

            // filter mode using accelerometer & gyroscope & magnetometer 필터 모드 설정
            OpenZen.ZenSensorComponentSetInt32Property(mZenHandle, mSensorHandle, mComponent,
                (int)EZenImuProperty.ZenImuProperty_FilterMode, 2);

            OpenZen.ZenSensorComponentSetBoolProperty(mZenHandle, mSensorHandle, mComponent,
                (int)EZenImuProperty.ZenImuProperty_OutputGyr0AlignCalib, true);

            // (int)EZenImuProperty.ZenImuProperty_OutputQuat, true);
            // (int)EZenImuProperty.ZenImuProperty_OutputEuler, true);
            print("Sensor configuration complete");
            is_connect = true;
        }
        if (is_connect == false)
            runstart();
    }
}
