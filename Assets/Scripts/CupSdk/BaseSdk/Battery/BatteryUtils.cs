using Cup.Utils.android;
using UnityEngine;

namespace CupSdk.BaseSdk.Battery
{
    public class BatteryUtils
    {
        private BatteryUtils()
        {

        }

        public static void init()
        {
            AndroidJavaObject context = AndroidContextHolder.GetAndroidContext();
            javaBatteryUtils = new AndroidJavaObject("com.bowhead.sheldon.battery.BatteryUtils", context);
        }

        private static AndroidJavaObject javaBatteryUtils;

        public static bool isCharging(AndroidJavaObject context)
        {
            return javaBatteryUtils.Call<bool>("isCharging");
        }

        public static int getBatteryStatus(AndroidJavaObject context)
        {
            return javaBatteryUtils.Call<int>("getBatteryStatus");
        }

        public static int getBatteryHealth(AndroidJavaObject context)
        {
            return javaBatteryUtils.Call<int>("getBatteryHealth");
        }

        public static bool isBatteryExist(AndroidJavaObject context)
        {
            return javaBatteryUtils.Call<bool>("isBatteryExist");
        }

        public static int getBatteryCapacity(AndroidJavaObject context)
        {
            return javaBatteryUtils.Call<int>("getBatteryCapacity");
        }

        public static int getBatteryPlugged(AndroidJavaObject context)
        {
            return javaBatteryUtils.Call<int>("getBatteryPlugged");
        }

        public static int getBatteryVoltage(AndroidJavaObject context)
        {
            return javaBatteryUtils.Call<int>("getBatteryVoltage");
        }

        public static int getBatteryTemperature(AndroidJavaObject context)
        {
            return javaBatteryUtils.Call<int>("getBatteryTemperature");
        }

        private static BatteryLevelReceiverListener mBatteryLevelReceiverListener;
        public static void registerBatteryLevelListener(BatteryLow batteryLow, BatteryOkay batteryOkay, BatteryLevelChange batteryLevelChange)
        {
            if (mBatteryLevelReceiverListener == null)
            {
                mBatteryLevelReceiverListener = new BatteryLevelReceiverListener();

                mBatteryLevelReceiverListener.setCallBack(() =>
                {
                    BatteryLowEvent();
                }, () =>
                {
                    BatteryOkayEvent();
                }, (batteryCapacity) =>
                {
                    BatteryLevelChangeEvent(batteryCapacity);
                });

                javaBatteryUtils.Call("registerBatteryLevelListener", mBatteryLevelReceiverListener);
            }

            if (batteryLow != null)
            {
                BatteryLowEvent += batteryLow;
            }

            if (batteryOkay != null)
            {
                BatteryOkayEvent += batteryOkay;
            }

            if (batteryLevelChange != null)
            {
                BatteryLevelChangeEvent += batteryLevelChange;
            }
        }
        public static void unRegisterBatteryLevelListener(BatteryLow batteryLow, BatteryOkay batteryOkay, BatteryLevelChange batteryLevelChange)
        {
            if (batteryLow != null)
            {
                BatteryLowEvent -= batteryLow;
            }

            if (batteryOkay != null)
            {
                BatteryOkayEvent -= batteryOkay;
            }

            if (batteryLevelChange != null)
            {
                BatteryLevelChangeEvent -= batteryLevelChange;
            }
        }

        private static PowerConnectionReceiverListener mPowerConnectionReceiverListener;
        public static void registerPowerConnectionListener(Connected connected, Disconnected disconnected)
        {

            if (mPowerConnectionReceiverListener == null)
            {
                mPowerConnectionReceiverListener = new PowerConnectionReceiverListener();
                mPowerConnectionReceiverListener.setCallBack(() =>
                {
                    ConnectedEvent();
                }, () =>
                {
                    DisconnectedEvent();
                });

                javaBatteryUtils.Call("registerPowerConnectionListener", mPowerConnectionReceiverListener);
            }

            if (connected != null)
            {
                ConnectedEvent += connected;
            }

            if (disconnected != null)
            {
                DisconnectedEvent += disconnected;
            }


        }

        public static void unRegisterPowerConnectionListener(Connected connected, Disconnected disconnected)
        {

            if (connected != null)
            {
                ConnectedEvent -= connected;
            }

            if (disconnected != null)
            {
                DisconnectedEvent -= disconnected;
            }

        }

        private static event BatteryLow BatteryLowEvent = delegate () { };
        private static event BatteryOkay BatteryOkayEvent = delegate () { };
        private static event BatteryLevelChange BatteryLevelChangeEvent = delegate (int batteryCapacity) { };

        private static event Connected ConnectedEvent = delegate () { };

        private static event Disconnected DisconnectedEvent = delegate () { };

    }

}