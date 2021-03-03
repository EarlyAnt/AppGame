using AppGame.Data.Common;
using AppGame.Data.Local;
using AppGame.Data.Model;
using AppGame.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hank
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
        //[Inject]
        //public IItemConfig ItemConifg { get; set; }
        public const string itemsKey = "ItemsData";
        Dictionary<int, int> itemsInfo = new Dictionary<int, int>();

        struct ItemInfo
        {
            public int itemId;
            public int itemCount;
        }

        [PostConstruct]
        public void PostConstruct()
        {
            LoadItemDatas();
        }

        public void SetItem(int itemId, int itemCount)
        {
            Debug.LogFormat("--------{0}-------ItemsDataManager.SetItem: {1}, {2}", System.DateTime.Now, itemId, itemCount);
            int count = 0;
            if (itemsInfo.TryGetValue(itemId, out count))
            {
                itemsInfo.Remove(itemId);
            }

            if (itemCount > 0)
            {
                itemsInfo.Add(itemId, itemCount);
            }
        }

        public void AddItem(int itemId, int itemCount = 1)
        {
            int currentCount = 0;
            if (itemsInfo.ContainsKey(itemId))
            {
                currentCount = itemsInfo[itemId] + itemCount;
                itemsInfo[itemId] = this.GetValidCount(itemId, currentCount);
            }
            else
            {
                currentCount = itemCount;
                itemsInfo.Add(itemId, this.GetValidCount(itemId, currentCount));
            }

            SaveItemDatas();
            SyncItemsData2Server(itemId, currentCount);
        }

        public bool HasItem(int itemId, int itemCount = 1)
        {
            int count = 0;
            if (itemsInfo.TryGetValue(itemId, out count))
            {
                return count >= itemCount;
            }
            return false;
        }

        public bool ReduceItem(int itemId, int count = 1)
        {
            //Item item = this.ItemConifg.GetItemById(itemId);
            //if (item.Type != (int)ItemTypes.Expression && item.Type != (int)ItemTypes.Plant)
            //{//目前只有表情类资源可被扣减
            //    Debug.LogErrorFormat("<><ItemsDataManager.ReduceItem>only expression and plant can be reduced, itemId: {0}, itemType: {1}, count: {2}", itemId, item.Type, count);
            //    return false;
            //}

            //int currentCount = itemsInfo.ContainsKey(itemId) ? itemsInfo[itemId] : 0;//获取当前数量
            //if (currentCount >= count)
            //{//扣减当前数量
            //    currentCount -= count;
            //}
            //else
            //{//余额不足，不予操作
            //    Debug.LogErrorFormat("<><ItemsDataManager.ReduceItem>no enough item, itemId: {0}, count: {1}, currentCount: {2}", itemId, count, currentCount);
            //    return false;
            //}

            //if (itemsInfo.ContainsKey(itemId))
            //    itemsInfo[itemId] = currentCount;
            //else
            //    itemsInfo.Add(itemId, currentCount);

            //try
            //{
            //    SaveItemDatas();
            //    SyncItemsData2Server(itemId, currentCount);
            //    return true;
            //}
            //catch (System.Exception ex)
            //{
            //    Debug.LogErrorFormat("<><ItemsDataManager.ReduceItem>Error: {0}, itemId: {1}, count: {2}, currentCount: {3}", ex.Message, itemId, count, currentCount);
                return false;
            //}
        }

        public int GetItemCount(int itemId)
        {
            int itemCount;
            if (itemsInfo.TryGetValue(itemId, out itemCount))
            {
                return itemCount;
            }
            return 0;

        }

        public List<int> GetAllItems()
        {
            return this.itemsInfo == null ? null : this.itemsInfo.Keys.ToList();
        }

        public void SaveItemDatas()
        {
            List<ItemInfo> listInfos = Convert2List();
            GameDataHelper.SaveObject<List<ItemInfo>>(itemsKey, listInfos);
        }

        public void LoadItemDatas()
        {
            itemsInfo.Clear();
            List<ItemInfo> listInfos = GameDataHelper.GetObject<List<ItemInfo>>(itemsKey);
            if (listInfos != null)
            {
                foreach (var info in listInfos)
                {
                    itemsInfo.Add(info.itemId, info.itemCount);
                }
            }
        }

        public void SendAllItemData(bool force)
        {
            //List<ItemsDataBean> items = new List<ItemsDataBean>();
            ////StringBuilder strbContent = new StringBuilder();
            //foreach (var pair in itemsInfo)
            //{
            //    Item item = ItemConifg.GetItemById(pair.Key);
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
            ////Debug.LogFormat("<><ItemsDataManager.SendAllItemsData>Datas: {0}", strbContent.ToString());
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
        //    Debug.LogFormat("----{0}====ItemsDataManager.SendItemsData: {1}", System.DateTime.Now, jsonData);
        //}

        private List<ItemInfo> Convert2List()
        {
            List<ItemInfo> listInfos = new List<ItemInfo>();
            foreach (var pair in itemsInfo)
            {
                listInfos.Add(new ItemInfo
                {
                    itemId = pair.Key,
                    itemCount = pair.Value
                });
            }
            return listInfos;
        }

        private void SyncItemsData2Server(int itemId, int itemCount)
        {
            //Item item = ItemConifg.GetItemById(itemId);
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

        private int GetValidCount(int itemId, int itemCount)
        {
            int maxCount = 1;
            //Item item = ItemConifg.GetItemById(itemId);
            //if (item != null)
            //{
            //    switch (item.Type)
            //    {
            //        case (int)ItemTypes.ExpCoinCrown:
            //            if (itemId == 10003 || itemId == 10004 || itemId == 10005)
            //                maxCount = 999;
            //            break;
            //        case (int)ItemTypes.Nim:
            //        case (int)ItemTypes.TreasureBox:
            //        case (int)ItemTypes.Dress:
            //            maxCount = 1;
            //            break;
            //        case (int)ItemTypes.Coin:
            //            maxCount = 99999;
            //            break;
            //        case (int)ItemTypes.Food:
            //        case (int)ItemTypes.Expression:
            //        case (int)ItemTypes.Plant:
            //            maxCount = 99;
            //            break;
            //    }
            //}
            return Mathf.Clamp(itemCount, 0, maxCount);
        }
    }
}

