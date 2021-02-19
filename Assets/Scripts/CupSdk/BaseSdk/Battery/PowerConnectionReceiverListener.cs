using UnityEngine;

namespace CupSdk.BaseSdk.Battery
{
    public class PowerConnectionReceiverListener: AndroidJavaProxy{
        public PowerConnectionReceiverListener() : base("com.bowhead.sheldon.battery.PowerConnectionReceiver$Listener") {}

        public void onConnected() {
            if(mConnected != null){
                Loom.QueueOnMainThread(()=>{
                    mConnected();
                });
            }
        }
        public void onDisconnected() {
            if(mDisconnected != null){
                Loom.QueueOnMainThread(()=>{
                    mDisconnected();
                });
            }
        }
        private Connected mConnected;
        private Disconnected mDisconnected;
        public void setCallBack(Connected connected,Disconnected disconnected)
        {
            mConnected = connected;
            mDisconnected = disconnected;
        }
    }


    public delegate void Connected();

    public delegate void Disconnected();
}