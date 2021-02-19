using UnityEngine;

namespace AppGame.Data.Common
{
    public class ResponseErroInfo
    {
        public int ErrorCode { get; set; }
        public string ErrorInfo { get; set; }

        private ResponseErroInfo(int ErrorCode, string ErrorInfo)
        {
            this.ErrorCode = ErrorCode;
            this.ErrorInfo = ErrorInfo;

        }
        public static ResponseErroInfo GetErrorInfo(int ErrorCode, string ErrorInfo)
        {
            Debug.LogFormat("<ResponseErroInfo.GetErrorInfo>Error: {0}", ErrorInfo);
            return new ResponseErroInfo(ErrorCode, ErrorInfo);
        }
    }
}