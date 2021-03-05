using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class MapNode : MonoBehaviour
    {
        /************************************************�������������************************************************/
        [SerializeField]
        private string id;
        [SerializeField]
        private List<Transform> points;
        public string ID { get { return this.id; } }
        public List<Transform> Points { get { return this.points; } }
        /************************************************Unity�������¼�***********************************************/

        /************************************************�� �� �� �� ��************************************************/
        [ContextMenu("1-�ռ���ͼ��")]
        private void CollectPoints()
        {
            this.points = new List<Transform>();
            for (int i = 0; i < this.transform.childCount; i++)
            {
                this.points.Add(this.transform.GetChild(i));
            }
        }
        [ContextMenu("2-����MapNodePoint���")]
        private void AddNodeScript()
        {
            if (this.points == null || this.points.Count == 0)
            {
                Debug.LogError("<><MapNode.AddNodeScript>Error: parameter 'this.points' is null or empty");
                return;
            }

            for (int i = 0; i < this.points.Count; i++)
            {
                MapPointNode mapPointNode = this.points[i].GetComponent<MapPointNode>();
                if (mapPointNode == null)
                {
                    mapPointNode = this.points[i].gameObject.AddComponent<MapPointNode>();
                    Image image = this.points[i].GetComponent<Image>();
                    mapPointNode.ChangeNodeType(image != null && image.color == Color.white ? NodeTypes.EventNode : NodeTypes.EmptyNode);
                }
                else
                {
                    mapPointNode.SetNodeColor();
                }
            }
        }
        [ContextMenu("3-���ýڵ�����")]
        private void SetNodeName()
        {
            if (this.points == null || this.points.Count == 0)
            {
                Debug.LogError("<><MapNode.SetNodeName>Error: parameter 'this.points' is null or empty");
                return;
            }

            for (int i = 0; i < this.points.Count; i++)
            {
                
                MapPointNode mapPointNode = this.points[i].GetComponent<MapPointNode>();
                if (mapPointNode != null)
                {
                    this.points[i].name = string.Format("{0}_{1}", mapPointNode.NodeType, i + 1);
                }
                else
                {
                    Debug.LogErrorFormat("<><MapNode.SetNodeName>Error: no MapPointNode script on this node: {0}", mapPointNode.name);
                    return;
                }
            }
        }
        [ContextMenu("4-���ýڵ�ID")]
        private void SetNodeID()
        {
            if (this.points == null || this.points.Count == 0)
            {
                Debug.LogError("<><MapNode.SetNodeID>Error: parameter 'this.points' is null or empty");
                return;
            }

            for (int i = 0; i < this.points.Count; i++)
            {
                MapPointNode mapPointNode = this.points[i].GetComponent<MapPointNode>();
                if (mapPointNode != null)
                {
                    mapPointNode.SetID(string.Format("{0}_{1:d2}", this.id, i + 1));
                }
                else
                {
                    Debug.LogErrorFormat("<><MapNode.SetNodeID>Error: no MapPointNode script on this node: {0}", mapPointNode.name);
                    return;
                }
            }
        }
    }
}
