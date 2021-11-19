using UnityEngine;
using System.Runtime.InteropServices;

public class iOSNativeAPI : MonoBehaviour
{
    public static iOSNativeAPI Instance { get; private set; }
    [SerializeField]
    private bool sendGameLoaded;

    private class NativeAPI
    {
#if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        public static extern void sendMessageToMobileApp(string message);
#endif
    }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        //Debug.Log("<><iOSNativeAPI.Start>version 2020-11-19 13:10:00");
        if (this.sendGameLoaded)
        {
            this.SendMessageToiOS("game_loaded");
        }
    }

    public void SendMessageToiOS(string message)
    {
#if UNITY_IOS && !UNITY_EDITOR
        NativeAPI.sendMessageToMobileApp(message);
#endif
        Debug.LogFormat("<><iOSNativeAPI.SendMessageToiOS>message: {0}", message);
    }

    public void ReceiveMessageFromiOS(string message)
    {
        AppGame.Module.Cycling.CyclingView.PlayerName = message;
        Debug.Log("<><iOSNativeAPI.ReceiveMessageFromiOS>message: " + message);
    }

    public void GoBack()
    {
        this.SendMessageToiOS("exit_game");
        Application.Unload();
    }

    public void OpenWebPage(string url)
    {
#if UNITY_IOS && !UNITY_EDITOR
        NativeAPI.sendMessageToMobileApp(url);
#endif
        Debug.LogFormat("<><iOSNativeAPI.OpenWebPage>open web page: {0}", url);
    }
}
