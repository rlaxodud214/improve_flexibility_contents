using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace LpmsCSharpWrapper
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SensorData
    {
        public int openMatId;
        public float ax, ay, az;
        public float gx, gy, gz;
        public float bx, by, bz;
        public float wx, wy, wz;
        public float rx, ry, rz;
        public float qw, qx, qy, qz;
        public float rM0, rM1, rM2, rM3, rM4, rM5, rM6, rM7, rM8;           // rotation Matrix
        public float rOM0, rOM1, rOM2, rOM3, rOM4, rOM5, rOM6, rOM7, rOM8;  //rotation Offset Matrix

        public float aRawX, aRawY, aRawZ;
        public float gRawX, gRawY, gRawZ;
        public float bRawX, bRawY, bRawZ;

        public float pressure;
        public int frameCount;
        public float linAccX, linAccY, linAccZ;
        public float gTemp;
        public float altitude;
        public float temperature;
        public double timeStamp;
    }

    public class LpSensorManager
    {
        // Sensor Type
        public static int DEVICE_LPMS_B = 0; // LPMS-B (Bluetooth)
        public static int DEVICE_LPMS_U = 1; // LPMS-CU / LPMS-USBAL (USB)
        public static int DEVICE_LPMS_C = 2; // LPMS-CU / LPMS-CANAL(CAN bus)
        public static int DEVICE_LPMS_BLE = 3; // LPMS-BLE (Bluetooth low energy)
        public static int DEVICE_LPMS_RS232 = 4; // LPMS-UARTAL (RS-232)
        public static int DEVICE_LPMS_B2 = 5; // LPMS-B2 (Bluetooth)
        public static int DEVICE_LPMS_U2 = 6; // LPMS-CU2/URS2/UTTL2/USBAL2 (USB)
        public static int DEVICE_LPMS_C2 = 7; // LPMS-CU2/CANAL2 (CAN)

        // Serial baudrate
        // For communication with LPMS sensor via serial connection:
        // LPMS-URS2
        // LPMS-UTTL2
        // LPMS-RS232AL
        // LPMS-RS232AL2
        // LPMS-CURS (RS232)
        public static int LPMS_UART_BAUDRATE_19200 = 0;
        public static int LPMS_UART_BAUDRATE_38400 = 1;
        public static int LPMS_UART_BAUDRATE_57600 = 2;
        public static int LPMS_UART_BAUDRATE_115200 = 3;
        public static int LPMS_UART_BAUDRATE_230400 = 4;
        public static int LPMS_UART_BAUDRATE_256000 = 5;
        public static int LPMS_UART_BAUDRATE_460800 = 6;
        public static int LPMS_UART_BAUDRATE_921600 = 7;


        // Sensor Connection status
        public static int SENSOR_CONNECTION_CONNECTED = 1;
        public static int SENSOR_CONNECTION_CONNECTING = 2;
        public static int SENSOR_CONNECTION_FAILED = 3;
        public static int SENSOR_CONNECTION_INTERRUPTED = 4;

        // Offset reset mode
        public static int LPMS_OFFSET_MODE_OBJECT = 0;
        public static int LPMS_OFFSET_MODE_HEADING = 1;
        public static int LPMS_OFFSET_MODE_ALIGNMENT = 2;

        // Initialize sensor manager
        [DllImport("LpSensorCWrapper")]
        public static extern void initSensorManager();

        // Deinit sensor manager: free up memory to prevent memory leak
        [DllImport("LpSensorCWrapper")]
        public static extern void deinitSensorManager();

        // Set serial baudrate for connection to sensor via serial interface
        [DllImport("LpSensorCWrapper")]
        public static extern void setSerialBaudrate(int baudrate);

        // Connects to Lpms sensor <sensorType> with address <deviceId>
        [DllImport("LpSensorCWrapper")]
        public static extern void connectToLpms(int sensorType,  String deviceId);

        // Connects to LpmsB2 with address <deviceId>
        [DllImport("LpSensorCWrapper")]
        public static extern void connectToLpmsB2(String deviceId);

        // Get Sensor Connection status:
        // returns <Sensor Connection status>
        [DllImport("LpSensorCWrapper")]
        public static extern int getConnectionStatus(String deviceId);

        // Gets sensor data for sensor <deviceId>
        // Returns a pointer to SensorData
        // Usage eg:
        // SensorData sd;
        // unsafe
        // {
        //     sd = *((SensorData*)LpSensorManager.getSensorData(deviceId));
        // }
        [DllImport("LpSensorCWrapper")]
        public static extern IntPtr getSensorData(String deviceId);

        // Sets offset for sensor <deviceId> 
        [DllImport("LpSensorCWrapper")]
        public static extern void setOrientationOffset(String deviceId, int offsetMode);

        // Resets offset for sensor <deviceId> 
        [DllImport("LpSensorCWrapper")]
        public static extern void resetOrientationOffset(String deviceId);

        // Synchronize multiple sensors
        [DllImport("LpSensorCWrapper")]
        public static extern void syncSensors();

        // Disconnect sensor with address deviceId
        [DllImport("LpSensorCWrapper")]
        public static extern void disconnectLpms(String deviceId);

    }
}
