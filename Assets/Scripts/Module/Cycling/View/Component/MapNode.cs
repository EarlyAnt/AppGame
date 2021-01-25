using System.Collections.Generic;
using UnityEngine;

namespace AppGame.Module.Cycling
{
    public class MapNode : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private string id;
        [SerializeField]
        private Transform pointRoot;
        [SerializeField]
        private List<Transform> points;
        public string ID { get { return this.id; } }
        public List<Transform> Points { get { return this.points; } }
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        [ContextMenu("收集地图点")]
        private void CollectPoints()
        {
            this.points = new List<Transform>();
            for (int i = 0; i < this.pointRoot.childCount; i++)
            {
                this.pointRoot.GetChild(i).name = string.Format("Point_{0}", i + 1);
                this.points.Add(this.pointRoot.GetChild(i));
            }
        }
    }
}
