using AppGame.Data.Local;
using AppGame.Data.Model;
using strange.extensions.mediation.impl;
using System.Collections.Generic;
using UnityEngine;

namespace AppGame.Module.Cycling
{
    public class CyclingMediator : EventMediator
    {
        /************************************************属性与变量命名************************************************/
        [Inject]
        public CyclingView View { get; set; }
        [Inject]
        public ILocalChildInfoAgent LocalChildInfoAgent { get; set; }
        [Inject]
        public IBasicDataManager BasicDataManager { get; set; }
        [Inject]
        public ICyclingDataManager CyclingDataManager { get; set; }
        private List<BasicData> basicDataList = null;
        private OriginData originData = null;
        private List<PlayerData> playerDataList = null;
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        public override void OnRegister()
        {
            UpdateListeners(true);
            this.BuildTestData();
            this.GetGameData();
            this.RefreshMpDatas();
            this.InvokeRepeating("GetGameData", 3f, 3f);
            this.InvokeRepeating("RefreshOriginData", 3f, 5f);
            this.InvokeRepeating("RefreshFriendData", 3f, 15f);
        }

        public override void OnRemove()
        {
            UpdateListeners(false);
        }

        private void UpdateListeners(bool value)
        {
            //view.dispatcher.UpdateListener(value, GameStartView.CLICK_EVENT, onViewClicked);
            //dispatcher.UpdateListener(value, GameEvent.GAME_UPDATE, onGameUpdate);
            //dispatcher.UpdateListener(value, GameEvent.GAME_OVER, onGameOver);

            //dispatcher.AddListener(GameEvent.RESTART_GAME, onRestart);
        }

        private void onViewClicked()
        {
            //dispatcher.Dispatch(GameEvent.SHIP_DESTROYED);
        }

        private void onGameUpdate()
        {
        }

        private void onGameOver()
        {
            UpdateListeners(false);
        }

        private void onRestart()
        {
            OnRegister();
        }

        private void GetGameData()
        {
            //LocationDatas locationDatas = new LocationDatas();
            //locationDatas.Datas.Add(new LocationData() { UserID = "01", AvatarID = "", MapPointID = "320101_01" });
            //locationDatas.Datas.Add(new LocationData() { UserID = "02", AvatarID = "avatar06", MapPointID = "320101_34" });
            //locationDatas.Datas.Add(new LocationData() { UserID = "03", AvatarID = "avatar09", MapPointID = "320101_12" });
            //this.View.LocationDatas = locationDatas;

            this.basicDataList = this.BasicDataManager.GetAllData();
            this.originData = this.CyclingDataManager.GetOriginData(this.LocalChildInfoAgent.GetChildSN());
            this.playerDataList = this.CyclingDataManager.GetAllPlayerData();
        }

        private void BuildTestData()
        {
            //创建基础数据
            List<BasicData> basicDataList = new List<BasicData>();
            basicDataList.Add(new BasicData() { child_sn = "01", child_name = "樱木花道", child_avatar = "avatar06", relation = (int)Relations.Self });
            basicDataList.Add(new BasicData() { child_sn = "02", child_name = "仙道彰", child_avatar = "avatar09", relation = (int)Relations.Family });
            basicDataList.Add(new BasicData() { child_sn = "03", child_name = "流川枫", child_avatar = "avatar09", relation = (int)Relations.Friend });
            basicDataList.Add(new BasicData() { child_sn = "04", child_name = "牧绅一", child_avatar = "avatar06", relation = (int)Relations.Friend });
            this.BasicDataManager.SaveDataList(basicDataList);
            //创建原始数据
            OriginData originData = new OriginData() { child_sn = "01", walk = 10000, ride = 5000, train = 20, learn = 30 };
            this.CyclingDataManager.SaveOriginData(originData);
            //创建游戏数据
            List<PlayerData> playerDataList = new List<PlayerData>();
            playerDataList.Add(new PlayerData()
            {
                child_sn = "01",
                map_id = "320101",
                map_position = "320101_09",
                walk_expend = 5000,
                walk_today = 5000,
                ride_expend = 1000,
                train_expend = 0,
                learn_expend = 0,
                mp = 0,
                mp_expend = 0,
                mp_today = 0,
                mp_yestoday = 0,
                hp = 5
            });
            playerDataList.Add(new PlayerData()
            {
                child_sn = "02",
                map_id = "320101",
                map_position = "320101_15",
                mp_yestoday = 20
            });
            playerDataList.Add(new PlayerData()
            {
                child_sn = "03",
                map_id = "320101",
                map_position = "320101_21",
                mp_yestoday = 15
            });
            playerDataList.Add(new PlayerData()
            {
                child_sn = "04",
                map_id = "320101",
                map_position = "320101_27",
                mp_yestoday = 10
            });
            this.CyclingDataManager.SavePlayerDataList(playerDataList);
        }

        private void RefreshOriginData()
        {
            this.originData.walk += Random.Range(1000, 5000);
            this.originData.ride += Random.Range(1000, 5000);
            this.CyclingDataManager.SaveOriginData(this.originData);
            this.RefreshMpDatas();
        }

        private void RefreshFriendData()
        {
            int index = 1 + Random.Range(0, 10) % 3;
            int offset = Random.Range(1, 5);
            PlayerData playerData = this.playerDataList[index];
            int position = int.Parse(playerData.map_position.Substring(7, 2));
            playerData.map_position = string.Format("{0}_{1}", playerData.map_id, position + offset);
            this.CyclingDataManager.SavePlayerData(playerData);
            this.RefreshMpDatas();
        }

        private void RefreshMpDatas()
        {
            List<MpData> mpDatas = new List<MpData>();
            PlayerData myPlayerData = this.playerDataList.Find(t => t.child_sn == this.LocalChildInfoAgent.GetChildSN());

            int mpWalk = (int)((this.originData.walk - myPlayerData.walk_expend) / 500);
            if (mpWalk > 0) mpDatas.Add(new MpData() { MpBallType = MpBallTypes.Walk, Value = mpWalk });

            int mpRide = (int)((this.originData.ride - myPlayerData.ride_expend) / 500);
            if (mpRide > 0) mpDatas.Add(new MpData() { MpBallType = MpBallTypes.Walk, Value = mpRide });

            int mpTrain = (this.originData.train - myPlayerData.train_expend);
            if (mpTrain > 0) mpDatas.Add(new MpData() { MpBallType = MpBallTypes.Walk, Value = mpTrain });

            int mpLearn = (int)((this.originData.learn - myPlayerData.learn_expend) / 5);
            if (mpLearn > 0) mpDatas.Add(new MpData() { MpBallType = MpBallTypes.Walk, Value = mpLearn });

            foreach (var playerData in this.playerDataList)
            {
                if (playerData.child_sn == this.LocalChildInfoAgent.GetChildSN())
                    continue;

                BasicData basicData = this.basicDataList.Find(t => t.child_sn == playerData.child_sn);
                if (basicData == null)
                {
                    Debug.LogErrorFormat("<><CyclingMediator.RefreshMpBalls>Can not find the basic data of [0]", playerData.child_sn);
                    continue;
                }

                int mpShare = 0;
                if (basicData.relation == (int)Relations.Family)
                    mpShare = (int)System.Math.Ceiling(playerData.mp_yestoday * 0.05);
                else if (basicData.relation == (int)Relations.Friend)
                    mpShare = (int)System.Math.Ceiling(playerData.mp_yestoday * 0.01);

                if (mpShare > 0)
                {
                    mpDatas.Add(new MpData()
                    {
                        MpBallType = (basicData.relation == (int)Relations.Family ? MpBallTypes.Family : MpBallTypes.Friend),
                        Value = mpShare,
                        FromID = playerData.child_sn,
                        FromName = basicData.child_name
                    });
                }
            }
            this.View.RefreshMpBalls(mpDatas);
        }
    }
}

