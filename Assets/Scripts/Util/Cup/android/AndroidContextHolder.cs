using UnityEngine;

namespace Cup.Utils.android
{
    public sealed class AndroidContextHolder
    {
        private static AndroidJavaObject s_ActivityContext;
        public static AndroidJavaObject GetAndroidContext(){
            if( s_ActivityContext == null )
            {
                AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                if (activityClass != null)
                {
                    s_ActivityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
				}
			}
            return s_ActivityContext;
            
        }
    }
}