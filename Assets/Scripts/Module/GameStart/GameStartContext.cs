using AppGame.Data.Common;
using AppGame.Data.Local;
using AppGame.Data.Remote;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace AppGame.Module.GameStart
{
    public class GameStartContext : MVCSContext
    {
        public GameStartContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
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
            base.mapBindings();

            //bind mediation
            mediationBinder.Bind<GameStartView>().To<GameStartMediator>();

            //bind command
            commandBinder.Bind<StartSignal>().To<StartCommand>();
            commandBinder.Bind<UploadItemDataSignal>().To<UploadItemDataCommand>();

            //bind injection
            injectionBinder.Bind<IItemDataManager>().To<ItemDataManager>().ToSingleton();
            injectionBinder.Bind<IItemDataUtil>().To<ItemDataUtil>().ToSingleton();

            injectionBinder.Bind<ICyclingDataUtil>().To<CyclingDataUtil>().ToSingleton();
            injectionBinder.Bind<IBasicDataManager>().To<BasicDataManager>().ToSingleton();
            injectionBinder.Bind<ICyclingDataManager>().To<CyclingDataManager>().ToSingleton();
        }
    }
}

