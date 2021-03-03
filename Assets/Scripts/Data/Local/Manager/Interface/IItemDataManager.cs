using AppGame.Data.Model;
using System.Collections.Generic;

namespace Hank
{
    public interface IItemDataManager
    {
        void SetItem(string itemID, int itemCount);
        void AddItem(string itemID, int count = 1);
        bool HasItem(string itemID, int count = 1);
        bool ReduceItem(string itemID, int count = 1);

        int GetItemCount(string itemID);

        void SaveItemDatas();
        void LoadItemDatas();

        void SendAllItemData(bool force);
        void SetValuesLong(GameData datas);
    }
}

