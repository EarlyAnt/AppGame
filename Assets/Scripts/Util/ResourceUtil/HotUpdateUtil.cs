using System;
using UnityEngine;

namespace AppGame.Util
{
    //热更数据获取工具
    public class HotUpdateUtil : IHotUpdateUtil
    {
        //[Inject]
        //public IUrlProvider UrlProvider { get; set; }
        //[Inject]
        //public IJsonUtils JsonUtils { get; set; }
        //[Inject]
        //public ILocalCupAgent LocalCupAgent { get; set; }
        //[Inject]
        //public INativeOkHttpMethodWrapper mNativeOkHttpMethodWrapper { get; set; }
        //[Inject]
        //public GululuNetworkHelper GululuNetworkHelper { get; set; }
        private UpdateInfo newestVersion = null;
        private string fileName = string.Format("{0}/UpdateRecord.json", Application.persistentDataPath);

        //获取服务器资源更新数据
        public void GetUpdateInfo(Action<UpdateInfos> callBack = null, Action<string> errCallBack = null)
        {
            //Header header = new Header();
            //header.headers = new List<HeaderData>();
            //header.headers.Add(new HeaderData() { key = "Gululu-Agent", value = GululuNetworkHelper.GetAgent() });
            //header.headers.Add(new HeaderData() { key = "udid", value = GululuNetworkHelper.GetUdid() });
            //header.headers.Add(new HeaderData() { key = "Accept-Language", value = GululuNetworkHelper.GetAcceptLang() });
            //header.headers.Add(new HeaderData() { key = "Content-Type", value = "application/json" });
            //string strHeader = this.JsonUtils.Json2String(header);

            //UpdateRecord updateRecord = this.ReadUpdateRecord();//读取本地更新记录
            //UpdateRequest body = new UpdateRequest()
            //{
            //    partner = AppInfo.Instance.Channel,
            //    apk_ver_code = AppInfo.Instance.VersionCode,
            //    res_ver_code = updateRecord != null ? updateRecord.ResVersionCode : 0//如果本地更新记录为空，资源版本号默认为0
            //};
            //string strBody = this.JsonUtils.Json2String(body);

            //string url = this.UrlProvider.GetUpdateInfo(CupBuild.getCupSn());
            //Debug.LogFormat("<><HotUpdateUtils.GetUpdateInfo>Header: {0}, Body: {1}, Url: {2}", strHeader, strBody, url);
            //mNativeOkHttpMethodWrapper.post(url, strHeader, strBody, (result) =>
            //{
            //    Debug.LogFormat("<><HotUpdateUtils.GetUpdateInfo>GotResponse: {0}", result);
            //    UpdateInfos updateInfos = this.JsonUtils.String2Json<UpdateInfos>(result);
            //    if (updateInfos != null && callBack != null)
            //    {
            //        this.SetNewsestVersion(updateInfos);
            //        callBack(updateInfos);
            //    }
            //    else if (errCallBack != null)
            //    {
            //        string errorText = "Response data 'updateInfo' is null";
            //        Debug.LogErrorFormat("<><HotUpdateUtils.GetUpdateInfo>Invalid data error: {0}", errorText);
            //        errCallBack(errorText);
            //    }
            //}, (errorResult) =>
            //{
            //    if (errorResult != null && errCallBack != null)
            //    {
            //        Debug.LogErrorFormat("<><HotUpdateUtils.GetUpdateInfo>Status error: {0}", errorResult.ErrorInfo);
            //        errCallBack(errorResult.ErrorInfo);
            //    }
            //});
        }
        //读取本地资源更新记录
        public UpdateRecord ReadUpdateRecord()
        {
            UpdateRecord updateRecord = null;
            //if (!File.Exists(this.fileName))
            //{
            //    updateRecord = new UpdateRecord()
            //    {
            //        CupSN = CupBuild.getCupSn(),
            //        CupType = "Go2",
            //        TimeStamp = "",
            //        ApkVersion = AppInfo.Instance.Version,
            //        ApkVersionCode = AppInfo.Instance.VersionCode,
            //        ResVersionCode = 0,
            //        FileList = new List<LocalFileInfo>()
            //    };
            //}
            //else
            //{
            //    try
            //    {
            //        string content = "";
            //        using (FileStream fs = new FileStream(this.fileName, FileMode.Open, FileAccess.Read))
            //        {
            //            using (StreamReader sr = new StreamReader(fs))
            //            {
            //                content = sr.ReadToEnd();
            //                sr.Close();
            //            }
            //            fs.Close();
            //        }
            //        updateRecord = this.JsonUtils.String2Json<UpdateRecord>(content);
            //    }
            //    catch (Exception ex)
            //    {
            //        Debug.LogErrorFormat("<><HotUpdateUtils.ReadUpdateRecord>Error: {0}", ex.Message);
            //    }
            //}
            return updateRecord;
        }
        //保存本地资源更新记录
        public void SaveUpdateRecord(UpdateRecord updateRecord)
        {
            //if (updateRecord == null)
            //{
            //    Debug.LogError("<><HotUpdateUtils.SaveUpdateRecord>Paramter 'updateRecord' is null");
            //    return;
            //}
            //else if (this.newestVersion == null)
            //{
            //    Debug.LogWarning("<><HotUpdateUtils.SaveUpdateRecord>Server version info is null");
            //    return;
            //}

            //try
            //{
            //    //记录从服务器上获取到的最新的，并成功更新完毕的版本的版本号和时间戳
            //    updateRecord.ResVersionCode = this.newestVersion.res_ver_code;
            //    updateRecord.TimeStamp = this.newestVersion.create_time;
            //    //保存更新记录
            //    string content = this.JsonUtils.Json2String(updateRecord);
            //    using (FileStream fs = new FileStream(this.fileName, FileMode.OpenOrCreate, FileAccess.Write))
            //    {
            //        using (StreamWriter sw = new StreamWriter(fs))
            //        {
            //            sw.Write(content);
            //            sw.Close();
            //        }
            //        fs.Close();
            //    }
            //    Debug.Log("<><HotUpdateUtils.SaveUpdateRecord>OK + OK");
            //}
            //catch (Exception ex)
            //{
            //    Debug.LogErrorFormat("<><HotUpdateUtils.SaveUpdateRecord>Error: {0}", ex.Message);
            //}
        }
        //记录最新版本
        private void SetNewsestVersion(UpdateInfos updateInfos)
        {
            //if (updateInfos != null && updateInfos.res_list != null)
            //{
            //    List<UpdateInfo> updateInfoList = updateInfos.res_list.OrderByDescending(t => t.res_ver_code).ToList();//按照资源版本号倒叙排列历次更新数据
            //    foreach (var updateInfo in updateInfoList)
            //    {
            //        if (updateInfo.apk_ver_code <= AppInfo.Instance.VersionCode)
            //        {
            //            Debug.LogFormat("<><HotUpdateUtils.SetNewsestVersion>apk_ver: {0}, apk_ver_code: {1}, res_ver_code: {2}", updateInfo.apk_ver, updateInfo.apk_ver_code, updateInfo.res_ver_code);
            //            this.newestVersion = updateInfo;
            //            break;
            //        }
            //    }
            //}
        }
    }
}