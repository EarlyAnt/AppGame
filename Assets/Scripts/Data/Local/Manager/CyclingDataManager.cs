using AppGame.Data.Model;
using AppGame.Util;
using System;
using System.Collections.Generic;

namespace AppGame.Data.Local
{
    class CyclingDataManager : ICyclingDataManager
    {
        [Inject]
        public ILocalDataManager LocalDataManager { get; set; }
        [Inject]
        public IJsonUtils JsonUtils { get; set; }

        private OriginData originData = null;
        private List<PlayerData> playerDataList = null;
        private Dictionary<string, DateTime> mpCollections = null;
        private const string COLLECT_MP_DATA_KEY = "collect_mp";

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

        /// <summary>
        /// 保存已收取的家人及好友的能量分成
        /// </summary>
        /// <param name="childSN"></param>
        public void SaveMpCollection(string childSN)
        {
            if (this.mpCollections == null)
                this.mpCollections = new Dictionary<string, DateTime>();

            if (this.mpCollections.ContainsKey(childSN))
                this.mpCollections[childSN] = DateTime.Today;
            else
                this.mpCollections.Add(childSN, DateTime.Today);

            string dataString = this.JsonUtils.Json2String(this.mpCollections);
            this.LocalDataManager.SaveObject<string>(COLLECT_MP_DATA_KEY, dataString);
        }
        /// <summary>
        /// 判断今日是否已经收取了某个家人或好友的能量分成
        /// </summary>
        /// <param name="childSN"></param>
        /// <returns></returns>
        public bool MpCollected(string childSN)
        {
            string dataString = this.LocalDataManager.GetObject<string>(COLLECT_MP_DATA_KEY);
            this.mpCollections = this.JsonUtils.String2Json<Dictionary<string, DateTime>>(dataString);

            if (this.mpCollections != null && this.mpCollections.ContainsKey(childSN) && this.mpCollections[childSN] == DateTime.Today)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 清空本地记录的家人及好友能量分成的收取
        /// </summary>
        public void ClearMpCollection()
        {
            if (this.mpCollections != null)
                this.mpCollections.Clear();

            this.LocalDataManager.SaveObject<string>(COLLECT_MP_DATA_KEY, "");
        }
    }
}
