using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Wizcorp.Web
{
    public class WebView : MonoBehaviour
    {
        [SerializeField]
        private string url = "https://wizcorp.jp";
        [SerializeField]
        private Text context;

        public string Url
        {
            get { return this.url; }
            set { this.url = value; }
        }

        #region shared
        public void CallBack(string message)
        {
            if (this.context != null)
                context.text = message;
        }
        #endregion

#if UNITY_ANDROID
        public void CallWebView()
        {
            if (string.IsNullOrEmpty(this.url))
                return;

            Debug.LogFormat("Call WebView: " + this.url);
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call("OpenWebView", url);
        }

        void Start()
        {
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call("SetupCallBack", this.gameObject.name, "CallBack", "Calling back from Android");
        }
#endif

#if UNITY_IOS
	[DllImport("__Internal")]
	private static extern void _nativeLog();
	[DllImport("__Internal")]
	private static extern void _openURL(string url);
	[DllImport("__Internal")]
	private static extern void _setupCallBack(string gameObject, string methodName);

	// Connect with button onClick event
	public void CallWebView()
	{
		_openURL(URL);
	}

	void Start()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_setupCallBack(this.gameObject.name, "CallBack");

			_nativeLog();
		}
	}

#endif
    }
}