using AppGame.UI;
using UnityEngine;

namespace AppGame.Module.Cycling
{
    public class InteractionData : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private string id;
        [SerializeField]
        private Interactions interaction;
        public string ID { get { return this.id; } }
        public Interactions Interacton { get { return this.interaction; } }
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
    }
}
