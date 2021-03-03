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
    /// 物品配置读取类
    /// </summary>
    public class ItemConfig : BaseConfig, IItemConfig
    {
        /************************************************属性与变量命名************************************************/
        //物品配置数据字典
        private List<Item> configs = new List<Item>();
        /************************************************私  有  方  法************************************************/
        //读取物品配置文件
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
                    foreach (SecurityElement seItems in allNodes)
                    {
                        if (seItems.Tag == "Items")
                        {
                            ArrayList itemsNode = seItems.Children;
                            foreach (SecurityElement seItem in itemsNode)
                            {
                                if (seItem.Tag == "Item")
                                {
                                    Item item = new Item()
                                    {
                                        ItemID = seItem.Attribute("ItemID"),
                                        ItemType = seItem.Attribute("ItemType"),
                                        ItemName = seItem.Attribute("ItemName"),
                                        ItemIcon = seItem.Attribute("ItemIcon"),
                                        Desc = seItem.Attribute("Desc")
                                    };
                                    this.configs.Add(item);
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
        /// 加载物品配置数据
        /// </summary>
        public void Load()
        {
            if (this.isLoaded)
                return;
            this.configs.Clear();
            ConfigLoader.Instance.LoadConfig("Config/Global/ItemConfig.xml", this.ReadConfig);
        }
        /// <summary>
        /// 获取所有物品
        /// </summary>
        /// <returns></returns>
        public List<Item> GetAllItems()
        {
            return this.configs;
        }
        /// <summary>
        /// 获取指定物品
        /// </summary>
        /// <param name="itemID">物品ID</param>
        /// <returns></returns>
        public Item GetItem(string itemID)
        {
            if (this.configs != null)
                return this.configs.Find(t => t.ItemID == itemID);
            else
                return null;
        }
        /// <summary>
        /// 判断物品是否合法
        /// </summary>
        /// <param name="itemID">物品ID</param>
        /// <returns></returns>
        public bool HasItem(string itemID)
        {
            if (this.configs != null)
                return this.configs.Exists(t => t.ItemID == itemID);
            else
                return false;
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
