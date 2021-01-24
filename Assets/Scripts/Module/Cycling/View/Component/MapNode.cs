using System.Collections.Generic;
using UnityEngine;

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
        [ContextMenu("�ռ���ͼ��")]
        private void CollectPoints()
        {
            this.points = new List<Transform>();
            for (int i = 0; i < this.transform.childCount; i++)
            {
                this.transform.GetChild(i).name = string.Format("Point_{0}", i + 1);
                this.points.Add(this.transform.GetChild(i));
            }
        }
    }
}
