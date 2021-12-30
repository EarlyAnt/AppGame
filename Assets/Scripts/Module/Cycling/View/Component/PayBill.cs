using AppGame.Config;
using AppGame.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class PayBill : BaseView
    {
        /************************************************属性与变量命名************************************************/
        #region 注入接口
        [Inject]
        public IMapConfig MapConfig { get; set; }
        [Inject]
        public IScenicConfig ScenicConfig { get; set; }
        [Inject]
        public ICardConfig CardConfig { get; set; }
        [Inject]
        public II18NConfig I18NConfig { get; set; }
        [Inject]
        public IModuleConfig ModuleConfig { get; set; }
        #endregion
        #region 页面UI组件
        [SerializeField]
        private Transform root;//卡片面板根物体
        [SerializeField]
        private CanvasGroup tipPanel;//错误提示面板
        [SerializeField]
        private Text titleBox;//标题文字框
        [SerializeField]
        private Text tipBox;//说明文字框
        [SerializeField]
        private Text mpBox;//能量文字框
        [SerializeField]
        private Text feepBox;//费用文字框
        [SerializeField]
        private Image buttonBox;//费用边框
        [SerializeField]
        private Color noMoneyColor;//金币不足时按钮上数字的颜色
        #endregion
        #region 其他变量
        private MpData mpData = null;
        private Tweener fadeTweener = null;
        private Tweener moveTweener = null;
        private float tipPanelShowY = 0f;
        private float tipPanelHideY = 0f;
        #endregion
        /************************************************Unity方法与事件***********************************************/
        protected override void Awake()
        {
            this.tipPanelShowY = this.tipPanel.transform.localPosition.y;
            this.tipPanelHideY = 2436f / 2f + this.tipPanel.GetComponent<RectTransform>().sizeDelta.y;
            this.tipPanel.transform.DOLocalMoveY(this.tipPanelHideY, 0f);
        }
        /************************************************自 定 义 方 法************************************************/
        //显示卡片
        public void Show(MpData mpData)
        {
            this.dispatcher.Dispatch(GameEvent.SET_TOUCH_PAD_ENABLE, false);
            this.mpData = mpData;
            //设置页面内容
            this.mpBox.text = mpData.Mp.ToString();
            this.feepBox.text = mpData.Coin.ToString();
            this.feepBox.color = mpData.CoinEnough ? Color.white : this.noMoneyColor;
            //this.titleBox.text = "";//Todo: 后续补上国际化的功能
            //this.tipBox.text = "";//Todo: 后续补上国际化的功能

            ABSpriteLoader loader = this.buttonBox.GetComponent<ABSpriteLoader>();
            if (loader != null)
                loader.LoadImage(mpData.CoinEnough ? "enable_button" : "disable_button");
            this.buttonBox.raycastTarget = mpData.CoinEnough;
            this.root.gameObject.SetActive(true);
            this.SetErrorPanel(!mpData.CoinEnough);
            if (!mpData.CoinEnough) this.DelayInvoke(() => this.SetErrorPanel(false), 2f);
        }
        //隐藏卡片
        public void Hide(bool pay)
        {
            this.SetErrorPanel(false);
            this.root.gameObject.SetActive(false);
            if (pay) this.dispatcher.Dispatch(GameEvent.PAY_BILL_CLOSE, this.mpData);
            this.dispatcher.Dispatch(GameEvent.SET_TOUCH_PAD_ENABLE, true);
        }
        //设置错误提示面板
        public void SetErrorPanel(bool visible)
        {
            if (this.fadeTweener != null) this.fadeTweener.Kill();
            if (this.moveTweener != null) this.moveTweener.Kill();
            this.fadeTweener = this.tipPanel.DOFade(visible ? 1f : 0f, visible ? 0.75f : 0.375f);
            this.moveTweener = this.tipPanel.transform.DOLocalMoveY(visible ? this.tipPanelShowY : this.tipPanelHideY, visible ? 0.75f : 0.375f);
        }
    }
}
