using UnityEngine;

namespace Cup.Utils.android
{
    public class PowerManager
    {
        public static void reboot(){
            AndroidJavaObject context = AndroidContextHolder.GetAndroidContext();
            context.Call("reboot");
        }
    }
}