using AppGame.Data.Local;
using AppGame.Data.Remote;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace AppGame.Module.BotDialog
{
    public class BotDialogContext : MVCSContext
    {
        public BotDialogContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }

        protected override void addCoreComponents()
        {
            base.addCoreComponents();
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }

        public override void Launch()
        {
            base.Launch();
            StartSignal startSignal = (StartSignal)injectionBinder.GetInstance<StartSignal>();
            startSignal.Dispatch();
        }

        protected override void mapBindings()
        {
            //bind mediation
            mediationBinder.Bind<BotDialogView>().To<BotDialogMediator>();

            //bind command
            commandBinder.Bind<StartSignal>().To<StartCommand>();

            //bind injection
            injectionBinder.Bind<IBotDialogDataUtil>().To<BotDialogDataUtil>().ToSingleton();
        }
    }
}

