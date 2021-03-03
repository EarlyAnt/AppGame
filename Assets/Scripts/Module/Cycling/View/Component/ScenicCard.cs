using DG.Tweening;
using AppGame.Config;
using AppGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class ScenicCard : BaseView
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
        #endregion
        #region ҳ��UI���
        [SerializeField]
        private Transform root;//��Ƭ��������
        [SerializeField]
        private GameObject fore;//��Ƭ����
        [SerializeField]
        private GameObject back;//��Ƭ����
        [SerializeField, Range(0f, 1f)]
        private float rotateDuration = 1f;//��Ƭ��תʱ��
        [SerializeField]
        private Image imageBox;//����ͼƬ��
        [SerializeField]
        private Text cityNameBox;//�����������ֿ�
        [SerializeField]
        private Text scenicNameBox;//�����������ֿ�
        [SerializeField]
        private Text descriptionBox;//����������ֿ�
        #endregion
        #region ��������
        private Vector3 middleAngle = new Vector3(0f, 90f, 0f);//��Ƭ90�Ƚ�
        private Vector3 foreAngle = new Vector3(0f, 180f, 0f);//��Ƭ��ת�Ƕ�
        #endregion
        /************************************************Unity�������¼�***********************************************/

        /************************************************�� �� �� �� ��************************************************/
        //��ʾ��Ƭ
        public void Show(string scenicID)
        {
            this.Reset();
            //��ѯ��ͼ�;�����������
            ScenicInfo scenicInfo = this.ScenicConfig.GetScenic(scenicID);
            if (scenicInfo == null)
            {

                Debug.LogErrorFormat("<><ScenicCard.Show>Error: can not find scenic[{0}]", scenicID);
                return;
            }

            MapInfo mapInfo = this.MapConfig.GetMap(scenicInfo.MapID);
            if (mapInfo == null)
            {

                Debug.LogErrorFormat("<><ScenicCard.Show>Error: can not find map[{0}]", scenicInfo.MapID);
                return;
            }

            CardInfo card = this.CardConfig.GetCard(scenicInfo.CardID);
            if (card == null)
            {

                Debug.LogErrorFormat("<><ScenicCard.Show>Error: can not find card[{0}]", scenicInfo.CardID);
                return;
            }

            //����ҳ������
            this.imageBox.sprite = SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Cycling, string.Format("Texture/Cycling/Site/{0}", card.Image));
            this.cityNameBox.text = mapInfo.CityName;
            this.scenicNameBox.text = scenicInfo.Name;
            this.descriptionBox.text = this.I18NConfig.GetText(card.Text);
            this.gameObject.SetActive(true);
        }
        //���ؿ�Ƭ
        public void Hide()
        {
            this.gameObject.SetActive(false);
            this.Reset();
            this.OnViewClosed();
        }
        //��ת��Ƭ
        public void Rotate()
        {
            this.root.DOScale(1, this.rotateDuration);
            this.root.DOLocalRotate(this.middleAngle, this.rotateDuration / 2).onComplete = () =>
            {
                this.back.SetActive(false);
                this.fore.SetActive(true);
                this.root.DOLocalRotate(this.foreAngle, this.rotateDuration / 2);
            };
        }
        //���迨Ƭ״̬
        private void Reset()
        {
            this.root.localEulerAngles = Vector3.zero;
            this.root.localScale = Vector3.one * 0.7f;
            this.fore.SetActive(false);
            this.back.SetActive(true);
        }
        //����Ƭ�ر�ʱ
        private void OnViewClosed()
        {
            this.dispatcher.Dispatch(GameEvent.SCENIC_CARD_CLOSE);
        }
    }
}
