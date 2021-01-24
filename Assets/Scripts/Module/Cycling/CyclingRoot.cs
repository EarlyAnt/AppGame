using strange.extensions.context.impl;

namespace AppGame.Module.Cycling
{
    public class CyclingRoot : ContextView
    {
        void Awake()
        {
            context = new CyclingContext(this);
        }
    }
}

