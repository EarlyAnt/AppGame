using AppGame.Data.Remote;
using AppGame.Util;

namespace AppGame.Data.Common
{
    public abstract class BaseService
    {
        [Inject]
        public IJsonUtil JsonUtils { set; get; }

        [Inject]
        public IUrlProvider UrlProvider { set; get; }

        [Inject]
        public INativeOkHttpMethodWrapper NativeOkHttpMethodWrapper { get; set; }
    }
}