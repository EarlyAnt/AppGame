using strange.extensions.context.impl;

namespace AppGame.Module.BotDialog
{
    public class BotDialogRoot : ContextView
    {
        void Awake()
        {
            this.context = new BotDialogContext(this, strange.extensions.context.api.ContextStartupFlags.MANUAL_LAUNCH);
        }

        private void OnEnable()
        {
            if (this.context != null)
                this.context.Launch();
        }
    }
}

