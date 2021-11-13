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
        [Inject]
        public IChildInfoManager ChildInfoManager { get; set; }
        [Inject]
        public IBasicDataManager BasicDataManager { get; set; }

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
        /// 判断是否有游戏数据(即是否第一次玩游戏)
        /// </summary>
        /// <returns></returns>
        public bool HasPlayerData()
        {
            List<PlayerData> playerDataList = this.GetAllPlayerData();
            if (playerDataList != null && playerDataList.Count > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 创建默认数据
        /// </summary>
        /// <returns></returns>
        public List<PlayerData> BuildGameData()
        {
            string childSN = this.ChildInfoManager.GetChildSN();
            //BasicData basicData = this.BasicDataManager.GetData(childSN);
            //string childName = basicData.child_name;
            //string childAvatar = basicData.child_avatar;

            //创建原始数据
            OriginData originData = new OriginData() { child_sn = childSN, walk = 10000, ride = 5000, train = 20, monitor = 30 };
            this.SaveOriginData(originData);
            //创建游戏数据
            List<PlayerData> playerDataList = new List<PlayerData>();
            playerDataList.Add(new PlayerData()
            {
                child_sn = childSN,
                child_name = "樱木花道",
                child_avatar = "6",
                relation = (int)Relations.Self,
                map_id = "3201",
                map_position = "3201_50",
                walk_expend = 5000,
                walk_today = 5000,
                ride_expend = 1000,
                train_expend = 0,
                learn_expend = 0,
                mp = 0,
                mp_expend = 0,
                mp_today = 0,
                mp_date = System.DateTime.Today,
                mp_yestoday = 0,
                hp = 5
            });
            playerDataList.Add(new PlayerData()
            {
                child_sn = "02",
                child_name = "赤木晴子",
                child_avatar = "9",
                relation = (int)Relations.Family,
                map_id = "3202",
                map_position = "3202_15",
                mp_yestoday = 50
            });
            playerDataList.Add(new PlayerData()
            {
                child_sn = "03",
                child_name = "仙道彰",
                child_avatar = "12",
                relation = (int)Relations.Friend,
                map_id = "3201",
                map_position = "3201_21",
                mp_yestoday = 25
            });
            playerDataList.Add(new PlayerData()
            {
                child_sn = "04",
                child_name = "流川枫",
                child_avatar = "15",
                relation = (int)Relations.Friend,
                map_id = "3201",
                map_position = "3201_27",
                mp_yestoday = 20
            });
            playerDataList.Add(new PlayerData()
            {
                child_sn = "05",
                child_name = "牧绅一",
                child_avatar = "19",
                relation = (int)Relations.Friend,
                map_id = "3202",
                map_position = "3202_33",
                mp_yestoday = 30
            });
            this.SavePlayerDataList(playerDataList);
            return playerDataList;
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

        /// <summary>
        /// 删除所有数据
        /// </summary>
        public void ClearAllData()
        {
            this.GameDataHelper.Clear(ORIGIN_DATA_DATA_KEY);
            this.GameDataHelper.Clear(PLAYER_DATA_DATA_KEY);
            this.GameDataHelper.Clear(COLLECT_MP_DATA_KEY);
            this.GameDataHelper.Clear(YESTERDAY_MP_DATA_KEY);
        }


    }
}
