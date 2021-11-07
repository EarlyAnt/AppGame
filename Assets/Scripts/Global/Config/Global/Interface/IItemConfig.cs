using System.Collections.Generic;

namespace AppGame.Config
{
    /// <summary>
    /// 物品配置文件读取接口
    /// </summary>
    public interface IItemConfig : IConfig
    {
        /// <summary>
        /// 加载物品配置数据
        /// </summary>
        void Load();
        /// <summary>
        /// 获取所有物品
        /// </summary>
        /// <returns></returns>
        List<Item> GetAllItems();
        /// <summary>
        /// 获取指定物品
        /// </summary>
        /// <param name="itemID">物品ID</param>
        /// <returns></returns>
        Item GetItem(string itemID);
        /// <summary>
        /// 判断物品是否合法
        /// </summary>
        /// <param name="itemID">物品ID</param>
        /// <returns></returns>
        bool HasItem(string itemID);
    }

    public enum ItemTypes
    {
        Coin = 10,
        Card = 20,
        Accessory = 21
    }

    public class Item
    {
        public string ItemID { get; set; }
        public ItemTypes ItemType { get; set; }
        public string ItemName { get; set; }
        public string ItemIcon { get; set; }
        public string Desc { get; set; }
    }
}
