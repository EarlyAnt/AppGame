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

        public void post(string url, string headerStr, string body, Action<string> result, Action<ResponseErroInfo> faile)
        {
#if UNITY_ANDROID && (!UNITY_EDITOR)
            Debug.Log(string.Format("<><GululuNetwork.post>url: {0}, headerStr: {1}, body: {2}", url, headerStr, body));
            GululuNetwork.SendRequest(url, NetUtils.transforDictionarHead(headerStr), body, (response) =>
            {
                result(response);
            }, faile, HTTPMethods.Post);

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
            Debug.Log(string.Format("<><GululuNetwork.put>url: {0}, headerStr: {1}, body: {2}", url, headerStr, body));
            GululuNetwork.SendRequest(url, NetUtils.transforDictionarHead(headerStr), body, (response) =>
            {
                result(response);
            }, faile, HTTPMethods.Put);
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
            Debug.Log("GululuNetwork get");
            GululuNetwork.SendRequest(url, NetUtils.transforDictionarHead(headerStr), (response) =>
            {
                result(response);
            }, faile, HTTPMethods.Get);
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
            Debug.Log("GululuNetwork delete");
            GululuNetwork.SendRequest(url, NetUtils.transforDictionarHead(headerStr), (response) =>
            {
                result(response);
            }, faile, HTTPMethods.Delete);
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
            Debug.Log("GululuNetwork delete");
            GululuNetwork.SendRequest(url, NetUtils.transforDictionarHead(headerStr), body, (response) =>
            {
                result(response);
            }, faile, HTTPMethods.Delete);
#elif UNITY_EDITOR
            GululuNetwork.SendRequest(url, NetUtils.transforDictionarHead(headerStr), body, (response) =>
            {
                result(response);
            }, faile, HTTPMethods.Delete);
#endif
        }
    }
}