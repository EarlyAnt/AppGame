using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class MpBall : MonoBehaviour
    {
        /************************************************�������������************************************************/
        #region ҳ��UI���
        [SerializeField]
        private MpBallTypes mpBallType;
        [SerializeField]
        private Text mpBox;
        [SerializeField]
        private Text fromBox;
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private Transform bubbleTransform;
        [SerializeField, Range(0f, 100f)]
        private float destinationY = 2f;
        [SerializeField, Range(0.1f, 5f)]
        private float durationMin = 2f;
        [SerializeField, Range(0.1f, 5f)]
        private float durationMax = 2f;
        #endregion
        #region ��������
        private Tweener tweener;
        private float originY;
        private int mp = 0;
        private string fromName = "";
        public int Value
        {
            get { return this.mp; }
            set { this.mp = value; this.mpBox.text = value.ToString(); }
        }
        public MpBallTypes MpBallType
        {
            get
            {
                return this.mpBallType;
            }
            set
            {
                this.mpBallType = value;
                switch (value)
                {
                    case MpBallTypes.Walk:
                        this.fromBox.text = "����";
                        break;
                    case MpBallTypes.Ride:
                        this.fromBox.text = "����";
                        break;
                    case MpBallTypes.Train:
                        this.fromBox.text = "����ѵ��";
                        break;
                    case MpBallTypes.Learn:
                        this.fromBox.text = "���˼��";
                        break;
                }
            }
        }
        public string FromID { get; set; }
        public string FromName
        {
            get { return this.fromName; }
            set { this.fromName = value; this.fromBox.text = value; }
        }
        #endregion
        /************************************************Unity�������¼�***********************************************/
        private void Awake()
        {
            this.mpBox.text = "0";
        }
        private void Start()
        {
            this.originY = this.bubbleTransform.localPosition.y;
            this.destinationY += this.originY;
            this.PlayPingPong();
        }
        /************************************************�� �� �� �� ��************************************************/
        //�����Ƿ�ɼ�
        public void SetStatus(bool visible)
        {
            this.canvasGroup.DOFade(visible ? 1f : 0f, visible ? 0.2f : 0f);
        }
        //��ʼ����
        public void PlayPingPong()
        {
            this.StopPingPong();
            if (Random.Range(0, 10) % 2 == 0)
                this.PingPong(this.originY, this.destinationY);
            else
                this.PingPong(this.originY, this.destinationY);
        }
        //ֹͣ����
        public void StopPingPong()
        {
            if (this.tweener != null)
                this.tweener.Kill();
        }
        //�����˶�
        private void PingPong(float from, float to)
        {
            this.tweener = this.bubbleTransform.DOLocalMoveY(to, Random.Range(this.durationMin, this.durationMax));
            this.tweener.onComplete += () => this.PingPong(to, from);
        }
    }
}
