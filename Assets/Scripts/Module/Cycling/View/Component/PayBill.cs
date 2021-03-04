using AppGame.Config;
using AppGame.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class PayBill : BaseView
    {
        /************************************************�������������************************************************/
        #region ע��ӿ�
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
        #region ҳ��UI���
        [SerializeField]
        private Transform root;//��Ƭ��������
        [SerializeField]
        private CanvasGroup tipPanel;//������ʾ���
        [SerializeField]
        private Text titleBox;//�������ֿ�
        [SerializeField]
        private Text tipBox;//˵�����ֿ�
        [SerializeField]
        private Text mpBox;//�������ֿ�
        [SerializeField]
        private Text feepBox;//�������ֿ�
        [SerializeField]
        private Image buttonBox;//���ñ߿�
        #endregion
        #region ��������
        private MpData mpData = null;
        private Tweener fadeTweener = null;
        private Tweener moveTweener = null;
        private float tipPanelShowY = 0f;
        private float tipPanelHideY = 0f;
        #endregion
        /************************************************Unity�������¼�***********************************************/
        protected override void Awake()
        {
            this.tipPanelShowY = this.tipPanel.transform.localPosition.y;
            this.tipPanelHideY = 2436f / 2f + this.tipPanel.GetComponent<RectTransform>().sizeDelta.y;
            this.tipPanel.transform.DOLocalMoveY(this.tipPanelHideY, 0f);
        }
        /************************************************�� �� �� �� ��************************************************/
        //��ʾ��Ƭ
        public void Show(MpData mpData)
        {
            this.mpData = mpData;
            //����ҳ������
            this.mpBox.text = mpData.Mp.ToString();
            this.feepBox.text = mpData.Coin.ToString();
            //this.titleBox.text = "";//Todo: �������Ϲ��ʻ��Ĺ���
            //this.tipBox.text = "";//Todo: �������Ϲ��ʻ��Ĺ���
            Sprite sprite = SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Cycling, this.ModuleConfig.GetImagePath(ModuleViews.Cycling, mpData.CoinEnough ? "enable_button" : "disable_button"));
            this.buttonBox.sprite = sprite;
            this.buttonBox.raycastTarget = mpData.CoinEnough;
            this.gameObject.SetActive(true);
            this.SetErrorPanel(!mpData.CoinEnough);
            if (!mpData.CoinEnough) this.DelayInvoke(() => this.SetErrorPanel(false), 2f);
        }
        //���ؿ�Ƭ
        public void Hide(bool pay)
        {
            this.SetErrorPanel(false);
            this.gameObject.SetActive(false);
            if (pay) this.dispatcher.Dispatch(GameEvent.PAY_BILL_CLOSE, this.mpData);
        }
        //���ô�����ʾ���
        public void SetErrorPanel(bool visible)
        {
            if (this.fadeTweener != null) this.fadeTweener.Kill();
            if (this.moveTweener != null) this.moveTweener.Kill();
            this.fadeTweener = this.tipPanel.DOFade(visible ? 1f : 0f, visible ? 0.75f : 0.375f);
            this.moveTweener = this.tipPanel.transform.DOLocalMoveY(visible ? this.tipPanelShowY : this.tipPanelHideY, visible ? 0.75f : 0.375f);
        }
    }
}
