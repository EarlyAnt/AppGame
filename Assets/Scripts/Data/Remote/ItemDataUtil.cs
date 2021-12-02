using AppGame.Data.Common;
using AppGame.Data.Local;
using AppGame.Data.Model;
using AppGame.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AppGame.Data.Remote
{
    //物品数据上传与下载
    public class ItemDataUtil : IItemDataUtil
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
        public IItemDataManager ItemDataManager { get; set; }
        [Inject]
        public INativeOkHttpMethodWrapper NativeOkHttpMethodWrapper { get; set; }
        private List<ActionData> putItemDatas = new List<ActionData>();
        private ActionData lastPutItemData = null;

        //获取物品数据
        public void GetItemData(Action<List<ItemData>> callback = null, Action<string> errCallback = null)
        {
            string url = this.UrlProvider.ItemDataUrl(this.ChildInfoManager.GetChildSN());
            Debug.LogFormat("<><ItemDataUtil.GetItemData>ChildSN: {0}, Url: {1}, token: {2}", this.ChildInfoManager.GetChildSN(), url, this.TokenManager.GetToken());

            Header header = new Header();
            header.headers.Add(new HeaderData() { key = "Authorization", value = this.TokenManager.GetToken() });
            string headerString = this.JsonUtil.Json2String(header);

            this.NativeOkHttpMethodWrapper.get(url, headerString, (result) =>
            {
                Debug.LogFormat("<><ItemDataUtil.GetItemData>Result: {0}", result);
                GetItemDataResponse response = this.JsonUtil.String2Json<GetItemDataResponse>(result);
                if (response != null && response.success)
                {
                    Debug.LogFormat("<><ItemDataUtil.GetItemData>Response data is valid: {0}", result);
                    List<ItemData> itemDataList = response.ToItemDataList();
                    this.ItemDataManager.SaveItemList(itemDataList);
                    if (callback != null) callback(itemDataList);
                }
            },
            (errorInfo) =>
            {
                Debug.LogErrorFormat("<><ItemDataUtil.GetItemData>Error: {0}", errorInfo.ErrorInfo);
                if (errCallback != null) errCallback(errorInfo.ErrorInfo);
            });
        }
        //上传物品数据
        public void PutItemData(List<ItemData> itemDataList, Action<Result> callback = null, Action<Result> errCallback = null)
        {
            Header header = new Header();
            header.headers = new List<HeaderData>();
            header.headers.Add(new HeaderData() { key = "Content-Type", value = "application/json" });
            header.headers.Add(new HeaderData() { key = "Authorization", value = this.TokenManager.GetToken() });
            string strHeader = this.JsonUtil.Json2String(header);

            PutItemDataRequest putItemDataRequest = new PutItemDataRequest(itemDataList);
            string strBody = this.JsonUtil.Json2String(putItemDataRequest.ToNetItemDataList());
            Debug.Log("<><ItemDataUtil.PutItemData>Prepare data");

            ActionData newActionData = new ActionData()
            {
                Header = strHeader,
                Body = strBody,
                SendTimes = 3,
                OnSuccess = callback,
                OnFailed = errCallback
            };

            //检查队列里是否已经有数据
            if (this.lastPutItemData != null && this.lastPutItemData.Body == strBody)
            {//队列里有数据，只需要处理数据是否压栈
                Debug.LogFormat("<><ItemDataUtil.PutItemData>Ignore same data, Header: {0}, Body: {1}", strHeader, strBody);
                return;//如果新数据与最后一条数据的Body相同，直接忽略
            }
            else
            {//队列里没有数据，才需要数据压栈，且主动调用数据同步方法
                Debug.LogFormat("<><ItemDataUtil.PutItemData>Append new data and execute, Header: {0}, Body: {1}", strHeader, strBody);
                this.lastPutItemData = newActionData;
                this.putItemDatas.Add(newActionData);
                this.SendGameData();
            }
        }
        //发送物品消息
        private void SendGameData()
        {
            if (this.putItemDatas.Count > 0)
            {
                ActionData actionData = this.putItemDatas[0];
                Debug.LogFormat("<><ItemDataUtil.SendGameData>Header: {0}, Body: {1}, SendTimes: {2}", actionData.Header, actionData.Body, actionData.SendTimes);
                Debug.LogFormat("<><ItemDataUtil.SendGameData>Url: {0}", this.UrlProvider.PutGameDataUrl(this.ChildInfoManager.GetChildSN()));
                this.NativeOkHttpMethodWrapper.put(this.UrlProvider.ItemDataUrl(this.ChildInfoManager.GetChildSN()), actionData.Header, actionData.Body, (result) =>
                {
                    Debug.LogFormat("<><ItemDataUtil.SendGameData>GotResponse: {0}", result);
                    DataBase dataBase = this.JsonUtil.String2Json<DataBase>(result);
                    if (actionData.OnSuccess != null) Loom.QueueOnMainThread(() => actionData.OnSuccess(Result.Success(result)));
                    Debug.LogFormat("<><ItemDataUtil.SendGameData>Success:\n{0}", result);
                    //检查数据
                    if (this.putItemDatas.Count > 0)
                    {
                        this.lastPutItemData = null;
                        this.putItemDatas.RemoveAt(0);//移除已经执行成功的数据
                        if (this.putItemDatas.Count > 0)//执行下一条数据
                            this.SendGameData();
                    }

                }, (errorResult) =>
                {
                    Debug.LogFormat("<><ItemDataUtil.SendGameData>Error: {0}", errorResult.ErrorInfo);
                    if (actionData.OnFailed != null) Loom.QueueOnMainThread(() => actionData.OnFailed(Result.Error(errorResult.ErrorInfo)));
                    //检查数据
                    if (this.putItemDatas.Count > 0)
                    {
                        this.lastPutItemData = null;
                        if (this.putItemDatas[0].SendTimes > 0)
                        {//重复上传(最多3次)
                            this.putItemDatas[0].SendTimes -= 1;
                            Debug.LogFormat("<><ItemDataUtil.SendGameData>Repeat, SendTimes: {0}, Body: {1}", actionData.SendTimes, actionData.Body);
                            this.SendGameData();
                        }
                        else
                        {//3次重传失败放弃
                            this.putItemDatas.RemoveAt(0);
                            Debug.LogFormat("<><ItemDataUtil.SendGameData>Abandon, SendTimes: {0}, Body: {1}", actionData.SendTimes, actionData.Body);
                        }
                    }
                });
            }
        }
    }
}