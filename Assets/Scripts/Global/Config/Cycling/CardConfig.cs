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
    /// 卡片配置读取类
    /// </summary>
    public class CardConfig : BaseConfig, ICardConfig
    {
        /************************************************属性与变量命名************************************************/
        //卡片配置数据字典
        private List<CardInfo> configs = new List<CardInfo>();
        /************************************************私  有  方  法************************************************/
        //读取卡片配置文件
        private void ReadConfig(WWW www)
        {
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogErrorFormat("<><CardConfig.ReadConfig>Error: {0}" + www.error);
                return;
            }

            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                try
                {
                    SecurityParser xmlDoc = new SecurityParser();
                    Debug.LogFormat("<><CardConfig.ReadConfig>Content: {0}", www.text);

                    xmlDoc.LoadXml(www.text);
                    ArrayList allNodes = xmlDoc.ToXml().Children;
                    foreach (SecurityElement seCards in allNodes)
                    {
                        if (seCards.Tag == "Cards")
                        {
                            ArrayList cardsNode = seCards.Children;
                            foreach (SecurityElement seCard in cardsNode)
                            {
                                if (seCard.Tag == "Card")
                                {
                                    CardInfo card = new CardInfo()
                                    {
                                        CardID = seCard.Attribute("CardID"),
                                        CardType = seCard.Attribute("CardType"),
                                        CardName = seCard.Attribute("CardName"),
                                        ScenicID = seCard.Attribute("ScenicID"),
                                        Image = seCard.Attribute("Image"),
                                        Text = seCard.Attribute("Text"),
                                        Url = seCard.Attribute("Url")
                                    };
                                    this.configs.Add(card);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("<><CardConfig.ReadConfig>Error: {0}", ex.Message);
                }
            }
            Debug.Log("<><CardConfig.ReadConfig>Load complete");
            this.isLoaded = true;
        }
        /************************************************公  共  方  法************************************************/
        /// <summary>
        /// 加载卡片配置数据
        /// </summary>
        public void Load()
        {
            if (this.isLoaded)
                return;
            this.configs.Clear();
            ConfigLoader.Instance.LoadConfig("Config/Cycling/Card.xml", this.ReadConfig);
        }
        /// <summary>
        /// 获取所有卡片
        /// </summary>
        /// <returns></returns>
        public List<CardInfo> GetAllCards()
        {
            return this.configs;
        }
        /// <summary>
        /// 获取指定编码的卡片
        /// </summary>
        /// <param name="cardID">卡片编码</param>
        /// <returns></returns>
        public CardInfo GetCard(string cardID)
        {
            if (this.configs != null)
                return this.configs.Find(t => t.CardID == cardID);
            else
                return null;
        }
        /// <summary>
        /// 获取指定景点的卡片
        /// </summary>
        /// <param name="scenicID">景点编码</param>
        /// <returns></returns>
        public CardInfo GetCardByScenicID(string scenicID)
        {
            if (this.configs != null)
                return this.configs.Find(t => t.ScenicID == scenicID);
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
