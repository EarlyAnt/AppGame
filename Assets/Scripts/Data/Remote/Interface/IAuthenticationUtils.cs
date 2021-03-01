using AppGame.Data.Common;
using AppGame.Data.Local;
using AppGame.Util;
using System;

namespace AppGame.Data.Remote
{
    public interface IAuthenticationUtils
    {
        IUrlProvider UrlProvider { get; set; }
        IJsonUtils JsonUtils { get; set; }
        ILocalChildInfoAgent LocalChildInfoAgent { get; set; }
        void GetVerifyCode(string phone, Action<Result> callBack, Action<ResponseErroInfo> errCallBack);
        void Login(LoginData loginData, Action<Result> callBack, Action<ResponseErroInfo> errCallBack);
        void GetToken(Action<Result> callBack, Action<ResponseErroInfo> errCallBack);
    }

    public class LoginData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string VerifyCode { get; set; }
    }

    public class GetTokenResponseData : DataBase
    {
        public string token;
    }

    public class LoginResponseData : DataBase
    {
        public string token { get; set; }
        public string user_id { get; set; }
    }
}