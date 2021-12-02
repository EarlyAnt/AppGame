using Mono.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

namespace AppGame.Config
{
    /// <summary>
    /// 语言配置读取类
    /// </summary>
    public class LanConfig : BaseConfig, ILanConfig
    {
        /************************************************自  定  义  类************************************************/
        /// <summary>
        /// 语言枚举
        /// </summary>
        public static class Languages
        {
            /// <summary>
            /// 中文简体
            /// </summary>
            public static string ChineseSimplified { get { return "CHS"; } }
            /// <summary>
            /// 中文繁体
            /// </summary>
            public static string ChineseTraditonal { get { return "CHT"; } }
            /// <summary>
            /// 英文
            /// </summary>
            public static string English { get { return "EN"; } }
            /// <summary>
            /// 日文
            /// </summary>
            public static string Japanese { get { return "JP"; } }
        }
        /************************************************属性与变量命名************************************************/
        //语言配置数据字典
        private Dictionary<string, Language> configs = new Dictionary<string, Language>();
        /************************************************私  有  方  法************************************************/
        //读取语言配置文件
        private void ReadConfig(WWW www)
        {
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogErrorFormat("<><LanConfig.ReadConfig>Error: {0}" + www.error);
                return;
            }

            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                try
                {
                    SecurityParser xmlDoc = new SecurityParser();
                    //Debug.LogFormat("<><LanConfig.ReadConfig>Content: {0}", www.text);

                    xmlDoc.LoadXml(www.text);
                    ArrayList allNodes = xmlDoc.ToXml().Children;
                    foreach (SecurityElement seLanguages in allNodes)
                    {
                        if (seLanguages.Tag == "Languages")
                        {
                            ArrayList languageNodes = seLanguages.Children;
                            foreach (SecurityElement seLanguage in languageNodes)
                            {
                                if (seLanguage.Tag == "Language")
                                {
                                    Language language = new Language()
                                    {
                                        Name = seLanguage.Attribute("Name"),
                                        Text = seLanguage.Attribute("Text"),
                                        Default = Convert.ToBoolean(seLanguage.Attribute("Default"))
                                    };
                                    this.configs.Add(language.Name, language);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("<><LanConfig.ReadConfig>Error: {0}", ex.Message);
                }
                Debug.Log("<><LanConfig.ReadConfig>Load complete");
                this.isLoaded = true;
            }
        }
        //获取语言实例
        private Language GetLanguage(string language)
        {
            if (this.configs != null && this.configs.Count > 0 &&
                !string.IsNullOrEmpty(language) && this.configs.ContainsKey(language))
            {
                return this.configs[language];
            }
            else return null;
        }
        /************************************************公  共  方  法************************************************/
        /// <summary>
        /// 加载语言配置数据
        /// </summary>
        public void Load()
        {
            if (this.isLoaded)
                return;
            this.configs.Clear();
            ConfigLoader.Instance.LoadConfig("Config/Global/LanConfig.xml", this.ReadConfig);
        }
        /// <summary>
        /// 获取语言显示文本
        /// </summary>
        /// <param name="languageShortName">语言简称(可在LanConfig.Languages类中得到枚举值</param>
        /// <returns></returns>
        public string GetLanText(string languageShortName)
        {
            Language language = this.GetLanguage(languageShortName);
            return language != null ? language.Text : null;
        }
        /// <summary>
        /// 判断指定语言是否为默认语言
        /// </summary>
        /// <param name="languageShortName">语言简称(可在LanConfig.Languages类中得到枚举值)</param>
        /// <returns></returns>
        public bool IsDefault(string languageShortName)
        {
            Language language = this.GetLanguage(languageShortName);
            return language != null ? language.Default : false;
        }
        /// <summary>
        /// 判断语言简称是否合法
        /// </summary>
        /// <param name="languageShortName"></param>
        /// <returns></returns>
        public bool IsValid(string languageShortName)
        {
            return this.configs != null && !string.IsNullOrEmpty(languageShortName) && this.configs.ContainsKey(languageShortName);
        }
        /// <summary>
        /// 获取所有语言的简称
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllLanShortName()
        {
            if (this.configs != null && this.configs.Count > 0)
            {
                List<string> allLanText = new List<string>();
                foreach (KeyValuePair<string, Language> kvp in this.configs)
                {
                    allLanText.Add(kvp.Value.Name);
                }
                return allLanText;
            }
            else return null;
        }
        /// <summary>
        /// 获取所有语言的显示文本
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllLanText()
        {
            if (this.configs != null && this.configs.Count > 0)
            {
                List<string> allLanText = new List<string>();
                foreach (KeyValuePair<string, Language> kvp in this.configs)
                {
                    allLanText.Add(kvp.Value.Text);
                }
                return allLanText;
            }
            else return null;
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
