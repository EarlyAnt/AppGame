using AppGame.Data.Model;
using System;
using System.Collections.Generic;

namespace AppGame.Data.Local
{
    class CyclingDataManager : ICyclingDataManager
    {
        private OriginData originData = null;
        private List<PlayerData> playerDataList = null;

        /// <summary>
        /// 保存玩家的原始数据(健康数据)
        /// </summary>
        /// <param name="originData"></param>
        public void SaveOriginData(OriginData originData)
        {
            if (originData != null)
                this.originData = originData;
            else
                throw new ArgumentException("<><CyclingDataManager.SaveOriginData>Error: parameter 'originData' is null");
        }
        /// <summary>
        /// 获取指定玩家的原始数据(健康数据)
        /// </summary>
        /// <param name="childSN">玩家编号</param>
        /// <returns></returns>
        public OriginData GetOriginData(string childSN)
        {
            return this.originData;
        }

        /// <summary>
        /// 保存所有玩家的原始数据(健康数据)
        /// </summary>
        /// <param name="playerDataList"></param>
        public void SavePlayerDataList(List<PlayerData> playerDataList)
        {
            if (playerDataList != null)
                this.playerDataList = playerDataList;
            else
                throw new ArgumentException("<><CyclingDataManager.SavePlayerDataList>Error: parameter 'playerDataList' is null");

        }
        /// <summary>
        /// 获取所有玩家的原始数据(健康数据)
        /// </summary>
        /// <returns></returns>
        public List<PlayerData> GetAllPlayerData()
        {
            return this.playerDataList;
        }

        /// <summary>
        /// 保存玩家的游戏数据
        /// </summary>
        /// <param name="playerData"></param>
        public void SavePlayerData(PlayerData playerData)
        {
            if (this.playerDataList == null)
                this.playerDataList = new List<PlayerData>();

            int index = this.playerDataList.FindIndex(t => t.child_sn == playerData.child_sn);
            if (index >= 0 && index < this.playerDataList.Count)
                this.playerDataList.RemoveAt(index);

            this.playerDataList.Add(playerData);
        }
        /// <summary>
        /// 获取指定玩家的游戏数据
        /// </summary>
        /// <param name="childSN"></param>
        /// <returns></returns>
        public PlayerData GetPlayerData(string childSN)
        {
            if (this.playerDataList != null)
                return this.playerDataList.Find(t => t.child_sn == childSN);
            else
                return null;
        }
    }
}
