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
        [SerializeField]
        private string normalImageName;
        [SerializeField]
        private string lightImageName;
        private SpriteLoader spriteLoader;
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
        //点亮图标
        public void SetIcon(bool light)
        {
            if (this.spriteLoader == null)
                this.spriteLoader = this.GetComponent<SpriteLoader>();

            if (this.spriteLoader != null)
                this.spriteLoader.LoadImage(light ? this.lightImageName : this.normalImageName);
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
        [ContextMenu("设置图标名字")]
        public void SetIconName()
        {
            switch (this.nodeType)
            {
                case NodeTypes.StartNode:
                    this.normalImageName = "start_normal ";
                    this.lightImageName = "stars_highlight ";
                    break;
                case NodeTypes.EventNode:
                    this.normalImageName = "site_normal";
                    this.lightImageName = "site_highlight";
                    break;
                case NodeTypes.SiteNode:
                    this.normalImageName = "scenicspot_normal";
                    this.lightImageName = "scenicspot_highlight";
                    break;
                case NodeTypes.EndNode:
                    this.normalImageName = "ticket_normal ";
                    this.lightImageName = "ticket _highlight";
                    break;
            }
        }
    }
}
