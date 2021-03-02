using AppGame.Data.Local;
using AppGame.Data.Model;
using strange.extensions.dispatcher.eventdispatcher.api;
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
        private PlayerData myPlayerData
        {
            get
            {
                return this.playerDataList.Find(t => t.child_sn == this.LocalChildInfoAgent.GetChildSN());
            }
        }
        /************************************************Unity方法与事件***********************************************/
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                this.CyclingDataManager.ClearMpCollection();
            }
        }
        /************************************************自 定 义 方 法************************************************/
        public override void OnRegister()
        {
            UpdateListeners(true);
            this.Initialize();
        }

        public override void OnRemove()
        {
            UpdateListeners(false);
        }

        private void UpdateListeners(bool register)
        {
            this.dispatcher.UpdateListener(register, GameEvent.COLLECT_MP, this.CollectMp);
            this.dispatcher.UpdateListener(register, GameEvent.GO_CLICK, this.OnGo);
        }

        private void onGameStart()
        {
            OnRegister();
        }

        private void onGameOver()
        {
            OnRemove();
        }

        private void onViewClicked()
        {
            //dispatcher.Dispatch(GameEvent.SHIP_DESTROYED);
        }

        private void Initialize()
        {
            this.BuildTestData();
            this.GetGameData();
            this.View.RefreshPlayer(this.basicDataList, this.playerDataList);
            this.View.RefreshTeammates(this.basicDataList, this.playerDataList);
            this.View.RefreshMp(myPlayerData.mp - myPlayerData.mp_expend, myPlayerData.hp);//刷新Go按钮
            this.RefreshMpDatas();
            this.InvokeRepeating("GetGameData", 3f, 3f);
            this.InvokeRepeating("RefreshOriginData", 3f, 5f);
            this.InvokeRepeating("RefreshFriendData", 3f, 15f);
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
            basicDataList.Add(new BasicData() { child_sn = "01", child_name = "樱木花道", child_avatar = "6", relation = (int)Relations.Self });
            basicDataList.Add(new BasicData() { child_sn = "02", child_name = "赤木晴子", child_avatar = "9", relation = (int)Relations.Family });
            basicDataList.Add(new BasicData() { child_sn = "03", child_name = "仙道彰", child_avatar = "12", relation = (int)Relations.Friend });
            basicDataList.Add(new BasicData() { child_sn = "04", child_name = "流川枫", child_avatar = "15", relation = (int)Relations.Friend });
            basicDataList.Add(new BasicData() { child_sn = "05", child_name = "牧绅一", child_avatar = "19", relation = (int)Relations.Friend });
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
                map_position = "320101_01",
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
                mp_yestoday = 50
            });
            playerDataList.Add(new PlayerData()
            {
                child_sn = "03",
                map_id = "320101",
                map_position = "320101_21",
                mp_yestoday = 25
            });
            playerDataList.Add(new PlayerData()
            {
                child_sn = "04",
                map_id = "320101",
                map_position = "320101_27",
                mp_yestoday = 20
            });
            playerDataList.Add(new PlayerData()
            {
                child_sn = "05",
                map_id = "320101",
                map_position = "320101_33",
                mp_yestoday = 30
            });
            this.CyclingDataManager.SavePlayerDataList(playerDataList);
        }

        private void RefreshOriginData()
        {
            this.originData.walk += Random.Range(1000, 5000) * 10;
            this.originData.ride += Random.Range(1000, 5000) * 10;
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
            this.View.RefreshTeammates(this.basicDataList, this.playerDataList);
        }

        private void RefreshMpDatas()
        {
            List<MpData> mpDatas = new List<MpData>();

            int mpWalk = (int)((this.originData.walk - this.myPlayerData.walk_expend) / 500);
            mpDatas.Add(new MpData() { MpBallType = MpBallTypes.Walk, Value = mpWalk });

            int mpRide = (int)((this.originData.ride - this.myPlayerData.ride_expend) / 500);
            mpDatas.Add(new MpData() { MpBallType = MpBallTypes.Ride, Value = mpRide });

            int mpTrain = (this.originData.train - this.myPlayerData.train_expend);
            mpDatas.Add(new MpData() { MpBallType = MpBallTypes.Train, Value = mpTrain });

            int mpLearn = (int)((this.originData.learn - this.myPlayerData.learn_expend) / 5);
            mpDatas.Add(new MpData() { MpBallType = MpBallTypes.Learn, Value = mpLearn });

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
                if (!this.CyclingDataManager.MpCollected(playerData.child_sn))
                {//今日没有收取此家人或朋友的能量分成，才计算其的能量分成
                    if (basicData.relation == (int)Relations.Family)
                        mpShare = (int)System.Math.Ceiling(playerData.mp_yestoday * 0.05);
                    else if (basicData.relation == (int)Relations.Friend)
                        mpShare = (int)System.Math.Ceiling(playerData.mp_yestoday * 0.01);
                }

                mpDatas.Add(new MpData()
                {
                    MpBallType = (basicData.relation == (int)Relations.Family ? MpBallTypes.Family : MpBallTypes.Friend),
                    Value = mpShare,
                    FromID = playerData.child_sn,
                    FromName = basicData.child_name
                });
            }
            this.View.RefreshMpBalls(mpDatas);
        }

        private void CollectMp(IEvent evt)
        {
            if (evt == null || evt.data == null)
            {
                Debug.LogError("<><CyclingMediator.CollectMp>Error: parameter 'evt' or 'evt.data' is null");
                return;
            }

            MpBall mpBall = evt.data as MpBall;
            if (mpBall == null)
            {
                Debug.LogError("<><CyclingMediator.CollectMp>Error: parameter 'evt.data' is not the type MpBall");
                return;
            }

            int mpIncrease = mpBall.Value;//计算收取了多少能量

            //记录耗用的原始数据(步行，骑行，坐姿及能量分成)
            switch (mpBall.MpBallType)
            {
                case MpBallTypes.Walk:
                    this.myPlayerData.walk_expend += mpIncrease * 500;
                    break;
                case MpBallTypes.Ride:
                    this.myPlayerData.ride_expend += mpIncrease * 500;
                    break;
                case MpBallTypes.Train:
                    this.myPlayerData.train_expend += mpIncrease;
                    break;
                case MpBallTypes.Learn:
                    this.myPlayerData.learn_expend += mpIncrease * 5;
                    break;
                case MpBallTypes.Family:
                case MpBallTypes.Friend:
                    //记录已收取此家人或朋友的能量分成
                    if (!this.CyclingDataManager.MpCollected(mpBall.FromID))
                        this.CyclingDataManager.SaveMpCollection(mpBall.FromID);
                    break;
            }

            this.myPlayerData.mp += mpIncrease;//记录增加的能量值
            int hpIncrease = (int)((this.myPlayerData.mp - myPlayerData.mp_expend) / 100);//计算能量值是否可转换成行动点数
            if (hpIncrease > 0)//每满100可转换成1点行动点数
            {
                this.myPlayerData.mp_expend += hpIncrease * 100;//记录耗用的能量
                this.myPlayerData.hp += hpIncrease;//记录增加的行动点数
            }

            this.CyclingDataManager.SavePlayerData(this.myPlayerData);//保存数据
            this.RefreshMpDatas();//刷新能量气泡
            this.View.RefreshMp(this.myPlayerData.mp - this.myPlayerData.mp_expend, myPlayerData.hp);//刷新Go按钮
        }

        private void OnGo()
        {
            int hp = this.myPlayerData.hp;
            if (hp > 0)
            {
                this.myPlayerData.hp -= 1;
                this.CyclingDataManager.SavePlayerData(this.myPlayerData);
            }
            this.View.Move(hp > 0, this.myPlayerData.hp);
        }
    }
}

