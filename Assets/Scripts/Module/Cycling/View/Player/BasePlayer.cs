using AppGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public abstract class BasePlayer : BaseView
    {
        /************************************************�������������************************************************/
        #region ע��ӿ�

        #endregion
        #region ҳ��UI���
        [SerializeField]
        protected Image avatarBox;
        [SerializeField]
        protected MapNode mapNode;
        [SerializeField]
        protected Transform player;
        [SerializeField, Range(0f, 5f)]
        protected float step;
        #endregion
        #region ��������
        protected int nodeIndex;
        protected Vector3 destination
        {
            get
            {
                return this.mapNode != null && this.mapNode.Points != null && this.mapNode.Points.Count > 0 ?
                       this.mapNode.Points[this.nodeIndex].position : Vector3.zero;
            }
        }
        public Sprite Avatar
        {
            get { return this.avatarBox.sprite; }
            set { this.avatarBox.sprite = value; }
        }
        #endregion
        /************************************************Unity�������¼�***********************************************/
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
        /************************************************�� �� �� �� ��************************************************/
        //��ʼ��
        protected virtual void Initialize()
        {
            this.player.position = this.mapNode.Points[this.nodeIndex].position;
        }
        //�ƶ���ָ��λ��
        public abstract void MoveToNode(string nodeID, bool lerp = false);
    }
}
