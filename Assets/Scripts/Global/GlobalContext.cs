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
            injectionBinder.Bind<IFontConfig>().To<FontConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILanConfig>().To<LanConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<II18NConfig>().To<I18NConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IAudioConfig>().To<AudioConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IModuleConfig>().To<ModuleConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IItemConfig>().To<ItemConfig>().ToSingleton().CrossContext();

            injectionBinder.Bind<IMapConfig>().To<MapConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<IScenicConfig>().To<ScenicConfig>().ToSingleton().CrossContext();
            injectionBinder.Bind<ICardConfig>().To<CardConfig>().ToSingleton().CrossContext();

            injectionBinder.Bind<IJsonUtil>().To<JsonUtil>().ToSingleton().CrossContext();
            injectionBinder.Bind<IPreferenceHelper>().To<PreferenceHelper>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILocalDataHelper>().To<LocalDataHelper>().ToSingleton().CrossContext();
            injectionBinder.Bind<IChildInfoManager>().To<ChildInfoManager>().ToSingleton().CrossContext();
            injectionBinder.Bind<IGameDataHelper>().To<GameDataHelper>().ToSingleton().CrossContext();
            injectionBinder.Bind<ITokenManager>().To<TokenManager>().ToSingleton().CrossContext();

            injectionBinder.Bind<IPrefabUtil>().To<PrefabUtil>().ToSingleton().CrossContext();
            injectionBinder.Bind<II18NUtil>().To<I18NUtil>().ToSingleton().CrossContext();
            injectionBinder.Bind<IHotUpdateUtil>().To<HotUpdateUtil>().ToSingleton().CrossContext();
            injectionBinder.Bind<IResourceUtil>().To<ResourceUtil>().ToSingleton().CrossContext();
            injectionBinder.Bind<IAssetBundleUtil>().To<AssetBundleUtil>().ToSingleton().CrossContext();
            injectionBinder.Bind<ICommonResourceUtil>().To<CommonResourceUtil>().ToSingleton().CrossContext();
            injectionBinder.Bind<ICommonImageUtils>().To<CommonImageUtils>().ToSingleton().CrossContext();

            injectionBinder.Bind<IUrlProvider>().To<UrlProvider>().ToSingleton().CrossContext();
            injectionBinder.Bind<INetUtils>().To<NetUtils>().ToSingleton().CrossContext();
            injectionBinder.Bind<IGululuNetwork>().To<GululuNetwork>().ToSingleton().CrossContext();
            injectionBinder.Bind<INativeOkHttpMethodWrapper>().To<NativeOkHttpMethodWrapper>().ToSingleton().CrossContext();
            injectionBinder.Bind<IAuthenticationUtils>().To<AuthenticationUtils>().ToSingleton().CrossContext();

            injectionBinder.Bind<IDeviceInfoManager>().To<DeviceInfoManager>().ToSingleton().CrossContext();
        }
    }
}

