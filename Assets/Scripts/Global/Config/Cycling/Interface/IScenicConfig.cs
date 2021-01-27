using System.Collections.Generic;

namespace AppGame.Config
{
    /// <summary>
    /// 景点配置接口
    /// </summary>
    public interface IScenicConfig : IConfig
    {
        /// <summary>
        /// 加载景点数据
        /// </summary>
        void Load();
        /// <summary>
        /// 判断是否包含指定的景点
        /// </summary>
        /// <param name="scenicID">指定的景点的ID</param>
        /// <returns></returns>
        bool HasScenic(string scenicID);
        /// <summary>
        /// 获取所有景点
        /// </summary>
        /// <returns></returns>
        List<ScenicInfo> GetAllScenics();
        /// <summary>
        /// 获取指定景点
        /// </summary>
        /// <param name="scenicID">景点ID</param>
        /// <returns></returns>
        ScenicInfo GetScenic(string scenicID);
    }

    public class ScenicInfo
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string MapID { get; set; }
        public string Image { get; set; }
        public string Text { get; set; }
    }
}
