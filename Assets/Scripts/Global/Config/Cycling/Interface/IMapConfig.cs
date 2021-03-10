﻿using System.Collections.Generic;

namespace AppGame.Config
{
    /// <summary>
    /// 地图配置接口
    /// </summary>
    public interface IMapConfig : IConfig
    {
        /// <summary>
        /// 加载地图数据
        /// </summary>
        void Load();
        /// <summary>
        /// 判断是否包含指定的地图
        /// </summary>
        /// <param name="mapID">指定的地图的ID</param>
        /// <returns></returns>
        bool HasMap(string mapID);
        /// <summary>
        /// 获取所有地图
        /// </summary>
        /// <returns></returns>
        List<MapInfo> GetAllMaps();
        /// <summary>
        /// 获取指定地图
        /// </summary>
        /// <param name="mapID">地图ID</param>
        /// <returns></returns>
        MapInfo GetMap(string mapID);
    }

    public class MapInfo
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public string CityID { get; set; }
        public string CityName { get; set; }
        public string AB { get; set; }
        public string MapImage { get; set; }
        public string PathImage { get; set; }
        public string NextMap { get; set; }
    }
}
