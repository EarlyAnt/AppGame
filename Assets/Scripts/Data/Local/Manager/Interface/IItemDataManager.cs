using AppGame.Data.Model;
using System.Collections.Generic;

namespace AppGame.Data.Local
{
    public interface IItemDataManager
    {
        void AddItem(string itemID, int itemCount = 1);        
        void ReduceItem(string itemID, int itemCount = 1);
        void SetItem(string itemID, int itemCount);
        bool HasItem(string itemID, int itemCount = 1);
        int GetItemCount(string itemID);
        void SaveItemList(List<ItemData> itemDataList);
        void Clear(bool confirm = false);
    }

    public static class Items
    {
        public const string COIN = "10000";
    }
}

