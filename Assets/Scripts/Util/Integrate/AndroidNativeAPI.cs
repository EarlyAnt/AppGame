using UnityEngine;

public class AndroidNativeAPI : SingletonMonoBehaviour<AndroidNativeAPI>
{
    private static AndroidJavaObject androidJavaClass = null;
    private static AndroidJavaObject androidJavaObject;

    private void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        //androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");

        androidJavaClass = new AndroidJavaClass("com.unity3d.player.TestUnityActivity");
        androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");

        Debug.Log("<><AndroidNativeAPI.Start>version001");
#endif
        Debug.Log("<><AndroidNativeAPI.Start>version001");
    }

    public void SendMessageToAndroid(string message)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (androidJavaClass == null || androidJavaObject == null)
        {
            androidJavaClass = new AndroidJavaClass("com.unity3d.player.TestUnityActivity");
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
}
