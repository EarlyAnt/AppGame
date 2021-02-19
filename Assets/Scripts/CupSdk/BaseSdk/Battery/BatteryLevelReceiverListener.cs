using UnityEngine;

namespace CupSdk.BaseSdk.Battery
{
    public class BatteryLevelReceiverListener : AndroidJavaProxy
    {
        private BatteryLow mBatteryLow;
        private BatteryOkay mBatteryOkay;
        private BatteryLevelChange mBatteryLevelChange;
        private BatteryLevelChange mBatteryLevelChangeUnsafe;

        public BatteryLevelReceiverListener() : base("com.bowhead.sheldon.battery.BatteryLevelReceiver$Listener") { }

        public void onBatteryLow()
        {
            if (mBatteryLow != null)
            {
                Loom.QueueOnMainThread(() =>
                {
                    mBatteryLow();
                });
            }
        }

        public void onBatteryOkay()
        {
            if (mBatteryOkay != null)
            {
                Loom.QueueOnMainThread(() =>
                {
                    mBatteryOkay();
                });

            }
        }

        public void onBatteryLevelChange(int batteryCapacity)
        {
            if (mBatteryLevelChange != null)
            {
                Debug.Log(string.Format("----BatteryLevelReceiverListener.onBatteryLevelChange----Battery: {0}", batteryCapacity));
                mBatteryLevelChange(batteryCapacity);
            }
        }

        public void setCallBack(BatteryLow batteryLow, BatteryOkay batteryOkay, BatteryLevelChange batteryLevelChange)
        {
            mBatteryOkay = batteryOkay;
            mBatteryLow = batteryLow;
            mBatteryLevelChange = batteryLevelChange;
        }
    }

    public delegate void BatteryLow();

    public delegate void BatteryOkay();

    public delegate void BatteryLevelChange(int batteryCapacity);
}