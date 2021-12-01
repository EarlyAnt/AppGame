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
        bool IsNovice();
        List<PlayerData> BuildGameData();

        void SavePlayerData(PlayerData playerData);
        PlayerData GetPlayerData(string childSN);

        void SaveMpCollection(string childSN);
        bool MpCollected(string childSN);
        void ClearMpCollection();

        void ClearAllData();
    }
}
