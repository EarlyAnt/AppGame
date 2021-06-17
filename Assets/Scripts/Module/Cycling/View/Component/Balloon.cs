using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    /// <summary>
    /// 气球面板
    /// </summary>
    public class Balloon : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private float appearY = 1f;
        [SerializeField]
        private float raiseY = 2.5f;
        [SerializeField]
        private Text coinBox;
        private RectTransform rectTransform;
        private float originY = 0f;
        public CanvasGroup CanvasGroup { get { return this.canvasGroup; } }
        public BalloonStatus BalloonStatus { get; private set; }
        public int Coin
        {
            get
            {
                int coin = 0;
                return int.TryParse(this.coinBox.text, out coin) ? coin : 0;
            }
        }
        /************************************************Unity方法与事件***********************************************/
        private void Awake()
        {
            this.coinBox.text = "0";
            this.canvasGroup.DOFade(0f, 0f);
            this.rectTransform = this.canvasGroup.GetComponent<RectTransform>();
            this.originY = this.rectTransform.localPosition.y;
            this.BalloonStatus = BalloonStatus.Disappear;
        }
        /************************************************自 定 义 方 法************************************************/
        //设置数值
        public void SetValue(int coin)
        {
            this.coinBox.text = coin.ToString();
        }
        //出现
        public void Appear()
        {
            this.canvasGroup.DOFade(0f, 0f);
            this.rectTransform.DOLocalMoveY(this.originY, 0f);

            this.canvasGroup.DOFade(1f, 0.5f);
            this.rectTransform.DOLocalMoveY(this.appearY, 0.5f).onComplete = () => { this.BalloonStatus = BalloonStatus.Appear; };
        }
        //上升
        public void Raise(System.Action callback)
        {
            this.canvasGroup.DOFade(0f, 0.5f);
            this.rectTransform.DOLocalMoveY(this.raiseY, 0.5f).onComplete = () =>
            {
                this.BalloonStatus = BalloonStatus.Raise;
                if (callback != null) callback();
            };
        }
        //消失
        public void Disappear(float duration = 0.5f)
        {
            this.canvasGroup.DOFade(0f, duration).onComplete = () => { this.BalloonStatus = BalloonStatus.Disappear; };
        }
        //重置
        public void Reset()
        {
            this.rectTransform.DOLocalMoveY(this.originY, 0f);
            this.Disappear(0);
        }
    }
}
