using strange.extensions.context.impl;
using UnityEngine;

namespace AppGame.Global
{
    public class GlobalRoot : ContextView
    {
        void Awake()
        {
            GameObject.DontDestroyOnLoad(this);
            this.context = new GlobalContext(this, strange.extensions.context.api.ContextStartupFlags.AUTOMATIC);
        }
    }
}

