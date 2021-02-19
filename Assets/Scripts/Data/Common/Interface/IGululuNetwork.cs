using System;
using System.Collections.Generic;
using AppGame.Util;
using BestHTTP;

namespace AppGame.Data.Common
{
    public interface IGululuNetwork
    {
        IAuthenticationUtils mAuthenticationUtils { get; set; }

        IJsonUtils mJsonUtils { get; set; }

        ClearLocalDataSignal mClearLocalDataSignal { get; set; }

        void sendRequest(string url, IDictionary<string, string> headrs, string bodyContent, Action<string> callBack, Action<ResponseErroInfo> errCallBack, HTTPMethods methods);

        void sendRequest(string url, IDictionary<string, string> headrs, Action<string> callBack, Action<ResponseErroInfo> errCallBack, HTTPMethods methods);

    }
}