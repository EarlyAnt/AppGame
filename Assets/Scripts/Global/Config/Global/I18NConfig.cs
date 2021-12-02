using Mono.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Text;
using UnityEngine;

namespace AppGame.Config
{
    /// <summary>
    /// 多语言资源配置读取类
    /// </summary>
    public class I18NConfig : BaseConfig, II18NConfig
    {
        /************************************************自  定  义  类************************************************/
        [Inject]
        public ILanConfig LanConfig { get; set; }
        //[Inject]
        //public IGameDataHelper GameDataHelper { get; set; }//游戏数据管理器
        /************************************************属性与变量命名************************************************/
        //多语言资源配置数据字典
        private Dictionary<string, ResConfig> configs = new Dictionary<string, ResConfig>();
        /************************************************私  有  方  法************************************************/
        //读取语言配置文件
        private void ReadConfig(WWW www)
        {
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogErrorFormat("<><I18NConfig.ReadConfig>Error: {0}" + www.error);
                return;
            }

            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                try
                {
                    SecurityParser xmlDoc = new SecurityParser();
                    //Debug.LogFormat("<><I18NConfig.ReadConfig>Content: {0}", www.text);

                    xmlDoc.LoadXml(www.text);
                    ArrayList allNodes = xmlDoc.ToXml().Children;
                    foreach (SecurityElement seResConfigs in allNodes)
                    {//根节点
                        if (seResConfigs.Tag == "ResConfigs")
                        {//ResConfigs节点
                            ArrayList resConfigNodes = seResConfigs.Children;
                            foreach (SecurityElement seResConfig in resConfigNodes)
                            {//ResConfig节点
                                if (seResConfig.Tag == "ResConfig")
                                {
                                    string language = seResConfig.Attribute("Language");
                                    if (this.LanConfig.IsValid(language))
                                    {
                                        ResConfig resConfig = new ResConfig() { Language = language };
                                        ArrayList elementNodes = seResConfig.Children;
                                        foreach (SecurityElement seElement in elementNodes)
                                        {
                                            if (seElement.Tag == "Text")
                                            {//TextElement节点
                                                TextElement textElement = new TextElement() { Key = seElement.Attribute("Key"), Value = seElement.Attribute("Value").Replace("\\n", "\n") };
                                                resConfig.TextElements.Add(textElement);
                                            }
                                            else if (seElement.Tag == "Image")
                                            {//ImageElement节点
                                                ImageElement imageElement = new ImageElement() { Key = seElement.Attribute("Key"), Value = seElement.Attribute("Value") };
                                                resConfig.ImageElements.Add(imageElement);
                                            }
                                            else if (seElement.Tag == "Animation")
                                            {//AnimationElement节点
                                                AnimationElement animationElement = new AnimationElement() { Key = seElement.Attribute("Key"), Value = seElement.Attribute("Value") };
                                                resConfig.AnimationElements.Add(animationElement);
                                            }
                                            else if (seElement.Tag == "Audio")
                                            {//AudioElement节点
                                                AudioElement audioElement = new AudioElement() { Key = seElement.Attribute("Key"), Value = seElement.Attribute("Value") };
                                                resConfig.AudioElements.Add(audioElement);
                                            }
                                        }
                                        this.configs.Add(language, resConfig);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("<><I18NConfig.ReadConfig>Error: {0}", ex.Message);
                }
            }
            Debug.Log("<><I18NConfig.ReadConfig>Load complete");
            this.isLoaded = true;
        }
        /************************************************公  共  方  法************************************************/
        /// <summary>
        /// 加载资源配置数据
        /// </summary>
        public void Load()
        {
            if (this.isLoaded)
                return;
            this.configs.Clear();
            ConfigLoader.Instance.LoadConfig("Config/Global/I18N.xml", this.ReadConfig);
        }
        /// <summary>
        /// 获取所有资源配置
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ResConfig> GetAllResConfig()
        {
            return this.configs;
        }
        /// <summary>
        /// 获取指定语言资源配置
        /// </summary>
        /// <param name="languageShortName">语言简称</param>
        /// <returns></returns>
        public ResConfig GetResConfig(string languageShortName)
        {
            return this.configs != null && this.configs.ContainsKey(languageShortName) ? this.configs[languageShortName] : null;
        }
        /// <summary>
        /// 获取文本
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string GetText(string key)
        {
            string languageShortName = this.GetLanguage();
            if (this.configs != null && this.configs.ContainsKey(languageShortName) &&
                this.configs[languageShortName].TextElements != null)
            {
                TextElement textElement = this.configs[languageShortName].TextElements.Find(t => t.Key == key);
                return textElement != null ? textElement.Text : null;
            }
            else return null;
        }
        /// <summary>
        /// 获取文本配置
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public TextElement GetTextElement(string key)
        {
            string languageShortName = this.GetLanguage();
            if (this.configs != null && this.configs.ContainsKey(languageShortName) &&
                this.configs[languageShortName].TextElements != null)
            {
                TextElement textElement = this.configs[languageShortName].TextElements.Find(t => t.Key == key);
                return textElement != null ? textElement : new TextElement() { Key = key, Value = "" };
            }
            else return null;
        }
        /// <summary>
        /// 获取图片文件路径
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="languageShortName">指定语言</param>
        /// <returns></returns>
        public string GetImagePath(string key, string languageShortName = "")
        {
            if (string.IsNullOrEmpty(languageShortName))
                languageShortName = this.GetLanguage();

            string defaultPath = string.Format("CHS/{0}", key);
            if (this.configs != null && this.configs.ContainsKey(languageShortName) &&
                this.configs[languageShortName].ImageElements != null)
            {
                ImageElement imageElement = this.configs[languageShortName].ImageElements.Find(t => t.Key == key);
                Debug.LogFormat("<><I18NConfig.GetImagePath>Key: {0}, FoundPath: {1}", key, imageElement != null ? imageElement.Value : defaultPath);
                return imageElement != null ? imageElement.Value : defaultPath;
            }
            else
            {
                Debug.LogFormat("<><I18NConfig.GetImagePath>Key: {0}, DefaultPath: {1}", key, defaultPath);
                return defaultPath;
            }
        }
        /// <summary>
        /// 获取图片文件所有语言的路径
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public List<string> GetImagePaths(string key)
        {
            List<string> languages = this.LanConfig.GetAllLanShortName();
            List<string> paths = new List<string>();
            for (int i = 0; i < languages.Count; i++)
            {
                string path = this.GetImagePath(key, languages[i]);
                if (!string.IsNullOrEmpty(path) && !paths.Contains(path))
                    paths.Add(path);
            }
            return paths;
        }
        /// <summary>
        /// 获取Spine动画中的动画名称
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string GetAnimationName(string key)
        {
            string languageShortName = this.GetLanguage();
            if (this.configs != null && this.configs.ContainsKey(languageShortName) &&
                this.configs[languageShortName].AnimationElements != null)
            {
                AnimationElement animationElement = this.configs[languageShortName].AnimationElements.Find(t => t.Key == key);
                Debug.LogFormat("<><I18NConfig.GetAnimationName>Key: {0}, FoundAnimationName: {1}", key, animationElement != null ? animationElement.Value : key);
                return animationElement != null ? animationElement.Value : key;
            }
            else
            {
                Debug.LogFormat("<><I18NConfig.GetAnimationName>Key: {0}, DefaultAnimationName: {1}", key, key);
                return key;
            }
        }
        /// <summary>
        /// 获取音频文件路径
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="languageShortName">指定语言</param>
        /// <returns></returns>
        public string GetAudioPath(string key, string languageShortName = "")
        {
            if (string.IsNullOrEmpty(languageShortName))
                languageShortName = this.GetLanguage();

            //this.PrintAllData(languageShortName);
            string defaultPath = string.Format("CHS/{0}", key);
            if (this.configs != null && this.configs.ContainsKey(languageShortName) &&
                this.configs[languageShortName].AudioElements != null)
            {
                AudioElement audioElement = this.configs[languageShortName].AudioElements.Find(t => t.Key == key);
                Debug.LogFormat("<><I18NConfig.GetAudioPath>Key: {0}, FoundPath: {1}", key, audioElement != null ? audioElement.Value : defaultPath);
                return audioElement != null ? audioElement.Value : defaultPath;
            }
            else
            {
                Debug.LogFormat("<><I18NConfig.GetAudioPath>Key: {0}, DefaultPath: {1}", key, defaultPath);
                return defaultPath;
            }
        }
        /// <summary>
        /// 获取音频文件所有语言的路径
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public List<string> GetAudioPaths(string key)
        {
            List<string> languages = this.LanConfig.GetAllLanShortName();
            List<string> paths = new List<string>();
            for (int i = 0; i < languages.Count; i++)
            {
                string path = this.GetAudioPath(key, languages[i]);
                if (!string.IsNullOrEmpty(path) && !paths.Contains(path))
                    paths.Add(path);
            }
            return paths;
        }
        /// <summary>
        /// 打印指定语言的所有配置数据
        /// </summary>
        /// <param name="languageShortName">语言简称</param>
        public void PrintAllData(string languageShortName)
        {
            if (this.configs != null && this.configs.ContainsKey(languageShortName))
            {
                StringBuilder strbDatas = new StringBuilder();
                foreach (var kvp in this.configs[languageShortName].AudioElements)
                {
                    strbDatas.AppendFormat("[Key: {0}, Value: {1}], \n", kvp.Key, kvp.Value);
                }
                Debug.LogFormat("<><I18NConfig.PrintAllData>\n{0}", strbDatas.ToString());
            }
        }
        /// <summary>
        /// 获取本地记在的语言
        /// </summary>
        /// <returns></returns>
        private string GetLanguage()
        {
            //return this.GameDataHelper.GetObject<string>("Language");
            return AppGame.Config.LanConfig.Languages.ChineseSimplified;
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
