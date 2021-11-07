using AppGame.Data.Model;
using System.Collections.Generic;

namespace AppGame.Data.Local
{
    public interface ICyclingDataManager
    {
        void SaveOriginData(OriginData originData);
        OriginData GetOriginData(string childSN);

        void SavePlayerDataList(List<PlayerData> playerDataList);
        List<PlayerData> GetAllPlayerData();

        void SavePlayerData(PlayerData playerData);
        PlayerData GetPlayerData(string childSN);

        void SaveMpCollection(string childSN);
        bool MpCollected(string childSN);
        void ClearMpCollection();
    }
}
