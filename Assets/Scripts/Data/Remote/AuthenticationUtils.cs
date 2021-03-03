using AppGame.Data.Common;
using AppGame.Data.Local;
using AppGame.Util;
using BestHTTP;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AppGame.Data.Remote
{
    public class AuthenticationUtils : IAuthenticationUtils
    {
        [Inject]
        public IUrlProvider UrlProvider { get; set; }
        [Inject]
        public IJsonUtil JsonUtils { get; set; }
        [Inject]
        public ILocalCupAgent LocalCupAgent { get; set; }
        [Inject]
        public GululuNetworkHelper mGululuNetworkHelper { get; set; }

        public void reNewToken(Action<Result> callBack, Action<Result> errCallBack)
        {
            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("access_cup", CupBuild.getCupSn());

            HTTPRequest hTTPRequest = new HTTPRequest(new Uri(UrlProvider.GetCupTokenUrl(CupBuild.getCupSn())), HTTPMethods.Post, (request, response) =>
            {
                if (request != null && response != null)
                {
                    Debug.LogFormat("--------AuthenticationUtils.reNewToken.GotResponse--------");
                    Debug.LogFormat("response.IsSuccess: {0}", response.IsSuccess);
                    Debug.LogFormat("response.DataAsText: {0}", response.DataAsText);
                    Debug.LogFormat("response.Message: {0}", response.Message);
                    Debug.LogFormat("request.State: {0}", request.State);

                    if (response.IsSuccess)
                    {
                        TokenResponseData mTokenResponseData = JsonUtils.String2Json<TokenResponseData>(response.DataAsText);
                        string token = mTokenResponseData.token;
                        this.LocalCupAgent.SaveCupToken(CupBuild.getCupSn(), token);
                        if (callBack != null) callBack(Result.Success(token));
                    }
                    else
                    {
                        if (errCallBack != null) errCallBack(Result.Error());
                    }
                }
                else if (response == null)
                {
                    Debug.LogError("<><AuthenticationUtils.reNewToken>callback 'response' is null");
                    return;
                }
                else if (request == null)
                {
                    Debug.LogError("<><AuthenticationUtils.reNewToken>callback 'request' is null");
                    return;
                }
            });

            string strBody = JsonUtils.Dictionary2String(body);
            hTTPRequest.RawData = Encoding.UTF8.GetBytes(strBody);

            hTTPRequest.AddHeader("Gululu-Agent", mGululuNetworkHelper.GetAgent());
            hTTPRequest.AddHeader("udid", mGululuNetworkHelper.GetUdid());
            hTTPRequest.AddHeader("Accept-Language", mGululuNetworkHelper.GetAcceptLang());
            hTTPRequest.SetHeader("Content-Type", "application/json");
            hTTPRequest.Send();
        }


    }

}