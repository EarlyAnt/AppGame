using System.Collections.Generic;
using UnityEngine;

namespace AppGame.Module.Cycling
{
    public class MapPointNode : MonoBehaviour
    {
        /************************************************�������������************************************************/
        [SerializeField]
        private string id;
        [SerializeField]
        private NodeTypes nodeType;
        public string ID { get { return this.id; } }
        public NodeTypes NodeType { get { return this.nodeType; } }
        /************************************************Unity�������¼�***********************************************/

        /************************************************�� �� �� �� ��************************************************/

    }
}
