using AppGame.Config;
using AppGame.Data.Local;
using AppGame.Data.Model;
using AppGame.Global;
using AppGame.UI;
using DG.Tweening;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.signal.impl;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class CyclingView : BaseView
    {
        /************************************************属性与变量命名************************************************/
        #region 注入接口
        [Inject]
        public IModuleConfig ModuleConfig { get; set; }
        [Inject]
        public IMapConfig MapConfig { get; set; }
        [Inject]
        public IChildInfoManager ChildInfoManager { get; set; }
        [Inject]
        public ICommonImageUtils CommonImageUtils { get; set; }
        #endregion
        #region 页面UI组件
        [SerializeField]
        private Image mask;
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private Player player;
        [SerializeField]
        private Transform teammateRoot;
        [SerializeField]
        private Teammate teammatePrefab;
        [SerializeField]
        private Image avatarBox;
        [SerializeField]
        private Text nameBox;
        [SerializeField]
        private Text coinBox;
        [SerializeField]
        private Transform mpBallRoot;
        [SerializeField]
        private MpBall mpBallPrefab1;
        [SerializeField]
        private MpBall mpBallPrefab2;
        [SerializeField]
        private Text mpBox;
        [SerializeField]
        private Text hpBox;
        [SerializeField]
        private ScenicCard scenicCard;
        [SerializeField]
        private CityStation cityStation;
        #endregion
        #region 其他变量
        private bool playerCanGo = true;
        private bool hideMpBalls = false;
        private float halfWidth = 380f;
        private float halfHeight = 800f;
        private List<Teammate> teammates;
        private List<MpBall> mpBalls;
        #endregion
        /************************************************Unity方法与事件***********************************************/
        protected override void Awake()
        {
            base.Awake();

            this.mpBalls = new List<MpBall>();

            while (this.mpBallRoot.childCount > 0)
                GameObject.DestroyImmediate(this.mpBallRoot.GetChild(0).gameObject);
        }
        protected override void Start()
        {
            base.Start();
            this.Initialize();
            this.UpdateDispatcher(true);
        }
        protected override void OnDestroy()
        {
            this.UpdateDispatcher(false);
            base.OnDestroy();
        }
        /************************************************自 定 义 方 法************************************************/
        private void Initialize()
        {
            this.DelayInvoke(() =>
            {
                this.mask.DOFade(0f, 1f);
                this.canvasGroup.DOFade(1f, 1f);
            }, 1f);

            this.StartCoroutine(this.LoadModuleFiles(ModuleViews.Cycling));
        }
        public void Go()
        {
            if (this.playerCanGo)
            {
                this.playerCanGo = false;
                this.dispatcher.Dispatch(GameEvent.GO_CLICK);
            }
        }
        public void Move(bool canMove, int hp)
        {
            if (canMove)
            {
                this.SetMpBallVisible(false);
                this.hpBox.text = hp.ToString();
                this.player.MoveForward();
            }
            else
            {
                this.playerCanGo = true;
                this.SetMpBallVisible(true);
                //Todo: 显示行动点数不足的提示
            }
        }
        public void RefreshPlayer(List<BasicData> basicDataList, List<PlayerData> playerDataList)
        {
            PlayerData myPlayerData = playerDataList.Find(t => t.child_sn == this.ChildInfoManager.GetChildSN());
            BasicData myBasicData = basicDataList.Find(t => t.child_sn == this.ChildInfoManager.GetChildSN());
            this.player.MoveToNode(myPlayerData.map_position);
            this.player.name = "Player_" + myPlayerData.child_sn;
            Sprite avatar = this.CommonImageUtils.GetAvatar(myBasicData.child_avatar);
            this.player.Avatar = avatar;
            this.avatarBox.sprite = avatar;
            this.nameBox.text = myBasicData.child_name;
        }
        public void RefreshTeammates(List<BasicData> basicDataList, List<PlayerData> playerDataList)
        {
            if (this.teammates == null)
            {
                this.teammates = new List<Teammate>();
                foreach (var teammateData in playerDataList)
                {
                    if (teammateData.child_sn == this.ChildInfoManager.GetChildSN())
                        continue;

                    Teammate teammate = GameObject.Instantiate<Teammate>(this.teammatePrefab, this.teammateRoot);
                    BasicData basicData = basicDataList.Find(t => t.child_sn == teammateData.child_sn);
                    teammate.name = "Teammate_" + teammateData.child_sn;
                    teammate.Avatar = this.CommonImageUtils.GetAvatar(basicData.child_avatar);
                    teammate.MoveToNode(teammateData.map_position);
                    this.teammates.Add(teammate);
                }
            }
            else
            {
                foreach (var teammateData in playerDataList)
                {
                    if (teammateData.child_sn == this.ChildInfoManager.GetChildSN())
                        continue;

                    Teammate teammate = this.teammates.Find(t => t.name == "Teammate_" + teammateData.child_sn);
                    if (teammate != null) teammate.MoveToNode(teammateData.map_position);
                }
            }
        }
        public void RefreshMpBalls(List<MpData> mpDatas)
        {
            if (this.player.IsMoving)
                return;

            foreach (var mpData in mpDatas)
            {
                MpBall mpBall = this.mpBalls.Find(t => t.MpBallType == mpData.MpBallType && t.FromID == mpData.FromID);
                if (mpBall == null && mpData.Value > 0)
                {
                    MpBall prefab = mpData.MpBallType == MpBallTypes.Family || mpData.MpBallType == MpBallTypes.Friend ? this.mpBallPrefab2 : this.mpBallPrefab1;
                    MpBall newMpBall = GameObject.Instantiate(prefab, this.mpBallRoot);
                    newMpBall.MpBallType = mpData.MpBallType;
                    if (newMpBall.MpBallType == MpBallTypes.Family || newMpBall.MpBallType == MpBallTypes.Friend)
                    {
                        newMpBall.FromID = mpData.FromID;
                        newMpBall.FromName = mpData.FromName;
                    }
                    newMpBall.Value = mpData.Value;
                    newMpBall.transform.localRotation = Quaternion.identity;
                    newMpBall.transform.localScale = Vector3.one * Random.Range(0.7f, 1.0f);
                    newMpBall.transform.localPosition = this.GetRandomPosition();
                    newMpBall.OnCollectMp = this.CollectMp;
                    newMpBall.SetStatus(!this.hideMpBalls);
                    this.mpBalls.Add(newMpBall);
                }
                else if (mpBall != null)
                {
                    mpBall.Value = mpData.Value;
                    if (mpBall.Value <= 0)
                    {
                        this.mpBalls.Remove(mpBall);
                        GameObject.DestroyImmediate(mpBall.gameObject);
                    }
                    else if (!this.hideMpBalls)
                    {
                        mpBall.SetStatus(true);
                    }
                }
            }
        }
        public void RefreshMp(int mp, int hp)
        {
            this.mpBox.text = mp.ToString();
            this.hpBox.text = hp.ToString();
        }
        private void UpdateDispatcher(bool register)
        {
            this.dispatcher.UpdateListener(register, GameEvent.INTERACTION, this.OnPlayerStopped);
            this.dispatcher.UpdateListener(register, GameEvent.SCENIC_CARD_CLOSE, this.OnScenicCardClosed);
            this.dispatcher.UpdateListener(register, GameEvent.CITY_STATION_CLOSE, this.OnCityStationClosed);
        }
        private Vector3 GetRandomPosition()
        {
            Vector3 position = new Vector3(Random.Range(-this.halfWidth, this.halfWidth), Random.Range(-this.halfHeight, this.halfHeight), 0);
            List<Vector3> positions = new List<Vector3>();
            positions.Add(this.player.transform.localPosition);
            this.teammates.ForEach(t => positions.Add(t.transform.localPosition));
            this.mpBalls.ForEach(t => positions.Add(t.transform.localPosition));
            positions.ForEach(t => t = this.transform.TransformPoint(t));
            bool valid = true;
            foreach (var item in positions)
            {
                if (Vector3.Distance(position, item) < 300f)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
                return position;
            else
                return this.GetRandomPosition();
        }
        private void SetMpBallVisible(bool visible)
        {
            this.hideMpBalls = !visible;
            this.CancelAllDelayInvoke();

            if (visible)
                this.DelayInvoke(() => this.mpBalls.ForEach(t => t.SetStatus(true)), 0.75f);
            else
                this.mpBalls.ForEach(t => t.SetStatus(false));
        }
        private void CollectMp(MpBall mpBall)
        {
            this.dispatcher.Dispatch(GameEvent.COLLECT_MP, mpBall);
        }
        private void OnPlayerStopped(IEvent evt)
        {
            if (evt == null || evt.data == null)
            {
                Debug.LogError("<><CyclingView.OnPlayerStopped>Error: parameter 'evt' or 'evt.data' is null");
                return;
            }

            MapPointNode mapPointNode = evt.data as MapPointNode;
            if (mapPointNode == null)
            {
                Debug.LogError("<><CyclingView.OnPlayerStopped>Error: parameter 'evt.data' is not the type MapPointNode");
                return;
            }

            //检测是否有卡片需要显示
            if (mapPointNode.NodeType == NodeTypes.EndNode)
            {
                this.cityStation.Show(this.player.MapNode.ID);
            }
            else if (mapPointNode.NodeType == NodeTypes.SiteNode)
            {
                InteractionData interactionData = mapPointNode.GetComponent<InteractionData>();
                if (interactionData != null && interactionData.Interacton == Interactions.KNOWLEDGE_LANDMARK)
                {
                    this.scenicCard.Show(interactionData.ID);//显示卡片
                }
            }
            else
            {
                this.playerCanGo = true;
                this.SetMpBallVisible(true);
            }
        }
        private void OnScenicCardClosed(IEvent evt)
        {
            this.playerCanGo = true;
            this.SetMpBallVisible(true);
        }
        private void OnCityStationClosed(IEvent evt)
        {
            if (evt == null || evt.data == null)
            {
                Debug.LogError("<><CyclingView.OnCityStationClosed>Error: parameter 'evt' or 'evt.data' is null");
                return;
            }

            Ticket ticket = evt.data as Ticket;
            if (ticket == null)
            {
                Debug.LogError("<><CyclingView.OnCityStationClosed>Error: parameter 'evt.data' is not the type Ticket");
                return;
            }

            if (ticket.Go)
            {
                Debug.LogFormat("<><CyclingView.OnCityStationClosed>Data, from[{0}], to[{1}], coin[{2}], step[{3}]", ticket.FromMapID, ticket.ToMapID, ticket.Coin, ticket.Step);
                MapNode mapNode = this.player.MapNode;
                if (mapNode == null)
                {
                    Debug.LogError("<><CyclingView.OnCityStationClosed>Error: parameter 'mapNode' is null");
                    return;
                }

                MapInfo mapInfo = this.MapConfig.GetMap(mapNode.ID);
                if (mapInfo == null)
                {
                    Debug.LogError("<><CyclingView.OnCityStationClosed>Error: parameter 'mapInfo' is null");
                    return;
                }

                //Todo: 获取下一个地图的ID，并进行跳转
                string nextMapID = mapInfo.NextMap;
            }
            else
            {
                this.playerCanGo = true;
            }
        }
    }
}
