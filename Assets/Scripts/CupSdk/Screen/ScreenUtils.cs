using UnityEngine;

namespace CupSdk.Screen
{
    public class ScreenUtils
    {
        private static AndroidJavaClass javaScreenUtilsClass = new AndroidJavaClass("com.bowhead.hank.utils.ScreenUtils");
        public static void StartScreenListener(AndroidJavaObject context,ScreenStatusListener listener){
            javaScreenUtilsClass.CallStatic("StartScreenListener",context,listener);
        }

        public static void TurnOnScreen(AndroidJavaObject context){
            javaScreenUtilsClass.CallStatic("turnOnScreen",context);
        }
    }
}