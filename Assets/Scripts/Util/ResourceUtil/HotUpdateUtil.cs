using AppGame.Data.Common;
using AppGame.Data.Remote;
using AppGame.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AppGame.Util
{
    //�ȸ����ݻ�ȡ����
    public class HotUpdateUtil : IHotUpdateUtil
    {
        [Inject]
        public IUrlProvider UrlProvider { get; set; }
        [Inject]
        public IJsonUtil JsonUtil { get; set; }
        [Inject]
        public INativeOkHttpMethodWrapper NativeOkHttpMethodWrapper { get; set; }
        private UpdateInfo newestVersion = null;
        private string fileName = string.Format("{0}/UpdateRecord.json", Application.persistentDataPath);

        //��ȡ��������Դ��������
        public void GetUpdateInfo(Action<UpdateInfos> callBack = null, Action<string> errCallBack = null)
        {
            Header header = new Header();
            header.headers = new List<HeaderData>();
            header.headers.Add(new HeaderData() { key = "Gululu-Agent", value = GululuNetworkHelper.GetAgent() });
            header.headers.Add(new HeaderData() { key = "udid", value = GululuNetworkHelper.GetUdid() });
            header.headers.Add(new HeaderData() { key = "Accept-Language", value = GululuNetworkHelper.GetAcceptLang() });
            header.headers.Add(new HeaderData() { key = "Content-Type", value = "application/json" });
            string strHeader = this.JsonUtil.Json2String(header);

            UpdateRecord updateRecord = this.ReadUpdateRecord();//��ȡ���ظ��¼�¼
            UpdateRequest body = new UpdateRequest()
            {
                partner = AppData.Channel,
                apk_ver_code = AppData.VersionCode,
                res_ver_code = updateRecord != null ? updateRecord.ResVersionCode : 0//������ظ��¼�¼Ϊ�գ���Դ�汾��Ĭ��Ϊ0
            };
            string strBody = this.JsonUtil.Json2String(body);

            string url = this.UrlProvider.GetUpdateInfo(CupBuild.getCupSn());
            Debug.LogFormat("<><HotUpdateUtils.GetUpdateInfo>Header: {0}, Body: {1}, Url: {2}", strHeader, strBody, url);
            NativeOkHttpMethodWrapper.post(url, strHeader, strBody, (result) =>
            {
                Debug.LogFormat("<><HotUpdateUtils.GetUpdateInfo>GotResponse: {0}", result);
                UpdateInfos updateInfos = this.JsonUtil.String2Json<UpdateInfos>(result);
                if (updateInfos != null && callBack != null)
                {
                    this.SetNewsestVersion(updateInfos);
                    callBack(updateInfos);
                }
                else if (errCallBack != null)
                {
                    string errorText = "Response data 'updateInfo' is null";
                    Debug.LogErrorFormat("<><HotUpdateUtils.GetUpdateInfo>Invalid data error: {0}", errorText);
                    errCallBack(errorText);
                }
            }, (errorResult) =>
            {
                if (errorResult != null && errCallBack != null)
                {
                    Debug.LogErrorFormat("<><HotUpdateUtils.GetUpdateInfo>Status error: {0}", errorResult.ErrorInfo);
                    errCallBack(errorResult.ErrorInfo);
                }
            });
        }
        //��ȡ������Դ���¼�¼
        public UpdateRecord ReadUpdateRecord()
        {
            UpdateRecord updateRecord = null;
            if (!File.Exists(this.fileName))
            {
                updateRecord = new UpdateRecord()
                {
                    CupSN = CupBuild.getCupSn(),
                    CupType = "Go2",
                    TimeStamp = "",
                    ApkVersion = AppData.Version,
                    ApkVersionCode = AppData.VersionCode,
                    ResVersionCode = 0,
                    FileList = new List<LocalFileInfo>()
                };
            }
            else
            {
                try
                {
                    string content = "";
                    using (FileStream fs = new FileStream(this.fileName, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            content = sr.ReadToEnd();
                            sr.Close();
                        }
                        fs.Close();
                    }
                    updateRecord = this.JsonUtil.String2Json<UpdateRecord>(content);
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("<><HotUpdateUtils.ReadUpdateRecord>Error: {0}", ex.Message);
                }
            }
            return updateRecord;
        }
        //���汾����Դ���¼�¼
        public void SaveUpdateRecord(UpdateRecord updateRecord)
        {
            if (updateRecord == null)
            {
                Debug.LogError("<><HotUpdateUtils.SaveUpdateRecord>Paramter 'updateRecord' is null");
                return;
            }
            else if (this.newestVersion == null)
            {
                Debug.LogWarning("<><HotUpdateUtils.SaveUpdateRecord>Server version info is null");
                return;
            }

            try
            {
                //��¼�ӷ������ϻ�ȡ�������µģ����ɹ�������ϵİ汾�İ汾�ź�ʱ���
                updateRecord.ResVersionCode = this.newestVersion.res_ver_code;
                updateRecord.TimeStamp = this.newestVersion.create_time;
                //������¼�¼
                string content = this.JsonUtil.Json2String(updateRecord);
                using (FileStream fs = new FileStream(this.fileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(content);
                        sw.Close();
                    }
                    fs.Close();
                }
                Debug.Log("<><HotUpdateUtils.SaveUpdateRecord>OK + OK");
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("<><HotUpdateUtils.SaveUpdateRecord>Error: {0}", ex.Message);
            }
        }
        //��¼���°汾
        private void SetNewsestVersion(UpdateInfos updateInfos)
        {
            if (updateInfos != null && updateInfos.res_list != null)
            {
                List<UpdateInfo> updateInfoList = updateInfos.res_list.OrderByDescending(t => t.res_ver_code).ToList();//������Դ�汾�ŵ����������θ�������
                foreach (var updateInfo in updateInfoList)
                {
                    if (updateInfo.apk_ver_code <= AppData.VersionCode)
                    {
                        Debug.LogFormat("<><HotUpdateUtils.SetNewsestVersion>apk_ver: {0}, apk_ver_code: {1}, res_ver_code: {2}", updateInfo.apk_ver, updateInfo.apk_ver_code, updateInfo.res_ver_code);
                        this.newestVersion = updateInfo;
                        break;
                    }
                }
            }
        }
    }
}