using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class Teammate : BasePlayer
    {
        /************************************************属性与变量命名************************************************/
        #region 注入接口

        #endregion
        #region 页面UI组件

        #endregion
        #region 其他变量

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
        //移动到指定位置
        public override void MoveToNode(string nodeID, bool lerp = false)
        {
            if (this.mapNode == null || this.mapNode.Points == null)
            {
                Debug.LogWarning("<><Teammate.MoveToNode>Warning: parameter 'mapNode' or 'mapNode.Points' is null");
                return;
            }

            int index = this.mapNode.Points.FindIndex(t => t.GetComponent<MapPointNode>().ID == nodeID);
            if (index >= 0 && index < this.mapNode.Points.Count)
            {
                if (lerp)
                {
                    this.StopAllCoroutines();
                    this.StartCoroutine(this.MovePlayer(this.mapNode.Points[this.nodeIndex]));
                }
                else
                {
                    this.nodeIndex = index;
                    this.player.position = this.mapNode.Points[this.nodeIndex].position;
                }
            }
            else
            {
                Debug.LogErrorFormat("<><Teammate.MoveToNode>Error: can not find the node named '{0}'", nodeID);
            }
        }
        //玩家移动
        private IEnumerator MovePlayer(Transform targetNode)
        {
            do
            {
                this.nodeIndex += 1;
                do
                {
                    this.player.position = Vector3.MoveTowards(this.player.position, this.destination, this.step);
                    yield return new WaitForEndOfFrame();
                }
                while (Vector3.Distance(this.player.position, this.destination) > 0.01f);
            }
            while (this.mapNode.Points[this.nodeIndex] == targetNode);
            yield return new WaitForSeconds(0.5f);
            Debug.Log("<><Teammate.MovePlayer>Stop + + + + +");
        }
    }
}
