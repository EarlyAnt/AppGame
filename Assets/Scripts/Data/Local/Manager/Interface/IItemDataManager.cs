using AppGame.Data.Model;
using System.Collections.Generic;

namespace Hank
{
    public interface IItemDataManager
    {
        void SetItem(int itemId, int itemCount);
        void AddItem(int itemId, int count = 1);
        bool HasItem(int itemId, int count = 1);
        bool ReduceItem(int itemId, int count = 1);

        int GetItemCount(int itemId);
        List<int> GetAllItems();

        void SaveItemDatas();
        void LoadItemDatas();

        void SendAllItemData(bool force);
        void SetValuesLong(GameData datas);
    }
}

