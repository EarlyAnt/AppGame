using AppGame.Global;
using AppGame.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class CyclingView : BaseView
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private Player player;
        [SerializeField]
        private Teammate teammatePrefab;
        [SerializeField]
        private List<Teammate> teammates;
        [SerializeField]
        private Image mask;
        [SerializeField]
        private CanvasGroup canvasGroup;
        public LocationDatas LocationDatas { get; set; }
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

            foreach (var teammateData in this.LocationDatas.Datas)
            {
                if (teammateData.UserID == AppData.UserID)
                    continue;

                Teammate teammate = GameObject.Instantiate<Teammate>(this.teammatePrefab, this.player.transform.parent);
                teammate.name = "Teammate_" + teammateData.UserID;
                teammate.MoveToNode(teammateData.MapPointID);
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
    }
}
