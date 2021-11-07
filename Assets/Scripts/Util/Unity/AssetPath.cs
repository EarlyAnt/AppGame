using UnityEngine;
using System.Collections;
#if UNITY_IPHONE
using UnityEngine.iOS;
#endif

namespace AppGame.Util
{
    public static class AssetPath
    {
        public static string SteamingAssetsPath
        {
            get
            {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
                return "file:///" + Application.streamingAssetsPath + "/";
#elif UNITY_IPHONE
		        return "file://" +  Application.streamingAssetsPath + "/";
#elif UNITY_ANDROID
                return  Application.streamingAssetsPath + "/";
#else
                return string.Empty;
#endif
            }
        }

        public static string PersistentAssetsPath
        {
            get
            {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                return "file:///" + Application.persistentDataPath + "/";
#else
                return "file://" + Application.persistentDataPath + "/";
#endif
            }
        }

        public static string GetResourcePath(bool hotFix = false)
        {
            if (hotFix)
            {
                return PersistentAssetsPath;
            }
            else
            {
                return SteamingAssetsPath;
            }
        }
    }
}

