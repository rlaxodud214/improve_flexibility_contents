using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using LpmsCSharpWrapper;
using System.IO;

public class data
{
    public string GyroX;
    public string GyroY;
    public string GyroZ;
    public string QuaternionX;
    public string QuaternionY;
    public string QuaternionZ;
}

public class LpmsTest : MonoBehaviour {
    // string lpmsSensor = "00:04:3E:94:3D:9E";
    // string lpmsSensor = "00:04:3E:9B:A2:B3";
    // string lpmsSensor = "00:04:3E:9B:A2:A5";
    string lpmsSensor = "00:04:3E:9B:A2:85";
    SensorData sd, sd_phone;
    public bool sd_data_isinit;
    public Text t1, t2, t3, t4;
    public List<data> please;
    public Dictionary<string, string> QtoA;

    // x, y, z
    private double roll, pitch, yaw;
    
    public double[] gyro;
    public double[] gyro_Speed; // 각속도
    public double[] offset;
    public float[] offset22;

    // 단위 시간을 구하기 위한 변수
    private double timestamp = 0.0;
    private double bTime = 0.0;
    private double dt;

    // 회전각을 구하기 위한 변수
    public double rad_to_dgr = 180 / Math.PI;
    public float NS2S = 1.0f / 1000000000.0f;

    #region Singleton                                         // 싱글톤 패턴은 하나의 인스턴스에 전역적인 접근을 시키며 보통 호출될 때 인스턴스화 되므로 사용하지 않는다면 생성되지도 않습니다.
    private static LpmsTest _Instance;          // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static 선언으로 어디서든 참조가 가능함

    public static LpmsTest Instance                    // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
    {
        get { return _Instance; }                         // _sceneManager이 변수값을 리턴받을 수 있음.
    }


    #endregion

    public void Awake()
    {
        sd_data_isinit = false;
        if (LpSensorManager.getConnectionStatus(lpmsSensor) == LpSensorManager.SENSOR_CONNECTION_CONNECTED)
        {
            print("연결 성공");
            unsafe
            {
                sd = *((SensorData*)LpSensorManager.getSensorData(lpmsSensor)); // 센서로부터 데이터 받아오는 코드 해당 코드를 바꿔야함

                /*                var x = Input.gyro.attitude.x.ToString("N2");
                                var y = Input.gyro.attitude.y.ToString("N2");
                                var z = Input.gyro.attitude.z.ToString("N2");*/
            }
            sd_data_isinit = true;
            print("sd.rx : " + sd.rx + "sd.ry : " + sd.ry + "sd.rz : " + sd.rz);
        }
        Screen.orientation = ScreenOrientation.LandscapeLeft; // 디바이스를 가로 방향으로 고정시키기
        // Screen.orientation = ScreenOrientation.Portrait; // 디바이스를 세로 방향으로 고정시키기
    }
    public void Start()
    {
        Input.gyro.enabled = true; // 해당 기기가 자이로스코프를 지원할 경우 자이로 스코프 켜기
        gyro = new double[3] { 0, 0, 0 };
        gyro_Speed = new double[3] { 0, 0, 0 };
        offset = new double[3] { 0, 0, 0 };
        offset22 = new float[3] { 0, 0, 0 };
    }
    // Update is called once per frame
    public void Update()
    {
        //sd_phone.rx = Input.gyro.attitude.eulerAngles.y; // SAGITTAL     - rx 사용   // 스마트폰 자이로 각도 // 정상 작동함
        //// Input.gyro.
        //sd_phone.ry = -Input.gyro.attitude.eulerAngles.z; // CORONAL      - ry 사용   // 스마트폰 자이로 각도
        //sd_phone.rz = Input.gyro.attitude.eulerAngles.z; // TRANSVERSE   - rz 사용   // 스마트폰 자이로 각도
        gyro_Speed[0] = Input.gyro.rotationRate.x;
        gyro_Speed[1] = Input.gyro.rotationRate.y;
        gyro_Speed[2] = Input.gyro.rotationRate.z;
        // print("1. " + Input.gyro.rotationRateUnbiased.x);
        // print("2. " + Input.gyro.rotationRate.x);

        sd_phone.rx = Input.gyro.attitude.eulerAngles.x; // SAGITTAL     - rx 사용   // 스마트폰 자이로 각도
        sd_phone.ry = Input.gyro.attitude.eulerAngles.y; // CORONAL      - ry 사용   // 스마트폰 자이로 각도
        sd_phone.rz = Input.gyro.attitude.eulerAngles.z; // TRANSVERSE   - rz 사용   // 스마트폰 자이로 각도

        var temp = new data();
        print("연결 시도중");
        if (LpSensorManager.getConnectionStatus(lpmsSensor) == LpSensorManager.SENSOR_CONNECTION_CONNECTED)
        {
            print("연결 성공");
            unsafe
            {
                sd = *((SensorData*)LpSensorManager.getSensorData(lpmsSensor)); // 센서로부터 데이터 받아오는 코드 해당 코드를 바꿔야함
                
/*                var x = Input.gyro.attitude.x.ToString("N2");
                var y = Input.gyro.attitude.y.ToString("N2");
                var z = Input.gyro.attitude.z.ToString("N2");*/
            }
            sd_data_isinit = true;
            print("sd.rx : " + sd.rx + "sd.ry : " + sd.ry + "sd.rz : " + sd.rz);
        }
        // sd.rx = (float)dataset(sd.rx - offset22[0]);
        // sd.ry = (float)dataset(sd.ry - offset22[1]);
        // sd.rz = (float)dataset(sd.rz - offset22[2]);

/*        // 각속도 적분
        timestamp += Time.deltaTime;
        dt = (timestamp - bTime);
        bTime = timestamp;
        print("dt : " + dt.ToString("N2"));

        // 시간이 변화했으면
        if (dt - timestamp != 0) {
            roll += gyro_Speed[0] * dt * rad_to_dgr;
            pitch += gyro_Speed[1] * dt * rad_to_dgr;
            yaw += gyro_Speed[2] * dt * rad_to_dgr;
        }*/

        // 0~360 만 나오게 돌리기
/*        roll = dataset(roll);
        pitch = dataset(pitch);
        yaw = dataset(yaw);

        gyro[0] = dataset(sd_phone.rx - offset[0]);
        gyro[1] = dataset(sd_phone.ry - offset[1]);
        gyro[2] = dataset(sd_phone.rz - offset[2]);

        t1.text = "LPMS - x : " + sd.rx.ToString("N0") + "  y : " + sd.ry.ToString("N0") + "  z : " + sd.rz.ToString("N0");
        t2.text = "offset x : " + gyro[0].ToString("N2") + ", y : " + gyro[1].ToString("N2") + ", z : " + gyro[2].ToString("N2");
        t3.text = "기본 x : " + Input.gyro.attitude.eulerAngles.x.ToString("N2") + ", " + "y : " + Input.gyro.attitude.eulerAngles.y.ToString("N2") + ", " + "z : " + Input.gyro.attitude.eulerAngles.z.ToString("N2");
        t4.text = "적분 x : " + roll.ToString("N2") + ", y : " + pitch.ToString("N2") + ", z : " + yaw.ToString("N2");
        System.Threading.Thread.Sleep(100);*/
    }

    /*public void data_reset()
    {
        print("눌림");
        roll = 0; pitch = 0; yaw = 0;
        offset[0] = Input.gyro.attitude.eulerAngles.x;
        offset[1] = Input.gyro.attitude.eulerAngles.y;
        offset[2] = Input.gyro.attitude.eulerAngles.z;

        offset22[0] = sd.rx;
        offset22[1] = sd.ry;
        offset22[2] = sd.rz;
    }

    public double dataset(double data)
    {
        if (data > 360)
            data -= 360;
        else if (data < 0)
            data += 360;
        
        return data;
    }*/

    // Use this for initialization
    public void Excute ()
    {
        // Initialize sensor manager
        LpSensorManager.initSensorManager();

        // connects to sensor
        LpSensorManager.connectToLpms(LpSensorManager.DEVICE_LPMS_B2, lpmsSensor);

        // Wait for establishment of sensor1 connection
        while (LpSensorManager.getConnectionStatus(lpmsSensor) != 1)
        {
            print("IMU 센서 연결 대기중");
            System.Threading.Thread.Sleep(200);
        }
        Debug.Log("Sensor connected");

        // Sets sensor offset
        LpSensorManager.setOrientationOffset(lpmsSensor, LpSensorManager.LPMS_OFFSET_MODE_HEADING);
        Debug.Log("Offset set");
    }
    
    public float getSensorDataX() { return sd.rx; }
    public float getSensorDataY() { return sd.ry; }
    public float getSensorDataZ() { return sd.rz; }
    public void  setSensorDataX(float Angle) { sd.rx = Angle; }
    public void  setSensorDataY(float Angle) { sd.ry = Angle; }
    public void  setSensorDataZ(float Angle) { sd.rz = Angle; }
    public float getSengsetGDataX() { return sd.gRawX; }
    public float getSengsetGDataY() { return sd.gRawY; }
    public float getSengsetGDataZ() { return sd.gRawZ; }

    void OnDestroy()
    {
        Debug.Log("PrintOnDestroy");
        LpSensorManager.disconnectLpms(lpmsSensor);
        // Destroy sensor manager and free up memory
        LpSensorManager.deinitSensorManager();
    }

    //public void OnApplicationQuit()
    //{
    //    UpdateLineFile();
    //}

    //public void UpdateLineFile() //파일 업데이트 or 파일 쓰기
    //{
    //    print("CSV 저장");
    //    string filePath = CSVData.getPath(); // 파일 경로 받아오기
    //    StreamWriter outStream = System.IO.File.CreateText(filePath); // 출력 스트림 생성
    //    string str; // 파일에 저장할 string형 타입 임시 변수 선언
    //    outStream.WriteLine("Gyroscope_Angle_X, Gyroscope_Angle_Y, Gyroscope_Angle_Z, Quaternion_X, Quaternion_Y, Quaternion_Z");

    //    for(int i=0; i<please.Count; i++)
    //    {
    //        data Data = new data();                   // Data객체 생성
    //        str = please[i].GyroX + "," + please[i].GyroY + "," + please[i].GyroZ + "," + please[i].QuaternionX + "," + please[i].QuaternionY + "," + please[i].QuaternionZ + ",";
    //        outStream.WriteLine(str);
    //    }
    //    outStream.Close(); // 출력스트림 닫기
    //}
}

/* Input.gyro 클래스의 마라미터들
attitude - 장치의 attitude를 반환합니다. -> 자이로값 반환 가능
enabled	- 해당 자이로스코프(gyroscope)의 상태(status)를 설정하거나 얻어옵니다. -> 자이로 on / off
gravity	- 장치의 참조 프레임(reference frame)단위로 표현되는, 중력 가속도 벡터를 반환합니다.
rotationRate - 장치의 자이로스코프(gyroscope)에 의해 측정되는, 회전율을 반환합니다.
rotationRateUnbiased - 장치의 자이로스코프(gyroscope)에 의해 측정되는 수평 회전율을 반환합니다.
updateInterval - 자이로스코프(gyroscope)의 시간단위 간격(interval)을 설정하거나 받아옵니다.
userAcceleration - 사용자가 장치에 전달하는 가속도를 반환합니다.

        please = new List<data>();
        QtoA = new Dictionary<string, string>();
        // 매핑 데이터셋 가져오기
        var list = new List<Dictionary<string, string>>();  // 리스트 안에 string과 object 타입을 키, 벨류로 하는 딕셔너리를 생성
        FileInfo fi = new FileInfo(Application.dataPath + "/CSV/data.csv");                   // FileInfo : C# 파일 이름, 확장자, 크기(용량), 수정 일자, 속성 등 알아내는 클래스
        StreamReader sr = new StreamReader(fi.FullName);    // 텍스트 파일 읽기
        string strData = "";                                // strData를 빈 문자열으로 초기화함
        var strKey = sr.ReadLine().Split(',');              // 첫 줄을 ','로구분하여 Split한다.

        while ((strData = sr.ReadLine()) != null)           // 그 다음줄 부터 읽어서 strData에 저장한다. strData가 널이 아닐때 까지 실행
        {
            var strValue1 = strData.Split(',')[0];              // 한 줄 읽은 걸 ','로 구분하여 Split해서 strValue에 저장
            var strValue2 = strData.Split(',')[1];              // 한 줄 읽은 걸 ','로 구분하여 Split해서 strValue에 저장
            Dictionary<string, string> obj = new Dictionary<string, string>(); // obj라는 딕셔너리 생성 - 키는 string, value는 object 타입
            obj.Add(strValue1, strValue2);            // 키 : strkey[i], 값 : strValue[i]를 obj라는 딕셔너리에 추가하는 코드
            list.Add(obj);                                  // list는 리스트 안에 딕셔너리를 가지므로 list.Add(obj)로 추가 가능함
        }
        sr.Close();                                         // 텍스트 파일 닫기

        print("읽어온 데이터 개수 : " + list.Count);
        for(int i=0; i<list.Count; i++)
        {
            foreach(KeyValuePair<string, string> data in list[i])
            {
                // print("쿼터니언 : " + data.Key + " = 각도 : " + data.Value);

                var k = float.Parse(data.Key).ToString("N3");
                var v = float.Parse(data.Value).ToString("N3");

                if (!QtoA.ContainsKey(k))
                    QtoA.Add(k, v);
            }
        }

        foreach (KeyValuePair<string, string> data in QtoA)
        {
            print("쿼터니언 : " + data.Key + " = 각도 : " + data.Value);
        }


update
//if(QtoA.ContainsKey(Input.gyro.attitude.x.ToString("N3")))
        //{
        //    var xx = (float.Parse(QtoA[Input.gyro.attitude.x.ToString("N3")])).ToString("N3");
        //    t2.text = "스마트폰 - x : " + xx +"  y : " + sd_phone.ry.ToString("N0") + "  z : " + sd_phone.rz.ToString("N0");
        //}
            
        //else
        t2.text = "스마트폰 - x : " + "적절한 키없음 * " + Input.gyro.attitude.x.ToString("N3") + "  y : " + sd_phone.ry.ToString("N0") + "  z : " + sd_phone.rz.ToString("N0");
*/