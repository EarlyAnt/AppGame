using AppGame.Config;
using AppGame.Data.Local;
using AppGame.Data.Model;
using AppGame.Global;
using AppGame.UI;
using AppGame.Util;
using DG.Tweening;
using strange.extensions.dispatcher.eventdispatcher.api;
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
        public IItemDataManager ItemDataManager { get; set; }
        [Inject]
        public ICommonImageUtils CommonImageUtils { get; set; }
        [Inject]
        public IPrefabUtil PrefabUtil { get; set; }
        #endregion
        #region 页面UI组件
        [SerializeField]
        private Image mask;
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private TextureLoader mapBox;
        [SerializeField]
        private TextureLoader pathBox;
        [SerializeField]
        private Transform mapRoot;
        [SerializeField]
        private MapNode mapPrefab;
        [SerializeField]
        private Player player;
        [SerializeField]
        private Transform teammateRoot;
        [SerializeField]
        private Teammate teammatePrefab;
        [SerializeField]
        private Text progressBox;
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
        private PayBill payBill;
        [SerializeField]
        private ScenicCard scenicCard;
        [SerializeField]
        private CityStation cityStation;
        [SerializeField]
        private TrafficLoading trafficLoading;
        [SerializeField]
        private Image touchPad;
        #endregion
        #region 其他变量
        private bool playerCanGo = true;
        private bool hideMpBalls = false;
        private float halfWidth = 380f;
        private float halfHeight = 800f;
        private MapNode mapNode;
        private List<Teammate> teammates;
        private List<MpBall> mpBalls;
        public int Coin
        {
            get { return int.Parse(this.coinBox.text); }
            set { this.coinBox.text = value.ToString(); }
        }
        public Player Player { get { return this.player; } }
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
            this.Restart();
            this.StartCoroutine(this.LoadModuleFiles(ModuleViews.Cycling));
        }
        public void Restart()
        {
            this.playerCanGo = true;
            this.hideMpBalls = false;
            this.mask.DOFade(1f, 0f);
            this.canvasGroup.DOFade(0f, 0f);

            this.DelayInvoke(() =>
            {
                this.mask.DOFade(0f, 1f);
                this.canvasGroup.DOFade(1f, 1f);
            }, 0.5f);
        }
        public void LoadMap(string mapID)
        {
            MapInfo mapInfo = this.MapConfig.GetMap(mapID);
            if (mapInfo == null)
            {
                Debug.LogErrorFormat("<><CyclingView.LoadMap>Can not find the map[{0}]", mapID);
                return;
            }
            this.mapBox.LoadImage(mapInfo.AB, mapInfo.MapImage);
            this.pathBox.LoadImage(mapInfo.AB, mapInfo.PathImage);

            this.mapNode = this.PrefabUtil.CreateGameObject("Cycling/Road", mapID).GetComponent<MapNode>();
            this.mapNode.transform.SetParent(this.mapRoot);
            this.mapNode.transform.localPosition = Vector3.zero;
            this.mapNode.transform.localRotation = Quaternion.identity;
            this.mapNode.transform.localScale = Vector3.one;
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
        public void RefreshMapProgress(string mapName, int cardCount, int scenicCount)
        {
            this.progressBox.text = string.Format("{0} {1}/{2}", mapName, cardCount, scenicCount);
        }
        public void RefreshPlayer(PlayerData myPlayerData, int coin)
        {
            if (myPlayerData == null)
            {
                Debug.LogError("<><CyclingView.RefreshPlayer>Error: parameter 'myPlayerData' is null");
                return;
            }

            this.player.MapNode = this.mapNode;
            this.player.MoveToNode(myPlayerData.map_position);
            this.player.name = "Player_" + myPlayerData.child_sn;
            Sprite avatar = this.CommonImageUtils.GetAvatar(myPlayerData.child_avatar);
            this.player.Avatar = avatar;
            this.avatarBox.sprite = avatar;
            this.nameBox.text = myPlayerData.child_name;
            this.coinBox.text = coin.ToString();

        }
        public void RefreshTeammates(List<PlayerData> playerDataList)
        {
            PlayerData myPlayerData = playerDataList.Find(t => t.child_sn == this.ChildInfoManager.GetChildSN());
            if (myPlayerData == null)
            {
                Debug.LogError("<><CyclingView.RefreshTeammates>Error: parameter 'myPlayerData' is null");
                return;
            }

            if (this.teammates == null)
            {
                this.teammates = new List<Teammate>();
                foreach (var teammateData in playerDataList)
                {
                    if (teammateData.child_sn == this.ChildInfoManager.GetChildSN() ||
                        teammateData.map_id != myPlayerData.map_id)
                        continue;

                    Teammate teammate = GameObject.Instantiate<Teammate>(this.teammatePrefab, this.teammateRoot);
                    teammate.name = "Teammate_" + teammateData.child_sn;
                    teammate.Avatar = this.CommonImageUtils.GetAvatar(teammateData.child_avatar);
                    teammate.MapNode = this.mapNode;
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
                if (mpBall == null && mpData.Mp > 0)
                {
                    MpBall prefab = mpData.MpBallType == MpBallTypes.Family || mpData.MpBallType == MpBallTypes.Friend ? this.mpBallPrefab2 : this.mpBallPrefab1;
                    MpBall newMpBall = GameObject.Instantiate(prefab, this.mpBallRoot);
                    newMpBall.MpBallType = mpData.MpBallType;
                    if (newMpBall.MpBallType == MpBallTypes.Family || newMpBall.MpBallType == MpBallTypes.Friend)
                    {
                        newMpBall.FromID = mpData.FromID;
                        newMpBall.FromName = mpData.FromName;
                    }
                    newMpBall.Value = mpData.Mp;
                    newMpBall.transform.localRotation = Quaternion.identity;
                    newMpBall.transform.localScale = Vector3.one * Random.Range(0.7f, 1.0f);
                    newMpBall.transform.localPosition = this.GetRandomPosition();
                    newMpBall.OnCollectMp = this.CollectMp;
                    newMpBall.SetStatus(!this.hideMpBalls);
                    this.mpBalls.Add(newMpBall);
                }
                else if (mpBall != null)
                {
                    mpBall.Value = mpData.Mp;
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
        public void ShowPayBill(MpData mpData)
        {
            this.payBill.Show(mpData);
        }
        private void UpdateDispatcher(bool register)
        {
            this.dispatcher.UpdateListener(register, GameEvent.SCENIC_CARD_CLOSE, this.OnScenicCardClosed);
            this.dispatcher.UpdateListener(register, GameEvent.PAY_BILL_CLOSE, this.OnPayBillClosed);
            this.dispatcher.UpdateListener(register, GameEvent.SET_TOUCH_PAD_ENABLE, this.OnSetTouch);
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
            this.dispatcher.Dispatch(GameEvent.MP_CLICK, mpBall);
        }
        public void Interact(MapPointNode mapPointNode, PlayerData myPlayerData)
        {
            if (mapPointNode == null)
            {
                Debug.LogError("<><CyclingView.OnPlayerStopped>Error: parameter 'evt.data' is not the type MapPointNode");
                return;
            }
            else if (myPlayerData == null)
            {
                Debug.LogError("<><CyclingView.Interact>Error: parameter 'myPlayerData' is null");
                return;
            }

            //检测是否有卡片需要显示
            if (mapPointNode.NodeType == NodeTypes.EndNode)
            {
                this.cityStation.Show(this.player.MapNode.ID, this.ItemDataManager.GetItemCount(Items.COIN), myPlayerData.hp);
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
        public void ShowLoading(Ticket ticket)
        {
            if (ticket != null && ticket.Go)
            {
                this.trafficLoading.Show(ticket);
            }
            else
            {
                this.playerCanGo = true;
            }
        }
        public void HideLoading()
        {
            this.Restart();
            this.DelayInvoke(this.trafficLoading.Hide, 0.5f);
        }
        public void Stay()
        {
            this.playerCanGo = true;
            this.SetMpBallVisible(true);
        }
        private void OnScenicCardClosed(IEvent evt)
        {
            this.playerCanGo = true;
            this.SetMpBallVisible(true);
        }
        private void OnPayBillClosed(IEvent evt)
        {
            if (evt == null || evt.data == null)
            {
                Debug.LogError("<><CyclingView.OnPayBillClosed>Error: parameter 'evt' or 'evt.data' is null");
                return;
            }

            MpData mpData = evt.data as MpData;
            if (mpData == null)
            {
                Debug.LogError("<><CyclingView.OnPayBillClosed>Error: parameter 'evt.data' is not the type CollectMp");
                return;
            }

            this.dispatcher.Dispatch(GameEvent.COLLECT_MP, mpData);
        }
        private void OnSetTouch(IEvent evt)
        {
            if (evt == null || evt.data == null)
            {
                Debug.LogError("<><CyclingView.OnSetTouch>Error: parameter 'evt' or 'evt.data' is null");
                return;
            }
            this.touchPad.enabled = (bool)evt.data;
        }
    }
}
