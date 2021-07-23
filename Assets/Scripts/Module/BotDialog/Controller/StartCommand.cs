using AppGame.Util;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace AppGame.Module.BotDialog
{
    public class StartCommand : Command
    {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject contextView { get; set; }

        [Inject]
        public IPrefabUtil PrefabUtil { get; set; }

        public override void Execute()
        {
            GameObject view = this.PrefabUtil.CreateGameObject("BotDialog", "BotDialog");
            view.transform.SetParent(this.contextView.transform);
            view.transform.localPosition = Vector3.zero;
            view.transform.localRotation = Quaternion.identity;
            view.transform.localScale = Vector3.one;
            view.SetActive(true);
        }
    }
}

