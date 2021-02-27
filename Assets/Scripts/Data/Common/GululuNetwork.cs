using AppGame.Data.Local;
using AppGame.Data.Remote;
using AppGame.Util;
using BestHTTP;
using LitJson;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AppGame.Data.Common
{
    public enum HandlerRespondeStatus
    {
        SUCCESS,
        RESPONSE_ERROR,
        REQUEST_ERROR,
    }

    public class GululuNetwork : IGululuNetwork
    {
        [Inject]
        public ILocalCupAgent LocalCupAgent { get; set; }
        [Inject]
        public IJsonUtils JsonUtils { get; set; }
        [Inject]
        public IAuthenticationUtils AuthenticationUtils { get; set; }
        //[Inject]
        //public ClearLocalDataSignal ClearLocalDataSignal { get; set; }

        public void SendRequest(string url, IDictionary<string, string> headrs, string bodyContent, Action<string> callBack, Action<ResponseErroInfo> errCallBack, HTTPMethods methods)
        {
            string requestInfo = "mothod:" + methods + " url:" + url + " body:" + bodyContent;
            Debug.LogFormat("GululuNetwork: {0}", requestInfo);

            HTTPRequest hTTPRequest = new HTTPRequest(new Uri(url), methods);
            hTTPRequest.Tag = RequestRetryInfo.allRetryCount;
            hTTPRequest.Callback = createRequestCallBack(callBack, errCallBack);
            hTTPRequest.ConnectTimeout = TimeSpan.FromSeconds(20);
            hTTPRequest.Timeout = TimeSpan.FromSeconds(20);
            setHeader(hTTPRequest, headrs);
            if (HTTPMethods.Get != methods)
            {
                setBody(hTTPRequest, bodyContent);
            }
            hTTPRequest.Send();
        }

        public void SendRequest(string url, IDictionary<string, string> headrs, Action<string> callBack, Action<ResponseErroInfo> errCallBack, HTTPMethods methods)
        {

            string requestInfo = "mothod:" + methods + " url:" + url;
            Debug.LogFormat("GululuNetwork: {0}", requestInfo);

            HTTPRequest hTTPRequest = new HTTPRequest(new Uri(url), methods);
            hTTPRequest.Tag = RequestRetryInfo.allRetryCount;
            hTTPRequest.ConnectTimeout = TimeSpan.FromSeconds(20);
            hTTPRequest.Timeout = TimeSpan.FromSeconds(20);
            hTTPRequest.Callback = createRequestCallBack(callBack, errCallBack);
            setHeader(hTTPRequest, headrs);
            hTTPRequest.Send();
        }

        public OnRequestFinishedDelegate createRequestCallBack(Action<string> callBack, Action<ResponseErroInfo> errCallBack)
        {
            OnRequestFinishedDelegate mOnRequestFinishedDelegate = delegate (HTTPRequest request, HTTPResponse response)
            {
                // Debug.Log("handleResqAndResp");
                handleResqAndResp(request, response, callBack, errCallBack);
            };
            return mOnRequestFinishedDelegate;
        }

        public void setHeader(HTTPRequest hTTPRequest, IDictionary<string, string> headrs)
        {
            hTTPRequest.AddHeader("Gululu-Agent", GululuNetworkHelper.GetAgent());
            hTTPRequest.AddHeader("udid", GululuNetworkHelper.GetUdid());
            hTTPRequest.AddHeader("Accept-Language", GululuNetworkHelper.GetAcceptLang());
            string token = this.LocalCupAgent.GetCupToken();
            if (token != null && token.Length != 0)
            {
                hTTPRequest.AddHeader("token", token);
            }
            if (headrs != null)
            {
                foreach (KeyValuePair<string, string> headr in headrs)
                {
                    hTTPRequest.AddHeader(headr.Key, headr.Value);
                }
            }
            hTTPRequest.SetHeader("Content-Type", "application/json");
        }


        public void setBody(HTTPRequest hTTPRequest, string bodyContent)
        {
            hTTPRequest.RawData = Encoding.UTF8.GetBytes(bodyContent);
        }


        public void handleResqAndResp(HTTPRequest originalRequest, HTTPResponse response, Action<string> callBack, Action<ResponseErroInfo> errCallBack)
        {
            //Debug.Log("<><>handleResqAndResp<><>" + originalRequest.State);
            int oddRetryCount = getOddRetryCount(originalRequest);

            switch (originalRequest.State)
            {
                case HTTPRequestStates.Finished:
                    string requestInfo = "Response url:" + originalRequest.Uri.AbsolutePath + " body:" + response.DataAsText;

                    Debug.Log(requestInfo);
                    if (response.IsSuccess)
                    {
                        callBack(response.DataAsText);

                    }
                    else
                    {
                        handleResponseError(originalRequest, response, errCallBack);
                    }
                    break;
                case HTTPRequestStates.Aborted:
                    errCallBack(ResponseErroInfo.GetErrorInfo(LocalRequestStatusCode.Aborted, "Request Aborted"));
                    break;
                case HTTPRequestStates.Initial:
                    errCallBack(ResponseErroInfo.GetErrorInfo(LocalRequestStatusCode.Initial, "Request Initial"));
                    break;
                case HTTPRequestStates.Processing:
                    errCallBack(ResponseErroInfo.GetErrorInfo(LocalRequestStatusCode.Processing, "Request Processing"));
                    break;
                case HTTPRequestStates.Queued:
                    errCallBack(ResponseErroInfo.GetErrorInfo(LocalRequestStatusCode.Queued, "Request Queued"));
                    break;
                case HTTPRequestStates.Error:
                    sendRetryRequest(oddRetryCount, errCallBack, originalRequest, "Error");
                    break;
                case HTTPRequestStates.ConnectionTimedOut:
                case HTTPRequestStates.TimedOut:
                    sendRetryRequest(oddRetryCount, errCallBack, originalRequest, "TimedOut");
                    break;
            }
        }

        private void sendRetryRequest(int oddRetryCount, Action<ResponseErroInfo> errCallBack, HTTPRequest originalRequest, string info)
        {
            if (oddRetryCount <= 0)
            {
                errCallBack(ResponseErroInfo.GetErrorInfo(LocalRequestStatusCode.TimedOut, info));
                Debug.Log("sendRetryRequest send request is over");
            }
            else
            {
                retrySendRequest(originalRequest, oddRetryCount);
            }
        }

        public void handleResponseError(HTTPRequest originalRequest, HTTPResponse response, Action<ResponseErroInfo> errCallBack)
        {
            if (response.StatusCode == 401)
            {
                handleAuthenticatioError(originalRequest, errCallBack);
            }
            else if (response.StatusCode == 503)
            {
                handleRetryRequest(response, originalRequest, errCallBack);

            }
            else
            {
                Debug.LogWarning("response.DataAsText:" + response.DataAsText + "  url:" + originalRequest.CurrentUri.ToString());
                if (isResetError(originalRequest, response))
                {
                    Debug.LogWarning("notify reset");
                    //ClearLocalDataSignal.Dispatch();
                }


                errCallBack(ResponseErroInfo.GetErrorInfo(response.StatusCode, response.DataAsText));
            }
        }

        public bool isResetError(HTTPRequest originalRequest, HTTPResponse response)
        {

            try
            {
                if (response.StatusCode == 400)
                {
                    string info = response.DataAsText;
                    JsonData errorData = JsonUtils.JsonStr2JsonData(info);
                    string error_code = (string)errorData["error_code"];

                    if (String.Equals(error_code, "C-004"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
            }
            return false;
        }

        public void handleRetryRequest(HTTPResponse response, HTTPRequest originalRequest, Action<ResponseErroInfo> errCallBack)
        {
            int oddRetryCount = getOddRetryCount(originalRequest);

            Debug.Log("<><>retryCount<><>" + oddRetryCount);

            if (oddRetryCount > 0)
            {
                retrySendRequest(originalRequest, oddRetryCount);

            }
            else
            {
                Debug.Log("handleRetryRequest send request is over");
                errCallBack(ResponseErroInfo.GetErrorInfo(response.StatusCode, response.DataAsText));
            }
        }

        public int getOddRetryCount(HTTPRequest originalRequest)
        {
            int oddRetryCount;
            try
            {
                oddRetryCount = (int)Convert.ChangeType(originalRequest.Tag, typeof(int));
            }
            catch
            {
                oddRetryCount = 0;
            }
            return oddRetryCount;
        }

        public void retrySendRequest(HTTPRequest originalRequest, int oddRetryCount)
        {
            originalRequest.Tag = oddRetryCount - 1;
            originalRequest.ConnectTimeout = TimeSpan.FromSeconds(20);
            originalRequest.Timeout = TimeSpan.FromSeconds(20);
            originalRequest.Send();
            // Timer t = new Timer((state) =>
            // {
            //     originalRequest.Send();
            //     Debug.Log("retryRequest:"+originalRequest.CurrentUri.ToString());
            // }, "", getRetryIntervalTime(oddRetryCount), Timeout.Infinite);
        }

        public int getRetryIntervalTime(int oddRetryCount)
        {
            return (RequestRetryInfo.allRetryCount - oddRetryCount + 1) * RequestRetryInfo.baseRetryIntervalTime + 20;
        }

        public void handleAuthenticatioError(HTTPRequest originalRequest, Action<ResponseErroInfo> errCallBack)
        {
            AuthenticationUtils.GetToken((scuccessrResultBack) =>
            {
                string token = scuccessrResultBack.info;
                reSendRequest(originalRequest, token);
            }, (errResultBack) =>
            {
                errCallBack(ResponseErroInfo.GetErrorInfo(401, "renew token is error"));
            });
        }

        public void reSendRequest(HTTPRequest originalRequest, string token)
        {
            if (originalRequest.HasHeader("token"))
            {
                originalRequest.SetHeader("token", token);
            }
            else
            {
                originalRequest.AddHeader("token", token);
            }
            originalRequest.Send();
        }



    }
}