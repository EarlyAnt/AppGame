using AppGame.Config;
using AppGame.Util;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace AppGame.Global
{
    public class GlobalContext : MVCSContext
    {
        public GlobalContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }

        protected override void addCoreComponents()
        {
            base.addCoreComponents();
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }

        protected override void mapBindings()
        {
            base.mapBindings();

            //bind injection
            injectionBinder.Bind<IPrefabUtil>().To<PrefabUtil>().ToSingleton().CrossContext();
            injectionBinder.Bind<II18NUtil>().To<I18NUtil>().ToSingleton().CrossContext();

            injectionBinder.Bind<IHotUpdateUtils>().To<HotUpdateUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<IResourceUtils>().To<ResourceUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<IAssetBundleUtils>().To<AssetBundleUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<ICommonResourceUtils>().To<CommonResourceUtils>().ToSingleton().CrossContext();

            injectionBinder.Bind<IFontConfig>().To<FontConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILanConfig>().To<LanConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<II18NConfig>().To<I18NConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IAudioConfig>().To<AudioConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IModuleConfig>().To<ModuleConfig>().ToSingleton().CrossContext();

            injectionBinder.Bind<IMapConfig>().To<MapConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IScenicConfig>().To<ScenicConfig>().ToSingleton().CrossContext();
        }
    }
}

