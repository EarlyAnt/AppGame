using AppGame.Data.Model;
using System.Collections.Generic;

namespace AppGame.Data.Local
{
    interface ICyclingDataManager
    {
        void SaveOriginData(OriginData originData);
        OriginData GetOriginData(string childSN);

        void SavePlayerDataList(List<PlayerData> playerDataList);
        List<PlayerData> GetAllPlayerData();

        void SavePlayerData(PlayerData playerData);
        PlayerData GetPlayerData(string childSN);
    }
}
