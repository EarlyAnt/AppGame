using System.Collections.Generic;

namespace AppGame.Config
{
    /// <summary>
    /// 卡片配置文件读取接口
    /// </summary>
    public interface ICardConfig : IConfig
    {
        /// <summary>
        /// 加载卡片配置数据
        /// </summary>
        void Load();
        /// <summary>
        /// 获取所有卡片
        /// </summary>
        /// <returns></returns>
        List<CardInfo> GetAllCards();
        /// <summary>
        /// 获取指定编码的卡片
        /// </summary>
        /// <param name="cardID">卡片编码</param>
        /// <returns></returns>
        CardInfo GetCard(string cardID);
        /// <summary>
        /// 获取指定景点的卡片
        /// </summary>
        /// <param name="scenicID">景点编码</param>
        /// <returns></returns>
        CardInfo GetCardByScenicID(string scenicID);
    }

    public static class CardTypes
    {
        public const string Scenic = "01";
        public const string Random = "02";
    }

    public class CardInfo
    {
        public string CardID { get; set; }
        public string CardType { get; set; }
        public string CardName { get; set; }
        public string ScenicID { get; set; }
        public string Image { get; set; }
        public string Text { get; set; }
    }
}
