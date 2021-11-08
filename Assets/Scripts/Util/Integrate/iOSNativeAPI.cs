using UnityEngine;
using System.Runtime.InteropServices;

public class iOSNativeAPI : SingletonMonoBehaviour<iOSNativeAPI>
{
    private class NativeAPI
    {
        [DllImport("__Internal")]
        public static extern void sendMessageToMobileApp(string message);
    }

    private void Start()
    {
        Debug.Log("<><iOSNativeAPI.Start>version 2020-11-08 14:00:00");
    }

    public void SendMessageToiOS(string message)
    {
#if UNITY_IOS && !UNITY_EDITOR
        NativeAPI.sendMessageToMobileApp("goback");
#endif
        Application.Unload();
        Debug.LogFormat("<><iOSNativeAPI.SendMessageToiOS>message: {0}", message);
    }

    public void UnityMethod(string message)
    {
        AppGame.Module.Cycling.CyclingView.PlayerName = message;
        Debug.Log("<><iOSNativeAPI.UnityMethod>message: " + message);
    }

    public void GoBack()
    {
        this.SendMessageToiOS("goback");
    }
}
