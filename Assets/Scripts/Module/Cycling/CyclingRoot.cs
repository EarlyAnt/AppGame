using strange.extensions.context.impl;

namespace AppGame.Module.Cycling
{
    public class CyclingRoot : ContextView
    {
        void Awake()
        {
            this.context = new CyclingContext(this, strange.extensions.context.api.ContextStartupFlags.MANUAL_LAUNCH);
        }

        private void OnEnable()
        {
            if (this.context != null)
                this.context.Launch();
        }
    }
}

