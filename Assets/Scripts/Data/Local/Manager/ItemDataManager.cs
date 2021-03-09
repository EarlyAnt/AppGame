using AppGame.Config;
using AppGame.Data.Common;
using AppGame.Data.Local;
using AppGame.Data.Model;
using AppGame.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AppGame.Data.Local
{
    public class ItemDataManager : IItemDataManager
    {
        [Inject]
        public IJsonUtil JsonUtil { set; get; }
        [Inject]
        public IGameDataHelper GameDataHelper { get; set; }
        [Inject]
        public IChildInfoManager ChildInfoManager { get; set; }
        //[Inject]
        //public StartPostGameDataCommandSignal startPostGameDataCommand { get; set; }
        [Inject]
        public IItemConfig ItemConifg { get; set; }
        public const string ITEM_DATA_KEY = "ItemData";
        Dictionary<string, int> itemInfos = new Dictionary<string, int>();

        struct ItemInfo
        {
            public string itemID;
            public int itemCount;
        }

        [PostConstruct]
        public void PostConstruct()
        {
            LoadItemDatas();
        }

        public void SetItem(string itemID, int itemCount)
        {
            Debug.LogFormat("<><ItemDataManager.SetItem>ItemID: {0}, ItemCount: {1}", itemID, itemCount);
            itemCount = this.GetValidCount(itemID, itemCount);
            if (this.itemInfos.ContainsKey(itemID))
            {
                this.itemInfos[itemID] = itemCount;
            }
            else
            {
                this.itemInfos.Add(itemID, itemCount);
            }

            try
            {
                this.SaveItemDatas();
                this.SyncItemsData2Server(itemID, itemCount);
            }
            catch (System.Exception ex)
            {
                Debug.LogErrorFormat("<><ItemDataManager.SetItem>Error: {0}, itemId: {1}, itemCount: {2}", ex.Message, itemID, itemCount);
            }
        }

        public void AddItem(string itemID, int itemCount = 1)
        {
            int newItemCount = 0;
            if (this.itemInfos.ContainsKey(itemID))
            {
                newItemCount = this.GetValidCount(itemID, this.itemInfos[itemID] + itemCount);
                this.itemInfos[itemID] = newItemCount;
            }
            else
            {
                newItemCount = this.GetValidCount(itemID, itemCount);
                this.itemInfos.Add(itemID, newItemCount);
            }

            try
            {
                this.SaveItemDatas();
                this.SyncItemsData2Server(itemID, newItemCount);
            }
            catch (System.Exception ex)
            {
                Debug.LogErrorFormat("<><ItemDataManager.AddItem>Error: {0}, itemId: {1}, count: {2}, currentCount: {3}", ex.Message, itemID, itemCount, newItemCount);
            }
        }

        public bool HasItem(string itemID, int itemCount = 1)
        {
            int count = 0;
            if (this.itemInfos.TryGetValue(itemID, out count))
            {
                return count >= itemCount;
            }
            return false;
        }

        public bool ReduceItem(string itemID, int count = 1)
        {
            Item item = this.ItemConifg.GetItem(itemID);
            if (item.ItemType != ItemTypes.Coin)
            {//目前只有表情类资源可被扣减
                Debug.LogErrorFormat("<><ItemDataManager.ReduceItem>only coin can be reduced, itemID: {0}, itemType: {1}, count: {2}", itemID, item.ItemType, count);
                return false;
            }

            int currentCount = this.itemInfos.ContainsKey(itemID) ? this.itemInfos[itemID] : 0;//获取当前数量
            if (currentCount >= count)
            {//扣减当前数量
                currentCount -= count;
            }
            else
            {//余额不足，不予操作
                Debug.LogErrorFormat("<><ItemDataManager.ReduceItem>no enough item, itemId: {0}, count: {1}, currentCount: {2}", itemID, count, currentCount);
                return false;
            }

            if (this.itemInfos.ContainsKey(itemID))
                this.itemInfos[itemID] = currentCount;
            else
                this.itemInfos.Add(itemID, currentCount);

            try
            {
                this.SaveItemDatas();
                this.SyncItemsData2Server(itemID, currentCount);
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogErrorFormat("<><ItemDataManager.ReduceItem>Error: {0}, itemId: {1}, count: {2}, currentCount: {3}", ex.Message, itemID, count, currentCount);
                return false;
            }
        }

        public int GetItemCount(string itemID)
        {
            int itemCount;
            if (this.itemInfos.TryGetValue(itemID, out itemCount))
            {
                return itemCount;
            }
            return 0;
        }

        public void SaveItemDatas()
        {
            List<ItemInfo> listInfos = Convert2List();
            this.GameDataHelper.SaveObject<List<ItemInfo>>(ITEM_DATA_KEY, listInfos);
        }

        public void LoadItemDatas()
        {
            this.itemInfos.Clear();
            List<ItemInfo> listInfos = this.GameDataHelper.GetObject<List<ItemInfo>>(ITEM_DATA_KEY);
            if (listInfos != null)
            {
                foreach (var info in listInfos)
                {
                    this.itemInfos.Add(info.itemID, info.itemCount);
                }
            }
        }

        public void SendAllItemData(bool force)
        {
            //List<ItemsDataBean> items = new List<ItemsDataBean>();
            ////StringBuilder strbContent = new StringBuilder();
            //foreach (var pair in itemsInfo)
            //{
            //    Item item = this.ItemConifg.GetItemById(pair.Key);
            //    if (item != null)
            //    {
            //        items.Add(new ItemsDataBean
            //        {
            //            item_id = pair.Key,
            //            type = item.Type,
            //            count = this.GetValidCount(item.ItemId, pair.Value),
            //            name = item.Name
            //        });
            //        //strbContent.AppendFormat("ID: {0}, Type: {1}, Count: {2}, Name: {3}\n", pair.Key, item.Type, pair.Value, item.Name);
            //    }
            //}
            //SendItemsData(items);
            ////Debug.LogFormat("<><ItemDataManager.SendAllItemsData>Datas: {0}", strbContent.ToString());
        }

        public void SetValuesLong(GameData datas)
        {
            //List<ItemsDataBean> listInfos = datas.getItemsData();
            //if (listInfos != null)
            //{
            //    for (int i = 0; i < listInfos.Count; ++i)
            //    {
            //        AddItem(listInfos[i].getItem_id(), listInfos[i].getCount());
            //    }
            //}
        }

        //private void SendItemsData(List<ItemsDataBean> dataItems)
        //{
        //    GameData datas = GameData.getGameDataBuilder()
        //        .setItemsData(dataItems)
        //        .setTimestamp((int)DateUtil.GetTimeStamp())
        //        .build();
        //    string jsonData = JsonUtil.Json2String(datas);
        //    startPostGameDataCommand.Dispatch(jsonData);
        //    Debug.LogFormat("----{0}====ItemDataManager.SendItemsData: {1}", System.DateTime.Now, jsonData);
        //}

        private List<ItemInfo> Convert2List()
        {
            List<ItemInfo> listInfos = new List<ItemInfo>();
            foreach (var pair in this.itemInfos)
            {
                listInfos.Add(new ItemInfo
                {
                    itemID = pair.Key,
                    itemCount = pair.Value
                });
            }
            return listInfos;
        }

        private void SyncItemsData2Server(string itemID, int itemCount)
        {
            //Item item = this.ItemConifg.GetItem(itemID);
            //if (item != null)
            //{
            //    List<ItemsDataBean> items = new List<ItemsDataBean>();
            //    items.Add(new ItemsDataBean
            //    {
            //        item_id = itemId,
            //        type = item.Type,
            //        count = this.GetValidCount(item.ItemId, itemCount),
            //        name = item.Name
            //    });
            //    SendItemsData(items);
            //}
        }

        private int GetValidCount(string itemID, int itemCount)
        {
            int maxCount = 1;
            Item item = this.ItemConifg.GetItem(itemID);
            if (item != null)
            {
                switch (item.ItemType)
                {
                    case ItemTypes.Coin:
                        maxCount = 99999;
                        break;
                    case ItemTypes.Card:
                        maxCount = 1;
                        break;
                }
            }
            return Mathf.Clamp(itemCount, 0, maxCount);
        }

        public void Clear(bool confirm = false)
        {
            if (this.itemInfos != null)
                this.itemInfos.Clear();

            if (confirm)
                this.GameDataHelper.Clear(ITEM_DATA_KEY);
        }
    }
}

