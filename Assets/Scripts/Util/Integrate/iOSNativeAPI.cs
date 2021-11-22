using UnityEngine;
using System.Runtime.InteropServices;
using AppGame.UI;
using AppGame.Util;

public class iOSNativeAPI : MonoBehaviour
{
    public static iOSNativeAPI Instance { get; private set; }
    [SerializeField]
    private bool sendGameLoaded;
    private JsonUtil jsonUtil { get; set; }

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
        this.jsonUtil = new JsonUtil();
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
        string jsonString = this.MessageToJson(message);
        NativeAPI.sendMessageToMobileApp(jsonString);
#endif
        Debug.LogFormat("<><iOSNativeAPI.SendMessageToiOS>message: {0}", message);
    }

    public void ReceiveMessageFromiOS(string message)
    {
        iOSNativeMessage nativeMessage = this.jsonUtil.String2Json<iOSNativeMessage>(message);
        if (nativeMessage != null)
        {
            AppGame.Module.Cycling.CyclingView.PlayerName = nativeMessage.childSN;
        }
        else
        {
            Debug.LogError("<><iOSNativeAPI.ReceiveMessageFromiOS>Error: invalid message[type error]");
        }

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
        string jsonString = this.MessageToJson("open_web_page", url);
        NativeAPI.sendMessageToMobileApp(jsonString);
#endif
        Debug.LogFormat("<><iOSNativeAPI.OpenWebPage>open web page: {0}", url);
    }

    public string MessageToJson(string type, string content = "")
    {
        UnityMessage unityMessage = new UnityMessage() { type = type, content = content };
        string jsonString = this.jsonUtil.Json2String(unityMessage);
        Debug.LogFormat("<><iOSNativeAPI.MessageToJson>jsonString: {0}", jsonString);
        return jsonString;
    }
}

public class UnityMessage
{
    public string type { get; set; }
    public string content { get; set; }
}

public class iOSNativeMessage
{
    public string childSN { get; set; }
    public string token { get; set; }
}
