using Mono.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

namespace AppGame.Config
{
    /// <summary>
    /// 地图配置读取类
    /// </summary>
    public class MapConfig : BaseConfig, IMapConfig
    {
        /************************************************属性与变量命名************************************************/
        private List<MapInfo> configs = new List<MapInfo>();
        /************************************************私  有  方  法************************************************/
        //读取语言配置文件
        private void ReadConfig(WWW www)
        {
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogErrorFormat("<><MapConfig.ReadConfig>Error: {0}" + www.error);
                return;
            }

            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                try
                {
                    SecurityParser xmlDoc = new SecurityParser();
                    Debug.LogFormat("<><MapConfig.ReadConfig>Content: {0}", www.text);

                    xmlDoc.LoadXml(www.text);
                    ArrayList allNodes = xmlDoc.ToXml().Children;
                    foreach (SecurityElement xeResConfigs in allNodes)
                    {//根节点
                        if (xeResConfigs.Tag == "Maps")
                        {
                            ArrayList mapNodes = xeResConfigs.Children;
                            foreach (SecurityElement xeMap in mapNodes)
                            {
                                if (xeMap.Tag == "Map")
                                {
                                    MapInfo mapInfo = new MapInfo()
                                    {
                                        ID = xeMap.Attribute("ID"),
                                        Name = xeMap.Attribute("Name"),
                                        ProvinceID = xeMap.Attribute("ProvinceID"),
                                        ProvinceName = xeMap.Attribute("ProvinceName"),
                                        CityID = xeMap.Attribute("CityID"),
                                        CityName = xeMap.Attribute("CityName")
                                    };
                                    this.configs.Add(mapInfo);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("<><MapConfig.ReadConfig>Error: {0}", ex.Message);
                }
                Debug.Log("<><MapConfig.ReadConfig>Load complete");
                this.isLoaded = true;
            }
        }
        /************************************************公  共  方  法************************************************/
        /// <summary>
        /// 加载地图数据
        /// </summary>
        public void Load()
        {
            if (this.isLoaded)
                return;
            this.configs.Clear();
            ConfigLoader.Instance.LoadConfig("Config/Cycling/Map.xml", this.ReadConfig);
        }
        /// <summary>
        /// 判断是否包含指定的地图
        /// </summary>
        /// <param name="mapID">指定的地图的ID</param>
        /// <returns></returns>
        public bool HasMap(string mapID)
        {
            if (this.configs != null)
                return this.configs.Exists(t => t.ID == mapID);
            else
                return false;
        }
        /// <summary>
        /// 获取所有地图
        /// </summary>
        /// <returns></returns>
        public List<MapInfo> GetAllMaps()
        {
            return this.configs;
        }
        /// <summary>
        /// 获取指定地图
        /// </summary>
        /// <param name="mapID">地图ID</param>
        /// <returns></returns>
        public MapInfo GetMap(string mapID)
        {
            if (this.configs != null)
                return this.configs.Find(t => t.ID == mapID);
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
