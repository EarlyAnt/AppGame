using AppGame.Global;
using strange.extensions.context.impl;
using UnityEngine;

namespace AppGame.Module.GameStart
{
    public class GameStartRoot : ContextView
    {
        void Awake()
        {
            GameObject gameObject = new GameObject("GlobalContext");
            gameObject.AddComponent<GlobalRoot>();
            GameObject.DontDestroyOnLoad(gameObject);
            this.context = new GameStartContext(this, strange.extensions.context.api.ContextStartupFlags.MANUAL_LAUNCH);
        }

        private void OnEnable()
        {
            if (this.context != null)
                this.context.Launch();
        }
    }
}

