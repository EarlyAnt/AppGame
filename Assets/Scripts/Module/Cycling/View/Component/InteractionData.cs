using AppGame.UI;
using UnityEngine;

namespace AppGame.Module.Cycling
{
    public class InteractionData : MonoBehaviour
    {
        /************************************************�������������************************************************/
        [SerializeField]
        private string id;
        [SerializeField]
        private Interactions interaction;
        public string ID { get { return this.id; } }
        public Interactions Interacton { get { return this.interaction; } }
        /************************************************Unity�������¼�***********************************************/

        /************************************************�� �� �� �� ��************************************************/
    }
}
