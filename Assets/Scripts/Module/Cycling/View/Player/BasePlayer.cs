using AppGame.UI;
using DG.Tweening;
using System.Collections;
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
        protected MapNode mapNode;
        [SerializeField]
        protected Transform player;

        #endregion
        #region 其他变量
        private int nodeIndex;
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
        private void Initialize()
        {
            this.player.position = this.mapNode.Points[this.nodeIndex].position;
        }
        //移动到指定位置
        public abstract void MoveToNode(string nodeID);
    }
}
