using AppGame.Data.Common;
using AppGame.Data.Local;
using AppGame.Data.Model;
using AppGame.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AppGame.Data.Remote
{
    //机器人对话云服务
    public class BotDialogDataUtil : IBotDialogDataUtil
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
            public Action<DialogResponse> OnSuccess { get; set; }//http请求成功时回调
            public Action<Result> OnFailed { get; set; }//http请求失败时回调
        }

        [Inject]
        public IUrlProvider UrlProvider { get; set; }
        [Inject]
        public IJsonUtil JsonUtil { get; set; }
        [Inject]
        public INativeOkHttpMethodWrapper NativeOkHttpMethodWrapper { get; set; }
        private List<ActionData> postGameDatas = new List<ActionData>();
        private ActionData lastPostGameData = null;

        //发送表情
        public void PostDialogData(DialogRequest dialogRequest, Action<DialogResponse> callback = null, Action<Result> errCallback = null)
        {
            Header header = new Header();
            header.headers = new List<HeaderData>();
            //header.headers.Add(new HeaderData() { key = "Gululu-Agent", value = GululuNetworkHelper.GetAgent() });
            //header.headers.Add(new HeaderData() { key = "udid", value = GululuNetworkHelper.GetUdid() });
            //header.headers.Add(new HeaderData() { key = "Accept-Language", value = GululuNetworkHelper.GetAcceptLang() });
            header.headers.Add(new HeaderData() { key = "Content-Type", value = "application/json" });
            string strHeader = this.JsonUtil.Json2String(header);

            string strBody = this.JsonUtil.Json2String(dialogRequest);
            Debug.Log("<><BotDialogDataUtil.PutGameData>Prepare data");

            ActionData newActionData = new ActionData()
            {
                Header = strHeader,
                Body = strBody,
                SendTimes = 3,
                OnSuccess = callback,
                OnFailed = errCallback
            };

            //检查队列里是否已经有数据
            if (this.lastPostGameData != null && this.lastPostGameData.Body == strBody)
            {//队列里有数据，只需要处理数据是否压栈
                Debug.LogFormat("<><BotDialogDataUtil.PutGameData>Ignore same data, Header: {0}, Body: {1}", strHeader, strBody);
                return;//如果新数据与最后一条数据的Body相同，直接忽略
            }
            else
            {//队列里没有数据，才需要数据压栈，且主动调用数据同步方法
                Debug.LogFormat("<><BotDialogDataUtil.PutGameData>Append new data and execute, Header: {0}, Body: {1}", strHeader, strBody);
                this.lastPostGameData = newActionData;
                this.postGameDatas.Add(newActionData);
                this.SendDialogData();
            }
        }
        //发送表情消息
        private void SendDialogData()
        {
            if (this.postGameDatas.Count > 0)
            {
                ActionData actionData = this.postGameDatas[0];
                Debug.LogFormat("<><BotDialogDataUtil.SendDialogData>Header: {0}, Body: {1}, SendTimes: {2}", actionData.Header, actionData.Body, actionData.SendTimes);
                Debug.LogFormat("<><BotDialogDataUtil.SendDialogData>Url: {0}", this.UrlProvider.PostDialogDataUrl());
                this.NativeOkHttpMethodWrapper.post(this.UrlProvider.PostDialogDataUrl(), actionData.Header, actionData.Body, (result) =>
                {
                    Debug.LogFormat("<><BotDialogDataUtil.SendDialogData>GotResponse: {0}", result);
                    DialogResponse dialogResponse = this.JsonUtil.String2Json<DialogResponse>(result);
                    if (actionData.OnSuccess != null) Loom.QueueOnMainThread(() => actionData.OnSuccess(dialogResponse));
                    Debug.LogFormat("<><BotDialogDataUtil.SendDialogData>Success:\n{0}", result);
                    //检查数据
                    if (this.postGameDatas.Count > 0)
                    {
                        this.lastPostGameData = null;
                        this.postGameDatas.RemoveAt(0);//移除已经执行成功的数据
                        if (this.postGameDatas.Count > 0)//执行下一条数据
                            this.SendDialogData();
                    }

                }, (errorResult) =>
                {
                    Debug.LogFormat("<><BotDialogDataUtil.SendDialogData>Error: {0}", errorResult.ErrorInfo);
                    if (actionData.OnFailed != null) Loom.QueueOnMainThread(() => actionData.OnFailed(Result.Error(errorResult.ErrorInfo)));
                    //检查数据
                    if (this.postGameDatas.Count > 0)
                    {
                        this.lastPostGameData = null;
                        if (this.postGameDatas[0].SendTimes > 0)
                        {//重复上传(最多3次)
                            this.postGameDatas[0].SendTimes -= 1;
                            Debug.LogFormat("<><BotDialogDataUtil.SendDialogData>Repeat, SendTimes: {0}, Body: {1}", actionData.SendTimes, actionData.Body);
                            this.SendDialogData();
                        }
                        else
                        {//3次重传失败放弃
                            this.postGameDatas.RemoveAt(0);
                            Debug.LogFormat("<><BotDialogDataUtil.SendDialogData>Abandon, SendTimes: {0}, Body: {1}", actionData.SendTimes, actionData.Body);
                        }
                    }
                });
            }
        }
    }
}