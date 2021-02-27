using AppGame.Config;
using AppGame.Data.Common;
using AppGame.Data.Local;
using AppGame.Data.Remote;
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
            injectionBinder.Bind<IJsonUtils>().To<JsonUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<IPrefabUtil>().To<PrefabUtil>().ToSingleton().CrossContext();
            injectionBinder.Bind<II18NUtil>().To<I18NUtil>().ToSingleton().CrossContext();

            injectionBinder.Bind<IHotUpdateUtil>().To<HotUpdateUtil>().ToSingleton().CrossContext();
            injectionBinder.Bind<IResourceUtil>().To<ResourceUtil>().ToSingleton().CrossContext();
            injectionBinder.Bind<IAssetBundleUtil>().To<AssetBundleUtil>().ToSingleton().CrossContext();
            injectionBinder.Bind<ICommonResourceUtil>().To<CommonResourceUtil>().ToSingleton().CrossContext();

            injectionBinder.Bind<IFontConfig>().To<FontConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILanConfig>().To<LanConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<II18NConfig>().To<I18NConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IAudioConfig>().To<AudioConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IModuleConfig>().To<ModuleConfig>().ToSingleton().CrossContext();

            injectionBinder.Bind<IMapConfig>().To<MapConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IScenicConfig>().To<ScenicConfig>().ToSingleton().CrossContext();

            injectionBinder.Bind<ILocalDataManager>().To<LocalDataManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILocalChildInfoAgent>().To<LocalChildInfoAgent>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILocalCupAgent>().To<LocalCupAgent>().ToSingleton().CrossContext();

            injectionBinder.Bind<ICommonImageUtils>().To<CommonImageUtils>().ToSingleton().CrossContext();

            injectionBinder.Bind<IUrlProvider>().To<UrlProvider>().ToSingleton().CrossContext();
            injectionBinder.Bind<INetUtils>().To<NetUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<IGululuNetwork>().To<GululuNetwork>().ToSingleton().CrossContext();
            injectionBinder.Bind<INativeOkHttpMethodWrapper>().To<NativeOkHttpMethodWrapper>().ToSingleton().CrossContext();
            injectionBinder.Bind<IAuthenticationUtils>().To<AuthenticationUtils>().ToSingleton().CrossContext();
        }
    }
}

