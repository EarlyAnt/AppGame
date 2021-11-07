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
    /// 字体配置读取类
    /// </summary>
    public class FontConfig : BaseConfig, IFontConfig
    {
        /************************************************属性与变量命名************************************************/
        public const string DEFAULT_FONT = "Arial";
        //字体配置数据字典
        private Dictionary<string, string> configs = new Dictionary<string, string>();
        /************************************************私  有  方  法************************************************/
        //读取字体配置文件
        private void ReadConfig(WWW www)
        {
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogErrorFormat("<><FontConfig.ReadConfig>Error: {0}" + www.error);
                return;
            }

            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                try
                {
                    SecurityParser xmlDoc = new SecurityParser();
                    Debug.LogFormat("<><FontConfig.ReadConfig>Content: {0}", www.text);

                    xmlDoc.LoadXml(www.text);
                    ArrayList allNodes = xmlDoc.ToXml().Children;
                    foreach (SecurityElement seFonts in allNodes)
                    {
                        if (seFonts.Tag == "Fonts")
                        {
                            ArrayList fontNodes = seFonts.Children;
                            foreach (SecurityElement seFont in fontNodes)
                            {
                                if (seFont.Tag == "Font")
                                {
                                    this.configs.Add(seFont.Attribute("Name"), seFont.Attribute("FullName"));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("<><FontConfig.ReadConfig>Error: {0}", ex.Message);
                }
            }
            Debug.Log("<><FontConfig.ReadConfig>Load complete");
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
            ConfigLoader.Instance.LoadConfig("Config/Global/FontConfig.xml", this.ReadConfig);
        }
        /// <summary>
        /// 获取字体完整名称
        /// </summary>
        /// <param name="fontShortName">字体简称</param>
        /// <returns>如果配置项中能找到简称对应的字体则返回该字体全程，否则返回Unity默认字体Arial</returns>
        public string GetFontFullName(string fontShortName)
        {
            return this.configs != null && this.configs.ContainsKey(fontShortName) ? this.configs[fontShortName] : DEFAULT_FONT;
        }
        /// <summary>
        /// 判断字体是否合法
        /// </summary>
        /// <param name="fontShortName">字体简称</param>
        /// <returns></returns>
        public bool IsValid(string fontShortName)
        {
            return this.configs != null && this.configs.ContainsKey(fontShortName);
        }
        /// <summary>
        /// 获取所有字体的完整名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllFont()
        {
            return this.configs != null && this.configs.Count > 0 ? this.configs.Values.ToList() : null;
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
