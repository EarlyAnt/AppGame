using AppGame.UI;
using UnityEngine;

namespace AppGame.Module.Cycling
{
    public abstract class BaseInteraction : BaseView
    {
        /************************************************�������������************************************************/
        [SerializeField]
        private string id;
        [SerializeField]
        private Interactions interaction;
        public string ID { get { return this.id; } }
        public Interactions Interacton { get { return this.interaction; } }
        public bool Complete { get; protected set; }
        /************************************************Unity�������¼�***********************************************/

        /************************************************�� �� �� �� ��************************************************/
        public abstract void Show();

        public abstract void Hide();
    }
}
