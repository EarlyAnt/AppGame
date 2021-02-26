using AppGame.Config;
using AppGame.Data.Local;
using AppGame.Data.Model;
using AppGame.Global;
using AppGame.UI;
using DG.Tweening;
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
        public ILocalChildInfoAgent LocalChildInfoAgent { get; set; }
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
        #endregion
        #region 其他变量
        private float halfWidth = 380f;
        private float halfHeight = 800f;
        private List<Teammate> teammates;
        private List<MpBall> mpBalls;
        public Signal<MpBall> CollectMpSignal = new Signal<MpBall>();
        public Signal GoSignal = new Signal();
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
        }
        protected override void OnDestroy()
        {
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
            if (!this.player.IsMoving)
                this.GoSignal.Dispatch();
        }
        public void Move(bool canMove, int hp)
        {
            if (canMove)
            {
                this.hpBox.text = hp.ToString();
                this.player.MoveForward();
            }
            else
            {
                //Todo: 显示行动点数不足的提示
            }
        }
        public void RefreshPlayer(List<BasicData> basicDataList, List<PlayerData> playerDataList)
        {
            PlayerData myPlayerData = playerDataList.Find(t => t.child_sn == this.LocalChildInfoAgent.GetChildSN());
            BasicData myBasicData = basicDataList.Find(t => t.child_sn == this.LocalChildInfoAgent.GetChildSN());
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
                    if (teammateData.child_sn == this.LocalChildInfoAgent.GetChildSN())
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
                    if (teammateData.child_sn == this.LocalChildInfoAgent.GetChildSN())
                        continue;

                    Teammate teammate = this.teammates.Find(t => t.name == "Teammate_" + teammateData.child_sn);
                    if (teammate != null) teammate.MoveToNode(teammateData.map_position);
                }
            }
        }
        public void RefreshMpBalls(List<MpData> mpDatas)
        {
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
                    newMpBall.transform.localScale = Vector3.one;
                    newMpBall.transform.localPosition = this.GetRandomPosition();
                    newMpBall.OnCollectMp = this.CollectMp;
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
                }
            }
        }
        public void RefreshMp(int mp, int hp)
        {
            this.mpBox.text = mp.ToString();
            this.hpBox.text = hp.ToString();
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
        private void CollectMp(MpBall mpBall)
        {
            if (mpBall.Value >= 100)
            {
                this.CollectMpSignal.Dispatch(mpBall);
            }
            else
            {
                //Todo: 能量不足100时，弹出提示
            }
        }
    }
}
