using AppGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public abstract class BasePlayer : BaseView
    {
        /************************************************属性与变量命名************************************************/
        #region 注入接口

        #endregion
        #region 页面UI组件
        [SerializeField]
        protected Image avatarBox;
        [SerializeField]
        protected MapNode mapNode;
        [SerializeField]
        protected Transform player;
        [SerializeField, Range(0f, 5f)]
        protected float step;
        #endregion
        #region 其他变量
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
        //初始化
        protected virtual void Initialize()
        {
            this.player.position = this.mapNode.Points[this.nodeIndex].position;
        }
        //移动到指定位置
        public abstract void MoveToNode(string nodeID, bool lerp = false);
    }
}
