using AppGame.Data.Common;
using AppGame.Data.Model;
using AppGame.Util;
using System;
using System.Collections.Generic;

namespace AppGame.Data.Local
{
    class CyclingDataManager : ICyclingDataManager
    {
        [Inject]
        public IGameDataHelper GameDataHelper { get; set; }

        private const string ORIGIN_DATA_DATA_KEY = "origin_data";
        private const string PLAYER_DATA_DATA_KEY = "player_data";
        private const string COLLECT_MP_DATA_KEY = "collect_mp";
        private const string YESTERDAY_MP_DATA_KEY = "yesterday_mp";


        /// <summary>
        /// 保存玩家的原始数据(健康数据)
        /// </summary>
        /// <param name="originData"></param>
        public void SaveOriginData(OriginData originData)
        {
            if (originData != null)
                this.GameDataHelper.SaveObject<OriginData>(ORIGIN_DATA_DATA_KEY, originData);
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
            return this.GameDataHelper.GetObject<OriginData>(ORIGIN_DATA_DATA_KEY);
        }

        /// <summary>
        /// 保存所有玩家的原始数据(健康数据)
        /// </summary>
        /// <param name="playerDataList"></param>
        public void SavePlayerDataList(List<PlayerData> playerDataList)
        {
            if (playerDataList != null)
                this.GameDataHelper.SaveObject(PLAYER_DATA_DATA_KEY, playerDataList);
            else
                throw new ArgumentException("<><CyclingDataManager.SavePlayerDataList>Error: parameter 'playerDataList' is null");

        }
        /// <summary>
        /// 获取所有玩家的原始数据(健康数据)
        /// </summary>
        /// <returns></returns>
        public List<PlayerData> GetAllPlayerData()
        {
            return this.GameDataHelper.GetObject<List<PlayerData>>(PLAYER_DATA_DATA_KEY);
        }

        /// <summary>
        /// 保存玩家的游戏数据
        /// </summary>
        /// <param name="playerData"></param>
        public void SavePlayerData(PlayerData playerData)
        {
            //记录PlayerData
            List<PlayerData> playerDataList = this.GetAllPlayerData();
            if (playerDataList == null)
                playerDataList = new List<PlayerData>();

            int index = playerDataList.FindIndex(t => t.child_sn == playerData.child_sn);
            if (index >= 0 && index < playerDataList.Count)
                playerDataList.RemoveAt(index);

            playerDataList.Add(playerData);
            this.SavePlayerDataList(playerDataList);

            //记录MpData
            List<MpData> mpDataList = this.GameDataHelper.GetObject<List<MpData>>(YESTERDAY_MP_DATA_KEY);
            if (mpDataList == null)
                mpDataList = new List<MpData>();

            MpData mpData = mpDataList.Find(t => t.Date == DateTime.Today);
            if (mpData != null)
                mpDataList.Remove(mpData);

            mpData = new MpData()
            {
                Date = DateTime.Today,
                Mp = playerData.mp_today,
                Uploaded = false
            };
            mpDataList.Add(mpData);

            List<MpData> removeItems = new List<MpData>();
            mpDataList.ForEach(t => { if ((t.Date - DateTime.Today).TotalDays >= 7) removeItems.Add(t); });
            removeItems.ForEach(t => mpDataList.Remove(t));
            this.GameDataHelper.SaveObject<List<MpData>>(YESTERDAY_MP_DATA_KEY, mpDataList);
        }
        /// <summary>
        /// 获取指定玩家的游戏数据
        /// </summary>
        /// <param name="childSN"></param>
        /// <returns></returns>
        public PlayerData GetPlayerData(string childSN)
        {
            List<PlayerData> playerDataList = this.GetAllPlayerData();
            if (playerDataList != null)
                return playerDataList.Find(t => t.child_sn == childSN);
            else
                return null;
        }

        /// <summary>
        /// 保存已收取的家人及好友的能量分成
        /// </summary>
        /// <param name="childSN"></param>
        public void SaveMpCollection(string childSN)
        {
            Dictionary<string, DateTime> mpCollections = this.GameDataHelper.GetObject<Dictionary<string, DateTime>>(COLLECT_MP_DATA_KEY);
            if (mpCollections == null)
                mpCollections = new Dictionary<string, DateTime>();

            if (mpCollections.ContainsKey(childSN))
                mpCollections[childSN] = DateTime.Today;
            else
                mpCollections.Add(childSN, DateTime.Today);

            this.GameDataHelper.SaveObject<Dictionary<string, DateTime>>(COLLECT_MP_DATA_KEY, mpCollections);
        }
        /// <summary>
        /// 判断今日是否已经收取了某个家人或好友的能量分成
        /// </summary>
        /// <param name="childSN"></param>
        /// <returns></returns>
        public bool MpCollected(string childSN)
        {
            Dictionary<string, DateTime> mpCollections = this.GameDataHelper.GetObject<Dictionary<string, DateTime>>(COLLECT_MP_DATA_KEY);
            if (mpCollections != null && mpCollections.ContainsKey(childSN) && mpCollections[childSN] == DateTime.Today)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 清空本地记录的家人及好友能量分成的收取
        /// </summary>
        public void ClearMpCollection()
        {
            this.GameDataHelper.Clear(COLLECT_MP_DATA_KEY);
        }
    }
}
