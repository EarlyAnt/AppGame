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
        private Vector3 middleAngle = new Vector3(0f, 90f, 0f);//��Ƭ90�Ƚ�
        private Vector3 foreAngle = new Vector3(0f, 180f, 0f);//��Ƭ��ת�Ƕ�
        public System.Action ViewClosed { get; set; }//��Ƭ�ر�ʱί��
        /************************************************Unity�������¼�***********************************************/

        /************************************************�� �� �� �� ��************************************************/
        //��ʾ��Ƭ
        public void Show(Sprite sprite, string cityName, string scenicName, string description)
        {
            this.Reset();
            this.imageBox.sprite = sprite;
            this.cityNameBox.text = cityName;
            this.scenicNameBox.text = scenicName;
            this.descriptionBox.text = description;
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
            if (this.ViewClosed != null)
                this.ViewClosed();
        }
    }
}
