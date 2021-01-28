using AppGame.UI;
using DG.Tweening;
using System.Collections;
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
        protected MapNode mapNode;
        [SerializeField]
        protected Transform player;

        #endregion
        #region ��������
        private int nodeIndex;
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
        private void Initialize()
        {
            this.player.position = this.mapNode.Points[this.nodeIndex].position;
        }
        //�ƶ���ָ��λ��
        public abstract void MoveToNode(string nodeID);
    }
}
