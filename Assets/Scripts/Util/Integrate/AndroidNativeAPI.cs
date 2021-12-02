using AppGame.Global;
using AppGame.UI;
using AppGame.Util;
using UnityEngine;

public class AndroidNativeAPI : BaseView
{
    public static AndroidNativeAPI Instance { get; private set; }
    public System.Action<string, string> OnReceiveGameData { get; set; }
    private static AndroidJavaObject androidJavaClass = null;
    private static AndroidJavaObject androidJavaObject;
    private JsonUtil jsonUtil { get; set; }
    [SerializeField]
    private bool sendGameLoaded;

    protected override void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.jsonUtil = new JsonUtil();
    }

    protected override void Start()
    {
        if (this.sendGameLoaded)
        {
            this.DelayInvoke(() => { this.SendMessageToAndroid("game_loaded"); }, 1f);
        }
    }

    public void SendMessageToAndroid(string action, string parameter = "")
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (androidJavaClass == null || androidJavaObject == null)
        {
            androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityExtActivity");
            androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
        }

        if (androidJavaObject != null)
        {
            androidJavaObject.Call("sendMessageToAndroid", action, parameter);
            Debug.LogFormat("<><AndroidNativeAPI.SendMessageToAndroid>execute in android, action: {0}, parameter : {1}", action, parameter);
        }
        Debug.LogFormat("<><AndroidNativeAPI.SendMessageToAndroid>run in android, action: {0}, parameter : {1}", action, parameter);
#else
        Debug.LogFormat("<><AndroidNativeAPI.SendMessageToAndroid>run in editor, action: {0}, parameter : {1}", action, parameter);
#endif
    }

    public void ReceiveMessageFromAndroid(string message)
    {
        Debug.Log("<><AndroidNativeAPI.ReceiveMessageFromAndroid>message: " + message);

        try
        {
            AndroidNativeMessage nativeMessage = this.jsonUtil.String2Json<AndroidNativeMessage>(message);
            if (nativeMessage != null)
            {
                GameData.ChildSn = nativeMessage.childSN;
                GameData.Token = nativeMessage.token;
                this.OnReceivedGameData(GameData.ChildSn, GameData.Token);
                Debug.LogFormat("<><AndroidNativeAPI.ReceiveMessageFromAndroid>childSN: {0}, token: {1}", GameData.ChildSn, GameData.Token);
            }
            else
            {
                Debug.LogError("<><AndroidNativeAPI.ReceiveMessageFromAndroid>Error: invalid message[type error]");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogErrorFormat("<><AndroidNativeAPI.ReceiveMessageFromAndroid>Error: {0}", ex.Message);
        }
    }

    public void GoBack()
    {
        this.SendMessageToAndroid("goback");
    }

    public void OpenWebPage(string url)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        this.SendMessageToAndroid("open_web_page", url);
        Debug.LogFormat("<><AndroidNativeAPI.OpenWebPage>open web page: {0}", url);
#endif
        Debug.LogFormat("<><AndroidNativeAPI.OpenWebPage>open web page: {0}", url);
    }

    private void OnReceivedGameData(string childSn, string token)
    {
        if (this.OnReceiveGameData != null)
        {
            this.OnReceiveGameData(childSn, token);
            Debug.LogFormat("<><AndroidNativeAPI.OnReceivedGameData>childSn: {0}, token: {1}", childSn, token);
        }
    }
}


