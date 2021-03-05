using AppGame.Data.Local;
using AppGame.Data.Model;
using AppGame.Data.Remote;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using System.Collections.Generic;
using UnityEngine;

namespace AppGame.Module.Cycling
{
    public class CyclingMediator : EventMediator
    {
        /************************************************属性与变量命名************************************************/
        #region 注入接口
        [Inject]
        public CyclingView View { get; set; }
        [Inject]
        public IAuthenticationUtils AuthenticationUtils { get; set; }
        [Inject]
        public ICyclingDataUtil CyclingDataUtil { get; set; }
        [Inject]
        public IChildInfoManager ChildInfoManager { get; set; }
        [Inject]
        public IDeviceInfoManager DeviceInfoManager { get; set; }
        [Inject]
        public IItemDataManager ItemDataManager { get; set; }
        [Inject]
        public IBasicDataManager BasicDataManager { get; set; }
        [Inject]
        public ICyclingDataManager CyclingDataManager { get; set; }
        #endregion
        #region 其他变量
        private List<BasicData> basicDataList = null;
        private OriginData originData = null;
        private List<PlayerData> playerDataList = null;
        private PlayerData myPlayerData
        {
            get
            {
                return this.playerDataList.Find(t => t.child_sn == this.ChildInfoManager.GetChildSN());
            }
        }
        #endregion
        /************************************************Unity方法与事件***********************************************/
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                this.CyclingDataManager.ClearMpCollection();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                int coin = GameObject.FindObjectOfType<TestData>().Coin;
                this.ItemDataManager.SetItem(Items.COIN, coin);
                this.View.Coin = coin;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                TestData testData = GameObject.FindObjectOfType<TestData>();
                this.AuthenticationUtils.GetVerifyCode(testData.Phone, (success) =>
                {
                    Debug.LogFormat("<><CyclingMediator.Update>GetVerifyCode, success: {0}", success.info);
                }, (failure) =>
                {
                    Debug.LogFormat("<><CyclingMediator.Update>GetVerifyCode, failure: {0}", failure.ErrorInfo);
                });
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                TestData loginTestData = GameObject.FindObjectOfType<TestData>();
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
        //注册
        public override void OnRegister()
        {
            UpdateListeners(true);
            this.Initialize();

            //this.CyclingDataUtil.GetBasicData((basicData) =>
            //{
            //    Debug.LogFormat("<><CyclingMediator.OnRegister>GetBasicData, success: {0}", basicData);
            //}, (errorText) =>
            //{
            //    Debug.LogFormat("<><CyclingMediator.OnRegister>GetBasicData, failure: {0}", errorText);
            //});

            //this.CyclingDataUtil.GetGameData((playerDataList) =>
            //{
            //    Debug.LogFormat("<><CyclingMediator.OnRegister>GetGameData, success: {0}", playerDataList != null ? playerDataList.Count : 0);
            //}, (errorText) =>
            //{
            //    Debug.LogFormat("<><CyclingMediator.OnRegister>GetGameData, failure: {0}", errorText);
            //});
        }
        //取消注册
        public override void OnRemove()
        {
            UpdateListeners(false);
        }
        //注册事件监听
        private void UpdateListeners(bool register)
        {
            this.dispatcher.UpdateListener(register, GameEvent.MP_CLICK, this.OnMpBallClick);
            this.dispatcher.UpdateListener(register, GameEvent.GO_CLICK, this.OnGo);
            this.dispatcher.UpdateListener(register, GameEvent.COLLECT_MP, this.OnCollectMp);
        }
        //初始化
        private void Initialize()
        {
            this.BuildTestData();
            this.GetGameData();
            this.View.RefreshPlayer(this.basicDataList, this.playerDataList, this.ItemDataManager.GetItemCount(Items.COIN));
            this.View.RefreshTeammates(this.basicDataList, this.playerDataList);
            this.View.RefreshMp(myPlayerData.mp - myPlayerData.mp_expend, myPlayerData.hp);//刷新Go按钮
            this.RefreshMpDatas();
            this.InvokeRepeating("GetGameData", 3f, 3f);
            this.InvokeRepeating("RefreshOriginData", 3f, 5f);
            this.InvokeRepeating("RefreshFriendData", 3f, 15f);
        }
        //获取游戏数据
        private void GetGameData()
        {
            this.basicDataList = this.BasicDataManager.GetAllData();
            this.originData = this.CyclingDataManager.GetOriginData(this.ChildInfoManager.GetChildSN());
            this.playerDataList = this.CyclingDataManager.GetAllPlayerData();
        }
        //创建模拟数据
        private void BuildTestData()
        {
            string childSN = this.ChildInfoManager.GetChildSN();
            //创建基础数据
            List<BasicData> basicDataList = new List<BasicData>();
            basicDataList.Add(new BasicData() { child_sn = childSN, child_name = "樱木花道", child_avatar = "6", relation = (int)Relations.Self });
            basicDataList.Add(new BasicData() { child_sn = "02", child_name = "赤木晴子", child_avatar = "9", relation = (int)Relations.Family });
            basicDataList.Add(new BasicData() { child_sn = "03", child_name = "仙道彰", child_avatar = "12", relation = (int)Relations.Friend });
            basicDataList.Add(new BasicData() { child_sn = "04", child_name = "流川枫", child_avatar = "15", relation = (int)Relations.Friend });
            basicDataList.Add(new BasicData() { child_sn = "05", child_name = "牧绅一", child_avatar = "19", relation = (int)Relations.Friend });
            this.BasicDataManager.SaveDataList(basicDataList);
            //创建原始数据
            OriginData originData = new OriginData() { child_sn = childSN, walk = 10000, ride = 5000, train = 20, learn = 30 };
            this.CyclingDataManager.SaveOriginData(originData);
            //创建游戏数据
            List<PlayerData> playerDataList = new List<PlayerData>();
            playerDataList.Add(new PlayerData()
            {
                child_sn = childSN,
                map_id = "320101",
                map_position = "320101_10",
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
        //刷新健康数据
        private void RefreshOriginData()
        {
            this.originData.walk += Random.Range(1000, 5000) * 30;
            this.originData.ride += Random.Range(1000, 5000) * 30;
            this.CyclingDataManager.SaveOriginData(this.originData);
            this.RefreshMpDatas();
        }
        //刷新亲友数据
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
        //刷新能量数据
        private void RefreshMpDatas()
        {
            List<MpData> mpDatas = new List<MpData>();

            int mpWalk = (int)((this.originData.walk - this.myPlayerData.walk_expend) / 500);
            mpDatas.Add(new MpData() { MpBallType = MpBallTypes.Walk, Mp = mpWalk });

            int mpRide = (int)((this.originData.ride - this.myPlayerData.ride_expend) / 500);
            mpDatas.Add(new MpData() { MpBallType = MpBallTypes.Ride, Mp = mpRide });

            int mpTrain = (this.originData.train - this.myPlayerData.train_expend);
            mpDatas.Add(new MpData() { MpBallType = MpBallTypes.Train, Mp = mpTrain });

            int mpLearn = (int)((this.originData.learn - this.myPlayerData.learn_expend) / 5);
            mpDatas.Add(new MpData() { MpBallType = MpBallTypes.Learn, Mp = mpLearn });

            foreach (var playerData in this.playerDataList)
            {
                if (playerData.child_sn == this.ChildInfoManager.GetChildSN())
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
                    Mp = mpShare,
                    FromID = playerData.child_sn,
                    FromName = basicData.child_name
                });
            }
            this.View.RefreshMpBalls(mpDatas);
        }
        //当能量球被点击时
        private void OnMpBallClick(IEvent evt)
        {
            if (evt == null || evt.data == null)
            {
                Debug.LogError("<><CyclingMediator.OnMpBallClick>Error: parameter 'evt' or 'evt.data' is null");
                return;
            }

            MpBall mpBall = evt.data as MpBall;
            if (mpBall == null)
            {
                Debug.LogError("<><CyclingMediator.OnMpBallClick>Error: parameter 'evt.data' is not the type MpBall");
                return;
            }

            if (this.myPlayerData.mp_today > this.DeviceInfoManager.GetMpLimit())
            {
                int coin = this.ItemDataManager.GetItemCount(Items.COIN);
                int min = Mathf.Min(coin, mpBall.Value);
                MpData mpData = mpBall.ToMpData();
                mpData.Mp = min;
                mpData.Coin = min;
                mpData.CoinEnough = min > 0;
                this.View.ShowPayBill(mpData);
            }
            else
            {
                this.dispatcher.Dispatch(GameEvent.COLLECT_MP, mpBall.ToMpData());
            }
        }
        //收取能量
        private void OnCollectMp(IEvent evt)
        {
            if (evt == null || evt.data == null)
            {
                Debug.LogError("<><CyclingMediator.OnCollectMp>Error: parameter 'evt' or 'evt.data' is null");
                return;
            }

            MpData mpData = evt.data as MpData;
            if (mpData == null)
            {
                Debug.LogError("<><CyclingMediator.OnCollectMp>Error: parameter 'evt.data' is not the type MpData");
                return;
            }

            //扣减相应的金币
            if (mpData.Coin > 0)
            {
                this.ItemDataManager.ReduceItem(Items.COIN, mpData.Coin);
                this.View.Coin = this.ItemDataManager.GetItemCount(Items.COIN);
            }

            //记录耗用的原始数据(步行，骑行，坐姿及能量分成)
            switch (mpData.MpBallType)
            {
                case MpBallTypes.Walk:
                    this.myPlayerData.walk_expend += mpData.Mp * 500;
                    break;
                case MpBallTypes.Ride:
                    this.myPlayerData.ride_expend += mpData.Mp * 500;
                    break;
                case MpBallTypes.Train:
                    this.myPlayerData.train_expend += mpData.Mp;
                    break;
                case MpBallTypes.Learn:
                    this.myPlayerData.learn_expend += mpData.Mp * 5;
                    break;
                case MpBallTypes.Family:
                case MpBallTypes.Friend:
                    //记录已收取此家人或朋友的能量分成
                    if (!this.CyclingDataManager.MpCollected(mpData.FromID))
                        this.CyclingDataManager.SaveMpCollection(mpData.FromID);
                    break;
            }

            this.myPlayerData.mp += mpData.Mp;//记录增加的能量值
            if (this.myPlayerData.mp_date == System.DateTime.Today)
                this.myPlayerData.mp_today += mpData.Mp;//记录增加的能量值
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
        //当Go按钮被点击时
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
