using strange.extensions.context.impl;

namespace AppGame.Module.GameStart
{
    public class GameStartRoot : ContextView
    {
        void Awake()
        {
            context = new GameStartContext(this);
        }
    }
}

