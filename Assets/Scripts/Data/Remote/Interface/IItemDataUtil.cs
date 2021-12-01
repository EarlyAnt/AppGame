using AppGame.Data.Common;
using AppGame.Data.Model;
using System;
using System.Collections.Generic;

namespace AppGame.Data.Remote
{
    public interface IItemDataUtil
    {
        void GetItemData(Action<List<ItemData>> callback = null, Action<string> errCallback = null);

        void PutItemData(List<ItemData> itemDataList, Action<Result> callback = null, Action<Result> errCallback = null);
    }

    public class GetItemDataResponse : DataBase
    {
        public List<NetItemData> data { get; set; }

        public List<ItemData> ToItemDataList()
        {
            if (this.data != null && this.data != null)
            {
                List<ItemData> itemDataList = new List<ItemData>();
                foreach (var item in this.data)
                {
                    itemDataList.Add(item.ToItemData());
                }
                return itemDataList;
            }
            else return null;
        }
    }

    public class NetItemData
    {
        public string item_id { get; set; }
        public int item_count { get; set; }

        public ItemData ToItemData()
        {
            return new ItemData()
            {
                ItemID = this.item_id,
                ItemCount = this.item_count
            };
        }
    }

    public class PutItemDataRequest
    {
        private List<ItemData> itemDataList;

        public PutItemDataRequest(List<ItemData> itemDataList)
        {
            this.itemDataList = itemDataList;
        }

        public List<NetItemData> ToNetItemDataList()
        {
            if (this.itemDataList != null)
            {
                List<NetItemData> netItemDataList = new List<NetItemData>();
                foreach (var item in this.itemDataList)
                {
                    netItemDataList.Add(new NetItemData() { item_id = item.ItemID, item_count = item.ItemCount });
                }
                return netItemDataList;
            }
            else return null;
        }
    }
}