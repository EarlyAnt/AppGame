using Mono.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;

namespace AppGame.Config
{
    /// <summary>
    /// 音频配置读取类
    /// </summary>
    public class AudioConfig : BaseConfig, IAudioConfig
    {
        /************************************************属性与变量命名************************************************/
        public static class StorageTypes
        {
            public const string LOCAL = "Local";
            public const string REMOTE = "Remote";
        }
        //音频配置数据字典
        private List<Audio> configs = new List<Audio>();
        /************************************************私  有  方  法************************************************/
        //读取音频配置文件
        private void ReadConfig(WWW www)
        {
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogErrorFormat("<><AudioConfig.ReadConfig>Error: {0}" + www.error);
                return;
            }

            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                try
                {
                    SecurityParser xmlDoc = new SecurityParser();
                    Debug.LogFormat("<><AudioConfig.ReadConfig>Content: {0}", www.text);

                    xmlDoc.LoadXml(www.text);
                    ArrayList allNodes = xmlDoc.ToXml().Children;
                    foreach (SecurityElement seAudios in allNodes)
                    {
                        if (seAudios.Tag == "Audios")
                        {
                            ArrayList audioNodes = seAudios.Children;
                            foreach (SecurityElement seAudio in audioNodes)
                            {
                                if (seAudio.Tag == "Audio")
                                {
                                    Audio audio = new Audio()
                                    {
                                        Name = seAudio.Attribute("Name"),
                                        Storage = seAudio.Attribute("Storage"),
                                        Des = seAudio.Attribute("Des")
                                    };
                                    ArrayList fileNodes = seAudio.Children;
                                    foreach (SecurityElement xeFile in fileNodes)
                                    {
                                        if (xeFile.Tag == "File")
                                        {
                                            audio.Files.Add(new AudioFile() { Path = xeFile.Attribute("Path") });
                                        }
                                    }
                                    this.configs.Add(audio);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("<><AudioConfig.ReadConfig>Error: {0}", ex.Message);
                }
            }
            Debug.Log("<><AudioConfig.ReadConfig>Load complete");
            this.isLoaded = true;
        }
        /************************************************公  共  方  法************************************************/
        /// <summary>
        /// 加载字体配置数据
        /// </summary>
        public void Load()
        {
            if (this.isLoaded)
                return;
            this.configs.Clear();
            ConfigLoader.Instance.LoadConfig("Config/Global/AudioConfig.xml", this.ReadConfig);
        }
        /// <summary>
        /// 获取所有音频
        /// </summary>
        /// <returns></returns>
        public List<Audio> GetAllAudios()
        {
            return this.configs;
        }
        /// <summary>
        /// 获取指定名称的音频
        /// </summary>
        /// <param name="name">音频名称</param>
        /// <returns></returns>
        public Audio GetAudio(string name)
        {
            if (this.configs != null)
                return this.configs.Find(t => t.Name == name);
            else
                return null;
        }
        /// <summary>
        /// 获取配置文件是否已经加载完
        /// </summary>
        /// <returns></returns>
        public bool IsLoaded()
        {
            return this.isLoaded;
        }
    }
}
