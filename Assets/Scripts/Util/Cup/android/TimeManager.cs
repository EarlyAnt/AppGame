using UnityEngine;

namespace Cup.Utils.android
{
    public class TimeManager : ITimeManager
    {
        public AndroidJavaObject nativeTimeManager;

        public void setServerTime(long millisTime)
        {
            if(nativeTimeManager == null){
                nativeTimeManager = new AndroidJavaObject("com.bowhead.hank.utils.TimeManager");
            }
            Debug.Log("millisTime:"+millisTime);
            nativeTimeManager.Call("setServerTime",AndroidContextHolder.GetAndroidContext(),millisTime);
        }

        public void setTimeZone(string timeZone)
        {
            if(nativeTimeManager == null){
                nativeTimeManager = new AndroidJavaObject("com.bowhead.hank.utils.TimeManager");
            }

            nativeTimeManager.Call("setTimeZone",AndroidContextHolder.GetAndroidContext(),timeZone);
        }
    }
}