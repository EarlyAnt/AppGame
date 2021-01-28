using AppGame.UI;
using DG.Tweening;
using System.Collections;
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
        private Image mask;
        [SerializeField]
        private CanvasGroup canvasGroup;
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
                this.mask.DOFade(0f, 1f);
                this.canvasGroup.DOFade(1f, 1f);
            }, 1f);
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
