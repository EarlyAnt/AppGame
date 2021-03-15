using AppGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class MapPointNode : BaseView
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

            Image icon = this.GetComponent<Image>();
            if (icon != null) icon.raycastTarget = light;
        }
        //点击按钮
        public void NodeClick()
        {
            this.dispatcher.Dispatch(GameEvent.INTERACTION, this);
        }
        //更改节点类型
        public void ChangeNodeType(NodeTypes nodeType)
        {
            this.nodeType = nodeType;
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
