using AppGame.Config;
using AppGame.Util;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace AppGame.Module.GameStart
{
    public class GameStartContext : MVCSContext
    {
        public GameStartContext(MonoBehaviour view) : base(view)
        {
        }

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
            //bind mediation
            mediationBinder.Bind<GameStartView>().To<GameStartMediator>();

            //bind command
            commandBinder.Bind<StartSignal>().To<StartCommand>();

            //bind injection
            injectionBinder.Bind<IPrefabUtil>().To<PrefabUtil>().ToSingleton().CrossContext();

            injectionBinder.Bind<IMapConfig>().To<MapConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IScenicConfig>().To<ScenicConfig>().ToSingleton().CrossContext();
        }
    }
}

