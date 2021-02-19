using UnityEngine;

namespace Cup.Utils.android
{
    public class LogUtils : ILogUtils
    {
        AndroidJavaObject nativeLogUtils;
        public LogUtils(){
            nativeLogUtils = new AndroidJavaObject("com.bowhead.hank.utils.LogUtils");
        }
        public void d(string tag, string log)
        {
            nativeLogUtils.Call("d",tag,log);
        }

        public void e(string tag, string log)
        {
            nativeLogUtils.Call("e",tag,log);
        }

        public void i(string tag, string log)
        {
            nativeLogUtils.Call("i",tag,log);
        }

        public void v(string tag, string log)
        {
            nativeLogUtils.Call("v",tag,log);
        }

        public void w(string tag, string log)
        {
            nativeLogUtils.Call("w",tag,log);
        }

        public void wtf(string tag, string log)
        {
            nativeLogUtils.Call("wtf",tag,log);
        }
    }
}