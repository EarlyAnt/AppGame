using System.Collections.Generic;
using UnityEngine;

namespace AppGame.Module.Cycling
{
    public class MapPointNode : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private string id;
        [SerializeField]
        private NodeTypes nodeType;
        public string ID { get { return this.id; } }
        public NodeTypes NodeType { get { return this.nodeType; } }
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/

    }
}
