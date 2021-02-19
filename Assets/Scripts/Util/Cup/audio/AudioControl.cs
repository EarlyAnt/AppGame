using Cup.Utils.android;
using UnityEngine;

namespace Cup.Utils.audio
{
    public class AudioControl
    {
        public static AndroidJavaClass nativeAudioControl;
        public static int getVolume(){
            AndroidJavaObject context = AndroidContextHolder.GetAndroidContext();

            if(nativeAudioControl == null){
                nativeAudioControl = new AndroidJavaClass("com.bowhead.hank.utils.AudioControl");
            }

            return nativeAudioControl.CallStatic<int>("getVolume",context);
        }

        public static bool setVolume(int vol){
            AndroidJavaObject context = AndroidContextHolder.GetAndroidContext();

            if(nativeAudioControl == null){
                nativeAudioControl = new AndroidJavaClass("com.bowhead.hank.utils.AudioControl");
            }

            return nativeAudioControl.CallStatic<bool>("setVolume",context,vol);
        }

        public static int getMaxVolume(){
           AndroidJavaObject context = AndroidContextHolder.GetAndroidContext();

            if(nativeAudioControl == null){
                nativeAudioControl = new AndroidJavaClass("com.bowhead.hank.utils.AudioControl");
            }

            return nativeAudioControl.CallStatic<int>("getMaxVolume",context);
        }
    }
}