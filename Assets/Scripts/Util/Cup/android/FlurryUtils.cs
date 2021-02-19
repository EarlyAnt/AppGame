using UnityEngine;

namespace Cup.Utils.android
{
    public class FlurryUtils : IFlurryUtils
    {

        AndroidJavaClass nativeFlurryUtils;

        private bool isInit = false;

        public FlurryUtils()
        {
            nativeFlurryUtils = new AndroidJavaClass("com.bowhead.hank.utils.FlurryUtils");
        }

        public void init (){
            if(isInit){
                return;
            }

            AndroidJavaObject context = AndroidContextHolder.GetAndroidContext();
            nativeFlurryUtils.CallStatic("init",context);
            isInit = true;
        }

        public void onEvent(string eventId)
        {

            if(!isInit){
                Debug.LogWarning("Flurry not init"+eventId);
                return;
            }

            nativeFlurryUtils.CallStatic("onEvent", eventId);
            //Debug.LogFormat("{0} ---->> FlurryUtils.onEvent: {1}",
            //                System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), eventId);
        }

        public void onEvent(string eventId, string paramValue)
        {
            if(!isInit){
                Debug.LogWarning("Flurry not init"+eventId);
                return;
            }

            nativeFlurryUtils.CallStatic("onEvent", eventId, paramValue);
            Debug.LogFormat("{0} ---->> FlurryUtils.onEvent: {1}, {2}",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), eventId, paramValue);
        }

        public void onEvent(string eventId, string paramKey, string paramValue)
        {
            if(!isInit){
                Debug.LogWarning("Flurry not init"+eventId);
                return;
            }

            nativeFlurryUtils.CallStatic("onEvent", eventId, paramKey, paramValue);
            Debug.LogFormat("{0} ---->> FlurryUtils.onEvent: {1}, {2}, {3}",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), eventId, paramKey, paramValue);
        }

        public void setUserId(string userId)
        {
            if(!isInit){
                Debug.LogWarning("Flurry not init"+userId);
                return;
            }

            nativeFlurryUtils.CallStatic("setUserId", userId);
            Debug.LogFormat("{0} ---->> FlurryUtils.setUserId: {1}",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), userId);
        }

        public void logTimeEvent(string eventId, string paramValue)
        {
            if(!isInit){
                Debug.LogWarning("Flurry not init"+eventId);
                return;
            }

            nativeFlurryUtils.CallStatic("logTimeEvent", eventId, paramValue);
            Debug.LogFormat("{0} ---->> FlurryUtils.logTimeEvent: {1}, {2}",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), eventId, paramValue);
        }

        public void endTimedEvent(string eventId)
        {
            if(!isInit){
                Debug.LogWarning("Flurry not init"+eventId);
                return;
            }

            nativeFlurryUtils.CallStatic("endTimedEvent", eventId);
            Debug.LogFormat("{0} ---->> FlurryUtils.endTimedEvent: {1}",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), eventId);
        }

        public void setAge(int age)
        {
            if(!isInit){
                Debug.LogWarning("Flurry not init"+age);
                return;
            }

            nativeFlurryUtils.CallStatic("setAge", age);
            Debug.LogFormat("{0} ---->> FlurryUtils.setAge: {1}",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), age);
        }

        public void setGender(bool isMan)
        {
            if(!isInit){
                Debug.LogWarning("Flurry not init"+isMan);
                return;
            }

            nativeFlurryUtils.CallStatic("setGender", isMan);
            Debug.LogFormat("{0} ---->> FlurryUtils.setGender: {1}",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), isMan);
        }
    }
}