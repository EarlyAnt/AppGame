using AppGame.UI;
using UnityEngine;

namespace AppGame.Module.Cycling
{
    public abstract class BaseInteraction : BaseView
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private string id;
        [SerializeField]
        private Interactions interaction;
        public string ID { get { return this.id; } }
        public Interactions Interacton { get { return this.interaction; } }
        public bool Complete { get; protected set; }
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        public abstract void Show();

        public abstract void Hide();
    }
}
