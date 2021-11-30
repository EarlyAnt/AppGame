using AppGame.Config;
using AppGame.Data.Common;
using AppGame.Data.Local;
using AppGame.Data.Model;
using AppGame.Module.Cycling;
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
        [Inject]
        public IItemConfig ItemConfig { get; set; }
        [Inject]
        public UploadItemDataSignal UploadItemDataSignal { get; set; }
        public const string ITEM_DATA_KEY = "ItemData";
        private List<ItemData> buffer = new List<ItemData>();

        //增加物品数量
        public void AddItem(string itemID, int itemCount = 1)
        {
            if (!this.ItemValid(itemID)) return;

            ItemData itemData = this.buffer.Find(t => t.ItemID == itemID);
            if (itemData != null)
            {
                itemData.ItemCount += itemCount;
                Debug.LogFormat("<><ItemDataManager.AddItem>existed, id: {0}, add count: {1}, current count: {2}", itemID, itemCount, itemData.ItemCount);
            }
            else
            {
                this.buffer.Add(new ItemData() { ItemID = itemID, ItemCount = itemCount });
                Debug.LogFormat("<><ItemDataManager.AddItem>not existed, id: {0}, add count: {1}, current count: {2}", itemID, itemCount, itemCount);
            }

            this.UploadItemData();
        }
        //减少物品数量
        public void ReduceItem(string itemID, int itemCount = 1)
        {
            if (!this.ItemValid(itemID)) return;

            ItemData itemData = this.buffer.Find(t => t.ItemID == itemID);
            if (itemData != null)
            {
                itemData.ItemCount -= itemCount;
                Debug.LogFormat("<><ItemDataManager.ReduceItem>existed, id: {0}, reduce count: {1}, current count: {2}", itemID, itemCount, itemData.ItemCount);
            }
            else
            {
                Debug.LogErrorFormat("<><ItemDataManager.ReduceItem>Error: item[{0}] is not existed", itemID);
            }

            this.UploadItemData();
        }
        //设置物品数量
        public void SetItem(string itemID, int itemCount)
        {
            if (!this.ItemValid(itemID)) return;

            ItemData itemData = this.buffer.Find(t => t.ItemID == itemID);
            if (itemData != null)
            {
                itemData.ItemCount = itemCount;
                Debug.LogFormat("<><ItemDataManager.SetItem>existed, id: {0}, add count: {1}", itemID, itemCount);
            }
            else
            {
                this.buffer.Add(new ItemData() { ItemID = itemID, ItemCount = itemCount });
                Debug.LogFormat("<><ItemDataManager.SetItem>not existed, id: {0}, add count: {1}", itemID, itemCount);
            }

            this.UploadItemData();
        }
        //判断是否有物品
        public bool HasItem(string itemID, int itemCount = 1)
        {
            if (!this.ItemValid(itemID)) return false;

            ItemData itemData = this.buffer.Find(t => t.ItemID == itemID);
            if (itemData != null)
            {
                Debug.LogFormat("<><ItemDataManager.HasItem>id: {0}, count: {1}, existed: {2}", itemID, itemCount, itemData.ItemCount >= itemCount);
                return itemData.ItemCount >= itemCount;
            }
            else
            {
                Debug.LogFormat("<><ItemDataManager.HasItem>id: {0}, count: {1}, existed: false", itemID, itemCount);
                return false;
            }
        }
        //获取物品数量
        public int GetItemCount(string itemID)
        {
            if (!this.ItemValid(itemID)) return 0;

            ItemData itemData = this.buffer.Find(t => t.ItemID == itemID);
            int itemCount = itemData != null ? itemData.ItemCount : 0;
            Debug.LogFormat("<><ItemDataManager.GetItemCount>id: {0}, count: {1}", itemID, itemCount);
            return itemCount;
        }
        //保存物品列表
        public void SaveItemList(List<ItemData> itemDataList)
        {
            if (itemDataList != null)
            {
                if (this.buffer == null)
                    this.buffer = new List<ItemData>();

                foreach (ItemData remoteItem in itemDataList)
                {
                    ItemData localItem = this.buffer.Find(t => t.ItemID == remoteItem.ItemID);
                    if (localItem != null)
                    {
                        localItem.ItemCount = Mathf.Max(localItem.ItemCount, remoteItem.ItemCount);
                        this.buffer.Add(localItem);
                    }
                    else
                    {
                        this.buffer.Add(new ItemData() { ItemID = remoteItem.ItemID, ItemCount = remoteItem.ItemCount });
                    }

                }
                this.GameDataHelper.SaveObject<List<ItemData>>(ITEM_DATA_KEY, itemDataList);
            }
        }
        //清除本地缓存
        public void Clear(bool confirm = false)
        {
            if (this.buffer != null)
                this.buffer.Clear();

            if (confirm)
                this.GameDataHelper.Clear(ITEM_DATA_KEY);
        }
        //判断配置表里是否有此物体
        private bool ItemValid(string itemID)
        {
            bool valid = this.ItemConfig != null && !string.IsNullOrEmpty(itemID) && this.ItemConfig.HasItem(itemID);
            if (!valid)
                Debug.LogErrorFormat("<><ItemDataManager.ItemValid>error: can not find the item[{0}] in config file", itemID);

            return valid;
        }
        //上传物品数据
        private void UploadItemData()
        {
            try
            {
                if (this.buffer != null)
                {
                    this.UploadItemDataSignal.Dispatch(this.buffer);
                    Debug.LogError("<><ItemDataManager.UploadItemData>+++++++++++++++++++++++++++++++++++++++");
                }
                else
                {
                    Debug.LogError("<><ItemDataManager.UploadItemData>error: parameter 'buffer' is null");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogErrorFormat("<><ItemDataManager.UploadItemData>error: {0}", ex.Message);
            }
        }
    }
}

