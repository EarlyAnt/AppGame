using DG.Tweening;
using AppGame.Config;
using AppGame.UI;
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
        #endregion
        /************************************************Unity�������¼�***********************************************/

        /************************************************�� �� �� �� ��************************************************/
        //��ʾ��Ƭ
        public void Show(int mp, int coin, bool coinEnough)
        {
            //����ҳ������
            this.mpBox.text = mp.ToString();
            this.feepBox.text = coin.ToString();
            //this.titleBox.text = "";//Todo: �������Ϲ��ʻ��Ĺ���
            //this.tipBox.text = "";//Todo: �������Ϲ��ʻ��Ĺ���
            Sprite sprite = SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Cycling, this.ModuleConfig.GetImagePath(ModuleViews.Cycling, coinEnough ? "enable_button" : "disable_button"));
            this.buttonBox.sprite = sprite;
            this.buttonBox.raycastTarget = coinEnough;
            this.gameObject.SetActive(true);
        }
        //���ؿ�Ƭ
        public void Hide()
        {
            this.gameObject.SetActive(false);
            this.OnViewClosed();
        }
        //����Ƭ�ر�ʱ
        private void OnViewClosed()
        {
            this.dispatcher.Dispatch(GameEvent.PAY_BILL_CLOSE);
        }
    }
}
