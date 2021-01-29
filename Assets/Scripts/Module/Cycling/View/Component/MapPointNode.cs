using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class MapPointNode : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private string id;
        [SerializeField]
        private bool idLock;
        [SerializeField]
        private NodeTypes nodeType;
        public string ID { get { return this.id; } }
        public NodeTypes NodeType { get { return this.nodeType; } }
        private static Color START_NODE_COLOR = new Color(0.286f, 1f, 0.357f, 1f);
        private static Color SITE_NODE_COLOR = new Color(1f, 0.286f, 0.443f, 1f);
        private static Color TRANSPARENT_COLOR = new Color(1f, 1f, 1f, 0f);
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        //设置节点ID
        public void SetID(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogErrorFormat("<><MapPointNode.SetID>Error: parameter 'id' is null or empty, {0}", this.name);
            }
            else if (this.idLock)
            {
                Debug.LogErrorFormat("<><MapPointNode.SetID>Error: id is locked, {0}", this.name);
            }
            else
            {
                this.id = id;
                this.idLock = true;
            }
        }
        //更改节点类型
        public void ChangeNodeType(NodeTypes nodeType)
        {
            this.nodeType = nodeType;
            this.SetNodeColor();
        }
        [ContextMenu("更改节点颜色")]
        public void SetNodeColor()
        {
            Image image = this.GetComponent<Image>();
            if (image != null)
            {
                switch (this.nodeType)
                {
                    case NodeTypes.StartNode:
                        image.color = START_NODE_COLOR;
                        break;
                    case NodeTypes.EmptyNode:
                        image.color = TRANSPARENT_COLOR;
                        break;
                    case NodeTypes.EventNode:
                        image.color = Color.white;
                        break;
                    case NodeTypes.SiteNode:
                        image.color = SITE_NODE_COLOR;
                        break;
                    case NodeTypes.EndNode:
                        image.color = START_NODE_COLOR;
                        break;
                }
            }
        }
    }
}
