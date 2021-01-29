using System;
using System.Collections.Generic;

namespace AppGame.Util
{
    public interface IHotUpdateUtils
    {
        void GetUpdateInfo(Action<UpdateInfos> callback = null, Action<string> errCallback = null);
        UpdateRecord ReadUpdateRecord();
        void SaveUpdateRecord(UpdateRecord updateRecord);
    }

    public class UpdateRequest
    {
        public string partner { get; set; }
        public int res_ver_code { get; set; }
        public int apk_ver_code { get; set; }
    }

    public class UpdateInfos
    {
        public string status { get; set; }
        public string url_prefix { get; set; }
        public List<UpdateInfo> res_list { get; set; }
    }

    public class UpdateInfo
    {
        public string create_time { get; set; }
        public string apk_ver { get; set; }
        public int apk_ver_code { get; set; }
        public int res_ver_code { get; set; }
        public List<UpdateFileInfo> update_files { get; set; }
        public override string ToString()
        {
            return string.Format("res_ver_code: {0}, apk_ver_code: {1}, apk_ver: {2}", this.res_ver_code, this.apk_ver_code, this.apk_ver);
        }
    }

    public class UpdateFileInfo
    {
        public string type { get; set; }
        public string file { get; set; }
        public string md5 { get; set; }
    }

    public class UpdateRecord
    {
        public string CupSN { get; set; }
        public string CupType { get; set; }
        public string TimeStamp { get; set; }
        public string ApkVersion { get; set; }
        public int ApkVersionCode { get; set; }
        public int ResVersionCode { get; set; }
        public List<LocalFileInfo> FileList { get; set; }
    }

    public class LocalFileInfo
    {
        public string File { get; set; }
        public string Type { get; set; }
        public string MD5 { get; set; }
    }
}