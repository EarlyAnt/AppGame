using AppGame.Data.Common;
using AppGame.Data.Local;
using AppGame.Util;
using System;

namespace AppGame.Data.Remote
{
    public interface IAuthenticationUtils
    {
        IUrlProvider UrlProvider { get; set; }
        IJsonUtils JsonUtils { get; set; }
        ILocalChildInfoAgent LocalChildInfoAgent { get; set; }
        void GetToken(Action<Result> callBack, Action<Result> errCallBack);
    }

    public class GetTokenResponseData : DataBase
    {
        public string token;
    }
}