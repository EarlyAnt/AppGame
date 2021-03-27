using AppGame.Config;
using AppGame.Data.Local;
using AppGame.Data.Model;
using AppGame.Data.Remote;
using AppGame.UI;
using strange.extensions.dispatcher.eventdispatcher.api;
using System.Collections.Generic;
using UnityEngine;

namespace AppGame.Module.Cycling
{
    public class CyclingMediator : BaseMediator
    {
        /************************************************属性与变量命名************************************************/
        #region 注入接口
        [Inject]
        public CyclingView View { get; set; }
        [Inject]
        public IMapConfig MapConfig { get; set; }
        [Inject]
        public IScenicConfig ScenicConfig { get; set; }
        [Inject]
        public ICardConfig CardConfig { get; set; }
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
                this.ItemDataManager.Clear(true);
                this.ItemDataManager.AddItem(Items.COIN, 1000);
                this.BuildTestData();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                this.View.Restart();
                this.Initialize();
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
#if (UNITY_ANDROID) && (!UNITY_EDITOR)
            this.CyclingDataManager.ClearMpCollection();
            this.ItemDataManager.Clear(true);
            this.ItemDataManager.AddItem(Items.COIN, 1000);
            this.BuildTestData();
#endif
            this.Initialize();
            #region 访问服务器获取数据
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
            #endregion
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
            this.dispatcher.UpdateListener(register, GameEvent.INTERACTION, this.OnPlayerStopped);
            this.dispatcher.UpdateListener(register, GameEvent.SCENIC_CARD_CLOSE, this.OnScenicCardClosed);
            this.dispatcher.UpdateListener(register, GameEvent.CITY_STATION_CLOSE, this.OnCityStationClosed);
        }
        //初始化
        private void Initialize()
        {
            this.GetGameData();
            this.RefreshMapInfo();
            this.View.LoadMap(this.myPlayerData.map_id);
            this.View.RefreshPlayer(this.myPlayerData, this.ItemDataManager.GetItemCount(Items.COIN));
            this.View.RefreshTeammates(this.playerDataList);
            this.View.RefreshMp(myPlayerData.mp - myPlayerData.mp_expend, myPlayerData.hp);//刷新Go按钮
            this.RefreshMpDatas();
            this.CancelInvoke();
            this.InvokeRepeating("GetGameData", 3f, 3f);
            this.InvokeRepeating("RefreshOriginData", 3f, 3f);
            this.InvokeRepeating("RefreshFriendData", 3f, 15f);
        }
        //获取游戏数据
        private void GetGameData()
        {
            this.originData = this.CyclingDataManager.GetOriginData(this.ChildInfoManager.GetChildSN());
            this.playerDataList = this.CyclingDataManager.GetAllPlayerData();
        }
        //创建模拟数据
        private void BuildTestData()
        {
            string childSN = this.ChildInfoManager.GetChildSN();
            //创建原始数据
            OriginData originData = new OriginData() { child_sn = childSN, walk = 10000, ride = 5000, train = 20, learn = 30 };
            this.CyclingDataManager.SaveOriginData(originData);
            //创建游戏数据
            List<PlayerData> playerDataList = new List<PlayerData>();
            playerDataList.Add(new PlayerData()
            {
                child_sn = childSN,
                child_name = "樱木花道",
                child_avatar = "6",
                relation = (int)Relations.Self,
                map_id = "3201",
                map_position = "3201_46",
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
            this.CyclingDataManager.SavePlayerDataList(playerDataList);
        }
        //刷新健康数据
        private void RefreshOriginData()
        {
            this.originData.walk += Random.Range(1000, 25000) * 3;
            this.originData.ride += Random.Range(1000, 25000) * 3;
            this.CyclingDataManager.SaveOriginData(this.originData);
            this.RefreshMpDatas();
        }
        //刷新亲友数据
        private void RefreshFriendData()
        {
            int pointCount = this.View.Player.MapNode.Points.Count;
            MapPointNode endPointNode = this.View.Player.MapNode.Points[pointCount - 1].GetComponent<MapPointNode>();
            int maxPointIndex = int.Parse(endPointNode.ID.Substring(5, 2));

            int index = 1 + Random.Range(0, 10) % 3;
            int offset = Random.Range(1, 5);
            PlayerData playerData = this.playerDataList[index];
            int position = int.Parse(playerData.map_position.Substring(5, 2));
            position = Mathf.Clamp(position + offset, 1, maxPointIndex);//不能超过地图上最大的编号
            playerData.map_position = string.Format("{0}_{1:d2}", playerData.map_id, position);
            this.CyclingDataManager.SavePlayerData(playerData);
            this.View.RefreshTeammates(this.playerDataList);
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

                int mpShare = 0;
                if (!this.CyclingDataManager.MpCollected(playerData.child_sn))
                {//今日没有收取此家人或朋友的能量分成，才计算其的能量分成
                    if (playerData.relation == (int)Relations.Family)
                        mpShare = (int)System.Math.Ceiling(playerData.mp_yestoday * 0.05);
                    else if (playerData.relation == (int)Relations.Friend)
                        mpShare = (int)System.Math.Ceiling(playerData.mp_yestoday * 0.01);
                }

                mpDatas.Add(new MpData()
                {
                    MpBallType = (playerData.relation == (int)Relations.Family ? MpBallTypes.Family : MpBallTypes.Friend),
                    Mp = mpShare,
                    FromID = playerData.child_sn,
                    FromName = playerData.child_name
                });
            }
            this.View.RefreshMpBalls(mpDatas);
        }
        //刷新地图信息
        private void RefreshMapInfo()
        {
            MapInfo mapInfo = this.MapConfig.GetMap(this.myPlayerData.map_id);
            if (mapInfo == null)
            {
                Debug.LogErrorFormat("<><CyclingMediator.RefreshMapInfo>Error: can not find the map [{0}]", this.myPlayerData.map_id);
                return;
            }

            List<ScenicInfo> scenicInfos = this.ScenicConfig.GetScenics(this.myPlayerData.map_id);
            if (scenicInfos == null || scenicInfos.Count == 0)
            {
                Debug.LogErrorFormat("<><CyclingMediator.RefreshMapInfo>Error: can not find scenics of the map [{0}]", this.myPlayerData.map_id);
                return;
            }

            int scenicCount = scenicInfos.Count;
            int cardCount = 0;
            scenicInfos.ForEach(t =>
            {
                CardInfo cardInfo = this.CardConfig.GetCardByScenicID(t.ID);
                if (cardInfo != null && this.ItemDataManager.HasItem(cardInfo.CardID))
                    cardCount += 1;
            });
            this.View.RefreshMapProgress(mapInfo.CityName, cardCount, scenicCount);
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

            MpData mpData = mpBall.ToMpData();
            bool overLimit = this.myPlayerData.mp_today >= this.DeviceInfoManager.GetMpLimit();
            if (overLimit && mpData.RefreshView)
            {//超出每日能量收取上限时，弹出支付手续费页面
                int coin = this.ItemDataManager.GetItemCount(Items.COIN);
                int min = Mathf.Min(coin, mpBall.Value);
                mpData.Mp = min;
                mpData.Coin = min;
                mpData.CoinEnough = min > 0;
                this.View.ShowPayBill(mpData);
            }
            else if (!overLimit)
            {//没有超出每日能量收取上限时，也要检查当日已经收取了多少能量，还能收取多少能量
                int validMp = Mathf.Min(mpBall.Value, this.DeviceInfoManager.GetMpLimit() - this.myPlayerData.mp_today);
                mpData.Mp = validMp;
                this.dispatcher.Dispatch(GameEvent.COLLECT_MP, mpData);
                Debug.LogFormat("<><>mp_today: {0}, mpLimit: {1}, leftMp: {2}, validMp: {3}",
                                this.myPlayerData.mp_today, this.DeviceInfoManager.GetMpLimit(),
                                this.DeviceInfoManager.GetMpLimit() - this.myPlayerData.mp_today, validMp);
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

            if (mpData.RefreshView)
                this.RefreshMpDatas();//玩家点击能量气泡主动收取能量时才立刻刷新能量气泡
            this.View.RefreshMp(this.myPlayerData.mp - this.myPlayerData.mp_expend, myPlayerData.hp);//刷新Go按钮
        }
        //当玩家前进停止时
        private void OnPlayerStopped(IEvent evt)
        {
            if (evt == null || evt.data == null)
            {
                Debug.LogError("<><CyclingMediator.OnPlayerStopped>Error: parameter 'evt' or 'evt.data' is null");
                return;
            }

            MapPointNode mapPointNode = evt.data as MapPointNode;
            if (mapPointNode == null)
            {
                Debug.LogError("<><CyclingMediator.OnPlayerStopped>Error: parameter 'evt.data' is not the type MapPointNode");
                return;
            }

            //记录玩家当前位置
            this.myPlayerData.map_position = mapPointNode.ID;
            this.CyclingDataManager.SavePlayerData(this.myPlayerData);

            //检测是否有卡片需要显示
            if (mapPointNode.NodeType == NodeTypes.EndNode)
            {
                //Todo: 处理到站时的数据逻辑
                this.View.Interact(mapPointNode, this.myPlayerData);
            }
            else if (mapPointNode.NodeType == NodeTypes.SiteNode)
            {
                InteractionData interactionData = mapPointNode.GetComponent<InteractionData>();
                if (interactionData != null && interactionData.Interacton == Interactions.KNOWLEDGE_LANDMARK)
                {//记录卡片进度
                    CardInfo cardInfo = this.CardConfig.GetCardByScenicID(interactionData.ID);
                    if (cardInfo != null)
                    {
                        this.ItemDataManager.SetItem(cardInfo.CardID, 1);
                        this.View.Interact(mapPointNode, this.myPlayerData);
                    }
                    else
                    {
                        Debug.LogErrorFormat("<><CyclingMediator.OnPlayerStopped>Error: can not find the card that its scenicID is {0}", interactionData.ID);
                    }
                }
                else
                {
                    Debug.LogErrorFormat("<><CyclingMediator.OnPlayerStopped>Error: can not find the scenic that its scenicID is {0}", interactionData.ID);
                }
            }
            else
            {
                this.View.Interact(mapPointNode, this.myPlayerData);
            }
        }
        //当景点卡片关闭时
        private void OnScenicCardClosed(IEvent evt)
        {
            this.RefreshMapInfo();
        }
        //当交通工具选择页面关闭时
        private void OnCityStationClosed(IEvent evt)
        {
            if (evt == null || evt.data == null)
            {
                Debug.LogError("<><CyclingMediator.OnCityStationClosed>Error: parameter 'evt' or 'evt.data' is null");
                return;
            }

            Ticket ticket = evt.data as Ticket;
            if (ticket == null)
            {
                Debug.LogError("<><CyclingMediator.OnCityStationClosed>Error: parameter 'evt.data' is not the type Ticket");
                return;
            }

            if (ticket.Go)
            {
                this.myPlayerData.map_id = ticket.ToMapID;
                this.myPlayerData.map_position = string.Format("{0}_01", ticket.ToMapID);
                this.myPlayerData.hp -= ticket.Hp;
                this.CyclingDataManager.SavePlayerData(this.myPlayerData);
                this.ItemDataManager.ReduceItem(Items.COIN, ticket.Coin);
                this.View.ShowLoading(ticket);
                this.Initialize();
                this.DelayInvoke(() => this.View.HideLoading(), Random.Range(1.5f, 3f));
            }
            else
            {
                this.View.Stay();
            }
        }
        //当Go按钮被点击时
        private void OnGo()
        {
            int hp = this.myPlayerData.hp;
            if (hp > 0 && this.View.Player.CurrentNode.NodeType != NodeTypes.EndNode)
            {
                this.myPlayerData.hp -= 1;
                this.CyclingDataManager.SavePlayerData(this.myPlayerData);
            }
            this.View.Move(hp > 0, this.myPlayerData.hp);
        }
    }
}
