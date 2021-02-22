using AppGame.Global;
using AppGame.UI;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class CyclingView : BaseView
    {
        /************************************************属性与变量命名************************************************/
        #region 注入接口

        #endregion
        #region 页面UI组件
        [SerializeField]
        private Image mask;
        [SerializeField]
        private CanvasGroup canvasGroup;
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
        #endregion
        #region 其他变量
        private float halfWidth = 280f;
        private float halfHeight = 800f;
        private Player player;
        private List<Teammate> teammates;
        private List<MpBall> mpBalls;
        public LocationDatas LocationDatas { get; set; }
        #endregion
        /************************************************Unity方法与事件***********************************************/
        protected override void Awake()
        {
            base.Awake();
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
                this.InitPlayerAndTeammates();
            }, 0.5f);

            this.DelayInvoke(() =>
            {
                this.mask.DOFade(0f, 1f);
                this.canvasGroup.DOFade(1f, 1f);
            }, 1f);

            this.StartCoroutine(this.LoadModuleFiles(ModuleViews.Cycling));
            //this.DelayInvoke(() => SpriteHelper.Instance.ClearBuffer(ModuleViews.Cycling), 5f);
        }
        private void InitPlayerAndTeammates()
        {
            LocationData myData = this.LocationDatas.Datas.Find(t => t.UserID == AppData.UserID);
            this.player.MoveToNode(myData.MapPointID);
            this.player.name = "Player_" + myData.UserID;

            this.teammates = new List<Teammate>();
            foreach (var teammateData in this.LocationDatas.Datas)
            {
                if (teammateData.UserID == AppData.UserID)
                    continue;

                Teammate teammate = GameObject.Instantiate<Teammate>(this.teammatePrefab, this.teammateRoot);
                teammate.name = "Teammate_" + teammateData.UserID;
                teammate.AvatarBox.sprite = SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Cycling, string.Format("Texture/Cycling/View/{0}.png", teammateData.AvatarID));
                teammate.MoveToNode(teammateData.MapPointID);
                this.teammates.Add(teammate);
            }
        }
        public void MoveForward()
        {
            this.player.MoveForward();
        }
        public void MoveBack()
        {
            this.player.MoveBack();
        }

        public void RefreshMpBalls(List<MpData> mpDatas)
        {
            if (this.mpBalls == null)
                this.mpBalls = new List<MpBall>();

            while (this.mpBallRoot.childCount > 0)
                GameObject.DestroyImmediate(this.mpBallRoot.GetChild(0).gameObject);

            foreach (var mpData in mpDatas)
            {
                MpBall mpBall = this.mpBalls.Find(t => t.FromID == mpData.FromID);
                if (mpBall == null)
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
                    newMpBall.transform.localPosition = new Vector3(Random.Range(-this.halfWidth, this.halfWidth), Random.Range(-this.halfHeight, this.halfHeight), 0);
                    this.mpBalls.Add(newMpBall);
                }
                else
                {
                    mpBall.Value = mpData.Value;
                }
            }
        }
    }
}
