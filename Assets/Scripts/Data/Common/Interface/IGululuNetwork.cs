using AppGame.Data.Remote;
using AppGame.Util;
using BestHTTP;
using System;
using System.Collections.Generic;

namespace AppGame.Data.Common
{
    public interface IGululuNetwork
    {
        IAuthenticationUtils AuthenticationUtils { get; set; }
        IJsonUtils JsonUtils { get; set; }
        //ClearLocalDataSignal ClearLocalDataSignal { get; set; }

        void SendRequest(string url, IDictionary<string, string> headrs, string bodyContent, Action<string> callBack, Action<ResponseErroInfo> errCallBack, HTTPMethods methods);
        void SendRequest(string url, IDictionary<string, string> headrs, Action<string> callBack, Action<ResponseErroInfo> errCallBack, HTTPMethods methods);
    }
}