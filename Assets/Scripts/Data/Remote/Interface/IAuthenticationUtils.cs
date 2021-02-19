using AppGame.Data.Local;
using AppGame.Util;
using System;

namespace AppGame.Data.Remote
{
    public interface IAuthenticationUtils
    {
        IUrlProvider UrlProvider{get;set;}
        IJsonUtils JsonUtils{get;set;}
        ILocalCupAgent LocalCupAgent{get;set;}
        void reNewToken(Action<Result> callBack, Action<Result> errCallBack);
    }
}