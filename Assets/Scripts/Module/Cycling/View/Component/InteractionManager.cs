using AppGame.UI;
using System.Collections.Generic;
using UnityEngine;

namespace AppGame.Module.Cycling
{
    public class InteractionManager : BaseView
    {
        /************************************************属性与变量命名************************************************/
        private Queue<BaseInteraction> interactionList;
        private BaseInteraction currentInteraction;
        private bool initialized;
        /************************************************Unity方法与事件***********************************************/
        protected override void Awake()
        {
            base.Awake();
            this.Initialize();
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        private void Update()
        {
            if (!this.initialized) return;

            if ((this.currentInteraction == null || this.currentInteraction.Complete) && this.interactionList.Count > 0)
            {
                this.currentInteraction = this.interactionList.Dequeue();
                this.currentInteraction.Show();
            }
        }
        /************************************************自 定 义 方 法************************************************/
        private void Initialize()
        {
            this.interactionList = new Queue<BaseInteraction>();
            this.initialized = true;
        }

        public void Enqueue(BaseInteraction interaction)
        {
            this.interactionList.Enqueue(interaction);
        }
    }
}
