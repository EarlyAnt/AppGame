using System;
using AppGame.Data.Remote;
using AppGame.Util;
using BestHTTP;
using UnityEngine;

namespace AppGame.Data.Common
{
    public class NativeOkHttpMethodWrapper : INativeOkHttpMethodWrapper
    {
        [Inject]
        public IUrlProvider UrlProvider { get; set; }
        [Inject]
        public IGululuNetwork GululuNetwork { set; get; }
        [Inject]
        public INetUtils NetUtils { set; get; }
        private AndroidJavaObject nativeOkHttpMethodWrapper;

        public void post(string url, string headerStr, string body, Action<string> result, Action<ResponseErroInfo> faile)
        {
#if UNITY_ANDROID && (!UNITY_EDITOR)
            Debug.Log(string.Format("<><NativeOkHttpMethodWrapper.post>url: {0}, headerStr: {1}, body: {2}", url, headerStr, body));
            if (nativeOkHttpMethodWrapper == null)
            {
                nativeOkHttpMethodWrapper = getOkHttpMethodWrapper();
            }

            NativeOkHttpMethodWrapperCallBack callBack = new NativeOkHttpMethodWrapperCallBack();
            callBack.setCallback((resultInfo) =>
            {
                Debug.Log(string.Format("<><NativeOkHttpMethodWrapper.post>post back: {0}", resultInfo));
                result(resultInfo);
            }, (faileInfo) =>
            {
                Debug.Log(string.Format("<><NativeOkHttpMethodWrapper.post>post back error: {0}", faileInfo != null ? faileInfo.ErrorInfo : "empty or null"));
                faile(faileInfo);
            });
            nativeOkHttpMethodWrapper.Call("post", url, headerStr, body, callBack);

#elif UNITY_EDITOR
            GululuNetwork.SendRequest(url, NetUtils.transforDictionarHead(headerStr), body, (response) =>
            {
                result(response);
            }, faile, HTTPMethods.Post);
#endif
        }

        public void post2(string url, string headerStr, string body, Action<string> result, Action<ResponseErroInfo> faile)
        {
            GululuNetwork.SendRequest(url, NetUtils.transforDictionarHead(headerStr), body, (response) =>
            {
                result(response);
            }, faile, HTTPMethods.Post);
        }

        private AndroidJavaObject getOkHttpMethodWrapper()
        {
            AndroidJavaClass okHttpMethodWrapperBuilder = new AndroidJavaClass("com.bowhead.hank.network.OKHttpMethodWrapperBuilder");
            return okHttpMethodWrapperBuilder.CallStatic<AndroidJavaObject>("getOkHttpMethodWrapper", UrlProvider.GetCupTokenUrl(CupBuild.getCupSn()));
        }

        public void put(string url, string headerStr, string body, Action<string> result, Action<ResponseErroInfo> faile)
        {
#if UNITY_ANDROID && (!UNITY_EDITOR)
            Debug.Log(string.Format("<><NativeOkHttpMethodWrapper.put>url: {0}, headerStr: {1}, body: {2}", url, headerStr, body));
            if (nativeOkHttpMethodWrapper == null)
            {
                nativeOkHttpMethodWrapper = getOkHttpMethodWrapper();
            }
            NativeOkHttpMethodWrapperCallBack callBack = new NativeOkHttpMethodWrapperCallBack();
            callBack.setCallback((resultInfo) =>
            {
                Debug.Log(string.Format("<><NativeOkHttpMethodWrapper.put>put back: {0}", resultInfo));
                result(resultInfo);
            }, (faileInfo) =>
            {
                Debug.Log(string.Format("<><NativeOkHttpMethodWrapper.put>put back error: {0}", faileInfo != null ? faileInfo.ErrorInfo : "empty or null"));
                faile(faileInfo);
            });
            nativeOkHttpMethodWrapper.Call("put", url, headerStr, body, callBack);
#elif UNITY_EDITOR
            GululuNetwork.SendRequest(url, NetUtils.transforDictionarHead(headerStr), body, (response) =>
            {
                result(response);
            }, faile, HTTPMethods.Put);
#endif
        }

        public void get(string url, string headerStr, Action<string> result, Action<ResponseErroInfo> faile)
        {
#if UNITY_ANDROID && (!UNITY_EDITOR)
            Debug.Log("NativeOkHttpMethodWrapper get");
            if(nativeOkHttpMethodWrapper == null){
                nativeOkHttpMethodWrapper = getOkHttpMethodWrapper();
            }
            NativeOkHttpMethodWrapperCallBack callBack = new NativeOkHttpMethodWrapperCallBack();
            callBack.setCallback((resultInfo)=>{
                result(resultInfo);
            },(faileInfo)=>{
                faile(faileInfo);
            });
            nativeOkHttpMethodWrapper.Call("get",url,headerStr,callBack);
#elif UNITY_EDITOR
            GululuNetwork.SendRequest(url, NetUtils.transforDictionarHead(headerStr), (response) =>
            {
                result(response);
            }, faile, HTTPMethods.Get);
#endif
        }

        public void delete(string url, string headerStr, Action<string> result, Action<ResponseErroInfo> faile)
        {
#if UNITY_ANDROID && (!UNITY_EDITOR)
            Debug.Log("NativeOkHttpMethodWrapper delete");
            if(nativeOkHttpMethodWrapper == null){
                nativeOkHttpMethodWrapper = getOkHttpMethodWrapper();
            }
            NativeOkHttpMethodWrapperCallBack callBack = new NativeOkHttpMethodWrapperCallBack();
            callBack.setCallback((resultInfo)=>{
                result(resultInfo);
            },(faileInfo)=>{
                faile(faileInfo);
            });
            nativeOkHttpMethodWrapper.Call("delete",url,headerStr,callBack);
#elif UNITY_EDITOR
            GululuNetwork.SendRequest(url, NetUtils.transforDictionarHead(headerStr), (response) =>
            {
                result(response);
            }, faile, HTTPMethods.Delete);
#endif
        }

        public void delete(string url, string headerStr, string body, Action<string> result, Action<ResponseErroInfo> faile)
        {
#if UNITY_ANDROID && (!UNITY_EDITOR)
             Debug.Log("NativeOkHttpMethodWrapper delete");
            if(nativeOkHttpMethodWrapper == null){
                nativeOkHttpMethodWrapper = getOkHttpMethodWrapper();
            }
            NativeOkHttpMethodWrapperCallBack callBack = new NativeOkHttpMethodWrapperCallBack();
            callBack.setCallback((resultInfo)=>{
                result(resultInfo);
            },(faileInfo)=>{
                faile(faileInfo);
            });
            nativeOkHttpMethodWrapper.Call("delete",url,headerStr,body,callBack);
#elif UNITY_EDITOR
            GululuNetwork.SendRequest(url, NetUtils.transforDictionarHead(headerStr), body, (response) =>
            {
                result(response);
            }, faile, HTTPMethods.Delete);
#endif
        }
    }
}