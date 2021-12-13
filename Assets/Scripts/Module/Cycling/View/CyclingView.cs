using AppGame.Config;
using AppGame.Data.Local;
using AppGame.Data.Model;
using AppGame.Global;
using AppGame.UI;
using AppGame.Util;
using DG.Tweening;
using strange.extensions.dispatcher.eventdispatcher.api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [Inject]
        public IAssetBundleUtil AssetBundleUtil { get; set; }
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
        private ABSpriteLoader avatarLoader;
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
        private GoButton goButton;
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
        private TreasureBox treasureBox;
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
        private Coroutine refreshBallCoroutine;
        private Coroutine collectMpCoroutine;
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
            this.Restart();
        }
        protected override void Start()
        {
            base.Start();
            this.Initialize();
            this.UpdateDispatcher(true);
        }
        protected override void OnDestroy()
        {
            this.goButton.DestroyAssetBundle();
            this.UpdateDispatcher(false);
            base.OnDestroy();
        }
        /************************************************自 定 义 方 法************************************************/
        private void Initialize()
        {
            this.StartCoroutine(this.LoadModuleFiles(ModuleViews.Cycling));
        }
        public void RestartGame()
        {
            this.dispatcher.Dispatch(GameEvent.GAME_START);
        }
        public void Restart()
        {
            //复原状态变量
            this.playerCanGo = true;
            this.hideMpBalls = false;
            this.mask.DOFade(1f, 0f);
            this.canvasGroup.DOFade(0f, 0f);

            //场景过渡
            this.DelayInvoke(() =>
            {
                this.mask.DOFade(0f, 0.5f);
                this.canvasGroup.DOFade(1f, 0.5f);
            }, 0.5f);

            //清除队友
            if (this.teammates != null)
                this.teammates.Clear();

            while (this.teammateRoot.childCount > 0)
                GameObject.DestroyImmediate(this.teammateRoot.GetChild(0).gameObject);

            //清除能量气泡
            if (this.mpBalls != null)
                this.mpBalls.Clear();

            while (this.mpBallRoot.childCount > 0)
                GameObject.DestroyImmediate(this.mpBallRoot.GetChild(0).gameObject);
        }
        public void GoBack()
        {
            SceneManager.LoadScene("GameOver");
            this.DelayInvoke(() =>
            {
                this.AssetBundleUtil.UnloadAllAssets();
#if UNITY_ANDROID
                AndroidNativeAPI.Instance.GoBack();
#elif UNITY_IOS
            iOSNativeAPI.Instance.GoBack();
#endif
                Debug.Log("<><CyclingView.GoBack>go back to flutter");
            }, 1f);
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

            while (this.mapRoot.childCount > 0)
                GameObject.DestroyImmediate(this.mapRoot.GetChild(0).gameObject);
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
                this.mpBalls.ForEach(t => t.AutoCollectMp());
                this.StopOneCoroutine(this.refreshBallCoroutine);
                this.StopOneCoroutine(this.collectMpCoroutine);
                this.collectMpCoroutine = this.StartCoroutine(this.CollectMpAnimation(() =>
                {
                    this.dispatcher.Dispatch(GameEvent.GO_CLICK);
                }));
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
            string avatarFileName = this.CommonImageUtils.GetAvatarFileName(myPlayerData.child_avatar);
            this.player.SetAvatar(avatarFileName);
            this.avatarLoader.SetImageName(avatarFileName);
            this.nameBox.text = myPlayerData.child_name;
            Debug.LogFormat("<><CyclingView.RefreshPlayer>player name: {0}", this.nameBox.text);
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

            if (this.teammates == null || this.teammates.Count == 0)
            {
                this.teammates = new List<Teammate>();
                foreach (var teammateData in playerDataList)
                {
                    if (teammateData.child_sn == this.ChildInfoManager.GetChildSN() ||
                        teammateData.map_id != myPlayerData.map_id)
                        continue;

                    Teammate teammate = GameObject.Instantiate<Teammate>(this.teammatePrefab, this.teammateRoot);
                    teammate.name = "Teammate_" + teammateData.child_sn;
                    teammate.SetAvatar(this.CommonImageUtils.GetAvatarFileName(teammateData.child_avatar));
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
                    newMpBall.transform.localScale = Vector3.one * Random.Range(0.4f, 0.7f);
                    newMpBall.transform.localPosition = this.GetRandomPosition();
                    newMpBall.OnCollectMp = this.CollectMp;
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
                }
            }

            this.StopOneCoroutine(this.refreshBallCoroutine);
            this.refreshBallCoroutine = this.StartCoroutine(this.RefreshMpBallAnimation());
        }
        public void RefreshMpAndHp(int mp, int hp)
        {
            this.mpBox.text = mp.ToString();
            this.hpBox.text = hp.ToString();
            this.goButton.SetWaterLevel(mp);
        }
        public void ShowPayBill(MpData mpData)
        {
            this.payBill.Show(mpData);
        }
        public void CityStation(int coin, int hp)
        {
            this.cityStation.Show(this.player.MapNode.ID, coin, hp);
        }
        public void ScenicCard(MapPointNode mapPointNode)
        {
            InteractionData interactionData = mapPointNode.GetComponent<InteractionData>();
            if (interactionData != null && interactionData.Interacton == Interactions.KNOWLEDGE_LANDMARK)
                this.scenicCard.Show(interactionData.ID);//显示卡片
            else
                this.KeepGoing();
        }
        public void TreasureBox()
        {
            this.treasureBox.Play(() =>
            {
                this.KeepGoing();
                this.Coin = this.ItemDataManager.GetItemCount(Items.COIN);
            });//开宝箱动画
        }
        public void KeepGoing()
        {
            this.playerCanGo = true;
            this.SetMpBallVisible(true);
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
            this.DelayInvoke(this.trafficLoading.Hide, 0.5f);
        }
        public void Stay()
        {
            this.playerCanGo = true;
            this.SetMpBallVisible(true);
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
        private void StopOneCoroutine(Coroutine coroutine)
        {
            if (coroutine != null)
                this.StopCoroutine(coroutine);
        }
        private IEnumerator RefreshMpBallAnimation()
        {
            int index = 0;
            float speed = Random.Range(0.375f, 0.75f);
            while (index < this.mpBalls.Count)
            {
                this.mpBalls[index].SetStatus(!this.hideMpBalls);
                yield return new WaitForSeconds(0.25f);
                index++;
            };
        }
        private IEnumerator CollectMpAnimation(System.Action callback)
        {
            if (this.mpBalls != null && this.mpBalls.Count > 0 && this.mpBalls.Exists(t => t.Visible))
            {
                int index = 0;
                float speed = Random.Range(0.375f, 0.75f);
                while (index < this.mpBalls.Count)
                {
                    MpBall mpBall = this.mpBalls[index];
                    mpBall.CanvasGroup.DOFade(0f, speed);
                    mpBall.transform.DOScale(0f, speed);
                    mpBall.transform.DOMove(this.goButton.transform.position, speed);
                    yield return new WaitUntil(() => mpBall.CanvasGroup == null || mpBall.CanvasGroup.alpha <= 0.5f);
                    index++;
                };

                while (this.mpBallRoot.childCount > 0)
                    GameObject.DestroyImmediate(this.mpBallRoot.GetChild(0).gameObject);
                this.mpBalls.Clear();

                if (callback != null) callback();
            }
            else
            {
                if (callback != null) callback();
            }
        }
    }
}
