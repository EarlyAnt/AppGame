using AppGame.Data.Model;
using System.Collections.Generic;

namespace AppGame.Data.Local
{
    interface IBasicDataManager
    {
        void SaveDataList(List<BasicData> basicDataList);
        List<BasicData> GetAllData();

        void SaveData(BasicData basicData);
        BasicData GetData(string childSN);
    }
}
