using AppGame.Util;
using System;
using System.Collections;
using UnityEngine;

namespace AppGame.Config
{
    public class ConfigLoader : Singleton<ConfigLoader>
    {
        private IEnumerator loadConfig(string configPath, Action<WWW> configReader)
        {
            string path = AssetPath.GetResourcePath() + configPath;
            WWW www = new WWW(path);
            yield return www;
            configReader(www);
        }

        public void LoadConfig(string configPath, Action<WWW> configReader)
        {
            StartCoroutine(loadConfig(configPath, configReader));
        }
    }
}
