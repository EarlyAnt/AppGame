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
        private Transform pointRoot;
        [SerializeField]
        private List<Transform> points;
        public string ID { get { return this.id; } }
        public List<Transform> Points { get { return this.points; } }
        /************************************************Unity�������¼�***********************************************/

        /************************************************�� �� �� �� ��************************************************/
        [ContextMenu("�ռ���ͼ��")]
        private void CollectPoints()
        {
            this.points = new List<Transform>();
            for (int i = 0; i < this.pointRoot.childCount; i++)
            {
                this.pointRoot.GetChild(i).name = string.Format("Point_{0}", i + 1);
                this.points.Add(this.pointRoot.GetChild(i));
            }
        }
        [ContextMenu("����MapNodePoint���")]
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
        [ContextMenu("���ýڵ�ID")]
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
