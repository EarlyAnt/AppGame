using Mono.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

namespace AppGame.Config
{
    /// <summary>
    /// 景点配置读取类
    /// </summary>
    public class ScenicConfig : BaseConfig, IScenicConfig
    {
        /************************************************属性与变量命名************************************************/
        private List<ScenicInfo> configs = new List<ScenicInfo>();
        /************************************************私  有  方  法************************************************/
        //读取语言配置文件
        private void ReadConfig(WWW www)
        {
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogErrorFormat("<><ScenicConfig.ReadConfig>Error: {0}" + www.error);
                return;
            }

            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                try
                {
                    SecurityParser xmlDoc = new SecurityParser();
                    Debug.LogFormat("<><ScenicConfig.ReadConfig>Content: {0}", www.text);

                    xmlDoc.LoadXml(www.text);
                    ArrayList allNodes = xmlDoc.ToXml().Children;
                    foreach (SecurityElement seResConfigs in allNodes)
                    {//根节点
                        if (seResConfigs.Tag == "Scenics")
                        {
                            ArrayList scenicNodes = seResConfigs.Children;
                            foreach (SecurityElement seScenic in scenicNodes)
                            {
                                if (seScenic.Tag == "Scenic")
                                {
                                    ScenicInfo mapInfo = new ScenicInfo()
                                    {
                                        ID = seScenic.Attribute("ID"),
                                        Name = seScenic.Attribute("Name"),
                                        MapID = seScenic.Attribute("MapID"),
                                        CardID = seScenic.Attribute("CardID")
                                    };
                                    this.configs.Add(mapInfo);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("<><ScenicConfig.ReadConfig>Error: {0}", ex.Message);
                }
                Debug.Log("<><ScenicConfig.ReadConfig>Load complete");
                this.isLoaded = true;
            }
        }
        /************************************************公  共  方  法************************************************/
        /// <summary>
        /// 加载景点数据
        /// </summary>
        public void Load()
        {
            if (this.isLoaded)
                return;
            this.configs.Clear();
            ConfigLoader.Instance.LoadConfig("Config/Cycling/Scenic.xml", this.ReadConfig);
        }
        /// <summary>
        /// 判断是否包含指定的景点
        /// </summary>
        /// <param name="scenicID">指定的景点的ID</param>
        /// <returns></returns>
        public bool HasScenic(string scenicID)
        {
            if (this.configs != null)
                return this.configs.Exists(t => t.ID == scenicID);
            else
                return false;
        }
        /// <summary>
        /// 获取所有景点
        /// </summary>
        /// <returns></returns>
        public List<ScenicInfo> GetAllScenics()
        {
            return this.configs;
        }
        /// <summary>
        /// 获取指定景点
        /// </summary>
        /// <param name="scenicID">景点ID</param>
        /// <returns></returns>
        public ScenicInfo GetScenic(string scenicID)
        {
            if (this.configs != null)
                return this.configs.Find(t => t.ID == scenicID);
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
