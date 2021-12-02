using AppGame.Data.Common;
using AppGame.Data.Local;
using AppGame.Data.Model;
using AppGame.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AppGame.Data.Remote
{
    //骑行数据上传与下载
    public class CyclingDataUtil : ICyclingDataUtil
    {
        /// <summary>
        /// 同步数据类
        /// </summary>
        class ActionData
        {
            public DateTime RegisterTime { get; set; }//数据注册时间
            public string Header { get; set; }
            public string Body { get; set; }
            public int SendTimes { get; set; }//发送次数(默认3次，3次失败后，不再重发)
            public Action<Result> OnSuccess { get; set; }//http请求成功时回调
            public Action<Result> OnFailed { get; set; }//http请求失败时回调
        }

        [Inject]
        public IUrlProvider UrlProvider { get; set; }
        [Inject]
        public IJsonUtil JsonUtil { get; set; }
        [Inject]
        public IChildInfoManager ChildInfoManager { get; set; }
        [Inject]
        public ITokenManager TokenManager { get; set; }
        [Inject]
        public IBasicDataManager BasicDataManager { get; set; }
        [Inject]
        public ICyclingDataManager CyclingDataManager { get; set; }
        [Inject]
        public INativeOkHttpMethodWrapper NativeOkHttpMethodWrapper { get; set; }
        private List<ActionData> putGameDatas = new List<ActionData>();
        private ActionData lastPutGameData = null;

        //获取孩子基本数据
        public void GetBasicData(Action<BasicData> callback = null, Action<string> errCallback = null)
        {
            string url = this.UrlProvider.GetBasicDataUrl(this.ChildInfoManager.GetChildSN());
            Debug.LogFormat("<><CyclingDataUtil.GetBasicData>ChildSN: {0}, Url: {1}, token: {2}", this.ChildInfoManager.GetChildSN(), url, this.TokenManager.GetToken());

            Header header = new Header();
            header.headers.Add(new HeaderData() { key = "Authorization", value = this.TokenManager.GetToken() });
            string headerString = this.JsonUtil.Json2String(header);

            this.NativeOkHttpMethodWrapper.get(url, headerString, (result) =>
            {
                Debug.LogFormat("<><CyclingDataUtil.GetBasicData>Result: {0}", result);
                GetBasicDataResponse response = this.JsonUtil.String2Json<GetBasicDataResponse>(result);
                if (response != null && response.success)
                {
                    Debug.LogFormat("<><CyclingDataUtil.GetBasicData>Response data is valid: {0}", result);
                    BasicData basicData = response.data.ToBasicData();
                    this.BasicDataManager.SaveData(basicData);
                    if (callback != null) callback(basicData);
                }
            },
            (errorInfo) =>
            {
                Debug.LogErrorFormat("<><CyclingDataUtil.GetBasicData>Error: {0}", errorInfo.ErrorInfo);
                if (errCallback != null) errCallback(errorInfo.ErrorInfo);
            });
        }
        //获取健康数据(原始数据)
        public void GetOriginData(Action<OriginData> callback = null, Action<string> errCallback = null)
        {
            string url = this.UrlProvider.GetOriginDataUrl(this.ChildInfoManager.GetChildSN(), "cycling");
            Debug.LogFormat("<><CyclingDataUtil.GetOriginData>ChildSN: {0}, Url: {1}, token: {2}", this.ChildInfoManager.GetChildSN(), url, this.TokenManager.GetToken());

            Header header = new Header();
            header.headers.Add(new HeaderData() { key = "Authorization", value = this.TokenManager.GetToken() });
            string headerString = this.JsonUtil.Json2String(header);

            this.NativeOkHttpMethodWrapper.get(url, headerString, (result) =>
            {
                Debug.LogFormat("<><CyclingDataUtil.GetOriginData>Result: {0}", result);
                GetOriginDataResponse response = this.JsonUtil.String2Json<GetOriginDataResponse>(result);
                if (response != null && response.success)
                {
                    OriginData newOriginData = response.data.ToOriginData();
                    OriginData curOriginData = this.CyclingDataManager.GetOriginData(this.ChildInfoManager.GetChildSN());
                    if (curOriginData == null)
                    {
                        curOriginData = newOriginData;
                    }
                    else
                    {
                        curOriginData.ride = newOriginData.ride;
                    }

                    this.CyclingDataManager.SaveOriginData(curOriginData);
                    if (callback != null) callback(curOriginData);
                }
            },
            (errorInfo) =>
            {
                Debug.LogErrorFormat("<><CyclingDataUtil.GetOriginData>Error: {0}", errorInfo.ErrorInfo);
                if (errCallback != null) errCallback(errorInfo.ErrorInfo);
            });
        }
        //获取游戏数据
        public void GetGameData(Action<List<PlayerData>> callback = null, Action<string> errCallback = null)
        {
            string url = this.UrlProvider.GetGameDataUrl(this.ChildInfoManager.GetChildSN());
            Debug.LogFormat("<><CyclingDataUtil.GetGameData>ChildSN: {0}, Url: {1}, token: {2}", this.ChildInfoManager.GetChildSN(), url, this.TokenManager.GetToken());

            Header header = new Header();
            header.headers.Add(new HeaderData() { key = "Authorization", value = this.TokenManager.GetToken() });
            string headerString = this.JsonUtil.Json2String(header);

            this.NativeOkHttpMethodWrapper.get(url, headerString, (result) =>
            {
                Debug.LogFormat("<><CyclingDataUtil.GetGameData>Result: {0}", result);
                GetGameDataResponse response = this.JsonUtil.String2Json<GetGameDataResponse>(result);
                if (response != null && response.success)
                {
                    Debug.LogFormat("<><CyclingDataUtil.GetGameData>Response data is valid: {0}", result);
                    List<PlayerData> playerDataList = response.ToPlayerDataList();
                    this.CyclingDataManager.SavePlayerDataList(playerDataList);
                    if (callback != null) callback(playerDataList);
                }
            },
            (errorInfo) =>
            {
                Debug.LogErrorFormat("<><CyclingDataUtil.GetGameData>Error: {0}", errorInfo.ErrorInfo);
                if (errCallback != null) errCallback(errorInfo.ErrorInfo);
            });
        }
        //发送游戏数据
        public void PutGameData(PlayerData playerData, Action<Result> callback = null, Action<Result> errCallback = null)
        {
            Header header = new Header();
            header.headers = new List<HeaderData>();
            header.headers.Add(new HeaderData() { key = "Content-Type", value = "application/json" });
            header.headers.Add(new HeaderData() { key = "Authorization", value = this.TokenManager.GetToken() });
            string strHeader = this.JsonUtil.Json2String(header);

            PutGameDataRequest putGameDataRequest = playerData.ToGameData();
            string strBody = this.JsonUtil.Json2String(putGameDataRequest);
            Debug.Log("<><CyclingDataUtil.PutGameData>Prepare data");

            ActionData newActionData = new ActionData()
            {
                Header = strHeader,
                Body = strBody,
                SendTimes = 3,
                OnSuccess = callback,
                OnFailed = errCallback
            };

            //检查队列里是否已经有数据
            if (this.lastPutGameData != null && this.lastPutGameData.Body == strBody)
            {//队列里有数据，只需要处理数据是否压栈
                Debug.LogFormat("<><CyclingDataUtil.PutGameData>Ignore same data, Header: {0}, Body: {1}", strHeader, strBody);
                return;//如果新数据与最后一条数据的Body相同，直接忽略
            }
            else
            {//队列里没有数据，才需要数据压栈，且主动调用数据同步方法
                Debug.LogFormat("<><CyclingDataUtil.PutGameData>Append new data and execute, Header: {0}, Body: {1}", strHeader, strBody);
                this.lastPutGameData = newActionData;
                this.putGameDatas.Add(newActionData);
                this.SendGameData();
            }
        }
        //发送游戏数据
        private void SendGameData()
        {
            if (this.putGameDatas.Count > 0)
            {
                ActionData actionData = this.putGameDatas[0];
                Debug.LogFormat("<><CyclingDataUtil.SendGameData>Header: {0}, Body: {1}, SendTimes: {2}", actionData.Header, actionData.Body, actionData.SendTimes);
                Debug.LogFormat("<><CyclingDataUtil.SendGameData>Url: {0}", this.UrlProvider.PutGameDataUrl(this.ChildInfoManager.GetChildSN()));
                this.NativeOkHttpMethodWrapper.put(this.UrlProvider.PutGameDataUrl(this.ChildInfoManager.GetChildSN()), actionData.Header, actionData.Body, (result) =>
                {
                    Debug.LogFormat("<><CyclingDataUtil.SendGameData>GotResponse: {0}", result);
                    DataBase dataBase = this.JsonUtil.String2Json<DataBase>(result);
                    if (actionData.OnSuccess != null) Loom.QueueOnMainThread(() => actionData.OnSuccess(Result.Success(result)));
                    Debug.LogFormat("<><CyclingDataUtil.SendGameData>Success:\n{0}", result);
                    //检查数据
                    if (this.putGameDatas.Count > 0)
                    {
                        this.lastPutGameData = null;
                        this.putGameDatas.RemoveAt(0);//移除已经执行成功的数据
                        if (this.putGameDatas.Count > 0)//执行下一条数据
                            this.SendGameData();
                    }

                }, (errorResult) =>
                {
                    Debug.LogFormat("<><CyclingDataUtil.SendGameData>Error: {0}", errorResult.ErrorInfo);
                    if (actionData.OnFailed != null) Loom.QueueOnMainThread(() => actionData.OnFailed(Result.Error(errorResult.ErrorInfo)));
                    //检查数据
                    if (this.putGameDatas.Count > 0)
                    {
                        this.lastPutGameData = null;
                        if (this.putGameDatas[0].SendTimes > 0)
                        {//重复上传(最多3次)
                            this.putGameDatas[0].SendTimes -= 1;
                            Debug.LogFormat("<><CyclingDataUtil.SendGameData>Repeat, SendTimes: {0}, Body: {1}", actionData.SendTimes, actionData.Body);
                            this.SendGameData();
                        }
                        else
                        {//3次重传失败放弃
                            this.putGameDatas.RemoveAt(0);
                            Debug.LogFormat("<><CyclingDataUtil.SendGameData>Abandon, SendTimes: {0}, Body: {1}", actionData.SendTimes, actionData.Body);
                        }
                    }
                });
            }
        }
    }
}