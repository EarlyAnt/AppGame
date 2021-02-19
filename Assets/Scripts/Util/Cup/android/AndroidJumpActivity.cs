using UnityEngine;

namespace Cup.Utils.android
{
    public class AndroidJumpActivity : IAndroidJumpActivity
    {
        public void jumpToTestFacotry()
        {
            AndroidJavaObject context = AndroidContextHolder.GetAndroidContext();
            context.Call("jumpToAppByPackage","com.bowhead.darla");
        }
    }
}