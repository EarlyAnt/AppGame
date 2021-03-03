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
        public IChildInfoManager ChildInfoManager { get; set; }
        [Inject]
        public INativeOkHttpMethodWrapper NativeOkHttpMethodWrapper { get; set; }
        [Inject]
        public ITokenManager TokenManager { get; set; }

        public void GetVerifyCode(string phone, Action<Result> callBack, Action<ResponseErroInfo> errCallBack)
        {
            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("phone", phone);

            HTTPRequest hTTPRequest = new HTTPRequest(new Uri(this.UrlProvider.GetVerifyCodeUrl()), HTTPMethods.Post, (request, response) =>
            {
                if (request != null && response != null)
                {
                    Debug.LogFormat("--------AuthenticationUtils.GetVerifyCode.GotResponse--------");
                    Debug.LogFormat("response.IsSuccess: {0}", response.IsSuccess);
                    Debug.LogFormat("response.DataAsText: {0}", response.DataAsText);
                    Debug.LogFormat("response.Message: {0}", response.Message);
                    Debug.LogFormat("request.State: {0}", request.State);

                    if (response.IsSuccess)
                    {
                        DataBase data = JsonUtils.String2Json<DataBase>(response.DataAsText);
                        if (callBack != null) callBack(Result.Success(response.Message));
                    }
                    else
                    {
                        if (errCallBack != null) errCallBack(ResponseErroInfo.GetErrorInfo(0, "<>Error: parameter 'request' or 'response' is null"));
                    }
                }
                else if (response == null)
                {
                    Debug.LogError("<><AuthenticationUtils.GetVerifyCode>callback 'response' is null");
                    return;
                }
                else if (request == null)
                {
                    Debug.LogError("<><AuthenticationUtils.GetVerifyCode>callback 'request' is null");
                    return;
                }
            });

            string strBody = JsonUtils.Dictionary2String(body);
            hTTPRequest.RawData = Encoding.UTF8.GetBytes(strBody);
            hTTPRequest.AddHeader("Gululu-Agent", GululuNetworkHelper.GetAgent());
            hTTPRequest.AddHeader("udid", GululuNetworkHelper.GetUdid());
            hTTPRequest.AddHeader("Accept-Language", GululuNetworkHelper.GetAcceptLang());
            hTTPRequest.SetHeader("Content-Type", "application/json");
            hTTPRequest.Send();
        }

        public void Login(LoginData loginData, Action<Result> callBack, Action<ResponseErroInfo> errCallBack)
        {
            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("phone", loginData.Phone);
            body.Add("verify_code", loginData.VerifyCode);
            body.Add("name", loginData.Name);
            body.Add("user_email", loginData.Email);
            body.Add("user_password", loginData.Password);

            HTTPRequest hTTPRequest = new HTTPRequest(new Uri(this.UrlProvider.LoginUrl()), HTTPMethods.Post, (request, response) =>
            {
                if (request != null && response != null)
                {
                    Debug.LogFormat("--------AuthenticationUtils.Login.GotResponse--------");
                    Debug.LogFormat("response.IsSuccess: {0}", response.IsSuccess);
                    Debug.LogFormat("response.DataAsText: {0}", response.DataAsText);
                    Debug.LogFormat("response.Message: {0}", response.Message);
                    Debug.LogFormat("request.State: {0}", request.State);

                    if (response.IsSuccess)
                    {
                        LoginResponseData data = JsonUtils.String2Json<LoginResponseData>(response.DataAsText);
                        if (data != null) this.TokenManager.SaveToken(data.token);
                        if (callBack != null) callBack(Result.Success(response.Message));
                    }
                    else
                    {
                        if (errCallBack != null) errCallBack(ResponseErroInfo.GetErrorInfo(0, "<>Error: parameter 'request' or 'response' is null"));
                    }
                }
                else if (response == null)
                {
                    Debug.LogError("<><AuthenticationUtils.Login>callback 'response' is null");
                    return;
                }
                else if (request == null)
                {
                    Debug.LogError("<><AuthenticationUtils.Login>callback 'request' is null");
                    return;
                }
            });

            string strBody = JsonUtils.Dictionary2String(body);
            hTTPRequest.RawData = Encoding.UTF8.GetBytes(strBody);
            hTTPRequest.AddHeader("Gululu-Agent", GululuNetworkHelper.GetAgent());
            hTTPRequest.AddHeader("udid", GululuNetworkHelper.GetUdid());
            hTTPRequest.AddHeader("Accept-Language", GululuNetworkHelper.GetAcceptLang());
            hTTPRequest.SetHeader("Content-Type", "application/json");
            hTTPRequest.Send();
        }

        public void GetToken(Action<Result> callBack, Action<ResponseErroInfo> errCallBack)
        {
            HTTPRequest hTTPRequest = new HTTPRequest(new Uri(this.UrlProvider.GetTokenUrl(this.ChildInfoManager.GetChildSN())), HTTPMethods.Get, (request, response) =>
            {
                if (request != null && response != null)
                {
                    Debug.LogFormat("--------AuthenticationUtils.GetToken.GotResponse--------");
                    Debug.LogFormat("response.IsSuccess: {0}", response.IsSuccess);
                    Debug.LogFormat("response.DataAsText: {0}", response.DataAsText);
                    Debug.LogFormat("response.Message: {0}", response.Message);
                    Debug.LogFormat("request.State: {0}", request.State);

                    if (response.IsSuccess)
                    {
                        GetTokenResponseData tokenResponseData = JsonUtils.String2Json<GetTokenResponseData>(response.DataAsText);
                        string token = tokenResponseData.token;
                        //this.LocalCupAgent.SaveCupToken(CupBuild.getCupSn(), token);
                        if (callBack != null) callBack(Result.Success(token));
                    }
                    else
                    {
                        if (errCallBack != null) errCallBack(ResponseErroInfo.GetErrorInfo(0, "<>Error: parameter 'request' or 'response' is null"));
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

            hTTPRequest.AddHeader("Gululu-Agent", GululuNetworkHelper.GetAgent());
            hTTPRequest.AddHeader("udid", GululuNetworkHelper.GetUdid());
            hTTPRequest.AddHeader("Accept-Language", GululuNetworkHelper.GetAcceptLang());
            hTTPRequest.AddHeader("token", this.TokenManager.GetToken());
            hTTPRequest.SetHeader("Content-Type", "application/json");
            hTTPRequest.Send();
        }
    }
}