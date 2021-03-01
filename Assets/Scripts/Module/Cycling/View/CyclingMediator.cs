using AppGame.Data.Local;
using AppGame.Data.Model;
using AppGame.Data.Remote;
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
        [Inject]
        public IAuthenticationUtils AuthenticationUtils { get; set; }
        [Inject]
        public ICyclingDataUtil CyclingDataUtil { get; set; }


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
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                LoginTestData loginTestData = GameObject.FindObjectOfType<LoginTestData>();
                this.AuthenticationUtils.GetVerifyCode(loginTestData.Phone, (success) =>
                {
                    Debug.LogFormat("<><CyclingMediator.Update>GetVerifyCode, success: {0}", success.info);
                }, (failure) =>
                {
                    Debug.LogFormat("<><CyclingMediator.Update>GetVerifyCode, failure: {0}", failure.ErrorInfo);
                });
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                LoginTestData loginTestData = GameObject.FindObjectOfType<LoginTestData>();
                LoginData loginData = new LoginData()
                {
                    Name = "早起的蚂蚁",
                    Password = "bobo123456",
                    Email = "54763755@qq.com",
                    Phone = loginTestData.Phone,
                    VerifyCode = loginTestData.VerifyCode
                };

                this.AuthenticationUtils.Login(loginData, (success) =>
                {
                    Debug.LogFormat("<><CyclingMediator.Update>Login, success: {0}", success.info);
                }, (failure) =>
                {
                    Debug.LogFormat("<><CyclingMediator.Update>Login, failure: {0}", failure.ErrorInfo);
                });
            }
        }
        /************************************************自 定 义 方 法************************************************/
        public override void OnRegister()
        {
            UpdateListeners(true);
            this.Initialize();

            this.CyclingDataUtil.GetBasicData((basicDataList) =>
            {
                Debug.LogFormat("<><CyclingMediator.OnRegister>GetBasicData, success: {0}", basicDataList);
            }, (errorText) =>
            {
                Debug.LogFormat("<><CyclingMediator.OnRegister>GetBasicData, failure: {0}", errorText);
            });
        }

        public override void OnRemove()
        {
            UpdateListeners(false);
        }

        private void UpdateListeners(bool value)
        {
            //dispatcher.UpdateListener(value, GameEvent.GAME_START, this.onGameStart);
            //dispatcher.UpdateListener(value, GameEvent.GAME_OVER, onGameOver);
            //view.dispatcher.UpdateListener(value, GameStartView.CLICK_EVENT, onViewClicked);

            if (value)
            {
                this.View.CollectMpSignal.AddListener(this.CollectMp);
                this.View.GoSignal.AddListener(this.OnGo);
            }
            else
            {
                this.View.CollectMpSignal.RemoveListener(this.CollectMp);
                this.View.GoSignal.RemoveListener(this.OnGo);
            }
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

        private void CollectMp(MpBall mpBall)
        {
            int mpIncrease = mpBall.Value;//计算收取了多少能量

            //记录好用的原始数据(步行，骑行，坐姿及能量分成)
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
                    //Todo: 记录已收取此家人的能量分成
                    break;
                case MpBallTypes.Friend:
                    //Todo: 记录已收取此朋友的能量分成
                    break;
            }

            this.myPlayerData.mp += mpIncrease;//记录增加的能量值
            int hpIncrease = (int)((this.myPlayerData.mp - myPlayerData.mp_expend) / 100);//计算能量值是否可转换成行动点数
            if (hpIncrease > 0)//每满100可转换成1点行动点数
            {
                this.myPlayerData.mp_expend += hpIncrease * 100;//记录好用的能量
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

