using UnityEngine;

namespace CupSdk.Screen
{
    public class ScreenStatusListener : AndroidJavaProxy
    {
        public ScreenStatusListener() : base("com.bowhead.hank.utils.ScreenReceiver$ScreenStatusListener"){}

        public void onStatus(string status){
            if(mListener != null){
                Loom.QueueOnMainThread(()=>{
                    mListener(status);
                });
            }
        }

        private ScreenStatus mListener;

        public void setListener(ScreenStatus listener){
            mListener = listener;
        }
        
    }

    public delegate void ScreenStatus(string status);
}