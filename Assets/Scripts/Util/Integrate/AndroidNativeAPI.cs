using UnityEngine;

public class AndroidNativeAPI : SingletonMonoBehaviour<AndroidNativeAPI>
{
    private static AndroidJavaObject androidJavaClass = null;
    private static AndroidJavaObject androidJavaObject;

    private void Start()
    {
        Debug.Log("<><AndroidNativeAPI.Start>version 2020-11-03 09:30:00");
    }

    public void SendMessageToAndroid(string message)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (androidJavaClass == null || androidJavaObject == null)
        {
            androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityExtActivity");
            androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
        }

        if (androidJavaObject != null)
        {
            androidJavaObject.Call("CallUnityMethod", message);
            Debug.LogFormat("<><AndroidNativeAPI.SendMessageToAndroid>execute in android, message: {0}", message);
        }
        Debug.LogFormat("<><AndroidNativeAPI.SendMessageToAndroid>run in android, message: {0}", message);
#else
        Debug.LogFormat("<><AndroidNativeAPI.SendMessageToAndroid>run in editor, message: {0}", message);
#endif
    }

    public void UnityMethod(string str)
    {
        Debug.Log("Android: " + str);
    }

    public void GoBack()
    {
        this.SendMessageToAndroid("goback");
    }
}
