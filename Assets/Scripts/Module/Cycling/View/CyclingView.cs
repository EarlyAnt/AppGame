using AppGame.Data.Local;
using AppGame.Data.Model;
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
        /************************************************�������������************************************************/
        #region ע��ӿ�
        [Inject]
        public ILocalChildInfoAgent LocalChildInfoAgent { get; set; }
        #endregion
        #region ҳ��UI���
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
        #endregion
        #region ��������
        private float halfWidth = 280f;
        private float halfHeight = 800f;
        private List<Teammate> teammates;
        private List<MpBall> mpBalls;
        #endregion
        /************************************************Unity�������¼�***********************************************/
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
        /************************************************�� �� �� �� ��************************************************/
        private void Initialize()
        {
            this.DelayInvoke(() =>
            {
                this.mask.DOFade(0f, 1f);
                this.canvasGroup.DOFade(1f, 1f);
            }, 1f);

            this.StartCoroutine(this.LoadModuleFiles(ModuleViews.Cycling));
            //this.DelayInvoke(() => SpriteHelper.Instance.ClearBuffer(ModuleViews.Cycling), 5f);
        }
        public void MoveForward()
        {
            this.player.MoveForward();
        }
        public void MoveBack()
        {
            this.player.MoveBack();
        }

        public void RefreshPlayer(List<BasicData> basicDataList, List<PlayerData> playerDataList)
        {
            PlayerData myPlayerData = playerDataList.Find(t => t.child_sn == this.LocalChildInfoAgent.GetChildSN());
            this.player.MoveToNode(myPlayerData.map_position);
            this.player.name = "Player_" + myPlayerData.child_sn;
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
                    teammate.name = "Teammate_" + teammateData.child_sn;
                    teammate.AvatarBox.sprite = SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Cycling, string.Format("Texture/Cycling/View/{0}.png", basicDataList.Find(t => t.child_sn == teammateData.child_sn).child_avatar));
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
