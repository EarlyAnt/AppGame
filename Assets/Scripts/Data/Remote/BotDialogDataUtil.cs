using AppGame.Data.Common;
using AppGame.Data.Local;
using AppGame.Data.Model;
using AppGame.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AppGame.Data.Remote
{
    //�����˶Ի��Ʒ���
    public class BotDialogDataUtil : IBotDialogDataUtil
    {
        /// <summary>
        /// ͬ��������
        /// </summary>
        class ActionData
        {
            public DateTime RegisterTime { get; set; }//����ע��ʱ��
            public string Header { get; set; }
            public string Body { get; set; }
            public int SendTimes { get; set; }//���ʹ���(Ĭ��3�Σ�3��ʧ�ܺ󣬲����ط�)
            public Action<DialogResponse> OnSuccess { get; set; }//http����ɹ�ʱ�ص�
            public Action<Result> OnFailed { get; set; }//http����ʧ��ʱ�ص�
        }

        [Inject]
        public IUrlProvider UrlProvider { get; set; }
        [Inject]
        public IJsonUtil JsonUtil { get; set; }
        [Inject]
        public INativeOkHttpMethodWrapper NativeOkHttpMethodWrapper { get; set; }
        private List<ActionData> postGameDatas = new List<ActionData>();
        private ActionData lastPostGameData = null;

        //���ͱ���
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

            //���������Ƿ��Ѿ�������
            if (this.lastPostGameData != null && this.lastPostGameData.Body == strBody)
            {//�����������ݣ�ֻ��Ҫ���������Ƿ�ѹջ
                Debug.LogFormat("<><BotDialogDataUtil.PutGameData>Ignore same data, Header: {0}, Body: {1}", strHeader, strBody);
                return;//��������������һ�����ݵ�Body��ͬ��ֱ�Ӻ���
            }
            else
            {//������û�����ݣ�����Ҫ����ѹջ����������������ͬ������
                Debug.LogFormat("<><BotDialogDataUtil.PutGameData>Append new data and execute, Header: {0}, Body: {1}", strHeader, strBody);
                this.lastPostGameData = newActionData;
                this.postGameDatas.Add(newActionData);
                this.SendDialogData();
            }
        }
        //���ͱ�����Ϣ
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
                    //�������
                    if (this.postGameDatas.Count > 0)
                    {
                        this.lastPostGameData = null;
                        this.postGameDatas.RemoveAt(0);//�Ƴ��Ѿ�ִ�гɹ�������
                        if (this.postGameDatas.Count > 0)//ִ����һ������
                            this.SendDialogData();
                    }

                }, (errorResult) =>
                {
                    Debug.LogFormat("<><BotDialogDataUtil.SendDialogData>Error: {0}", errorResult.ErrorInfo);
                    if (actionData.OnFailed != null) Loom.QueueOnMainThread(() => actionData.OnFailed(Result.Error(errorResult.ErrorInfo)));
                    //�������
                    if (this.postGameDatas.Count > 0)
                    {
                        this.lastPostGameData = null;
                        if (this.postGameDatas[0].SendTimes > 0)
                        {//�ظ��ϴ�(���3��)
                            this.postGameDatas[0].SendTimes -= 1;
                            Debug.LogFormat("<><BotDialogDataUtil.SendDialogData>Repeat, SendTimes: {0}, Body: {1}", actionData.SendTimes, actionData.Body);
                            this.SendDialogData();
                        }
                        else
                        {//3���ش�ʧ�ܷ���
                            this.postGameDatas.RemoveAt(0);
                            Debug.LogFormat("<><BotDialogDataUtil.SendDialogData>Abandon, SendTimes: {0}, Body: {1}", actionData.SendTimes, actionData.Body);
                        }
                    }
                });
            }
        }
    }
}