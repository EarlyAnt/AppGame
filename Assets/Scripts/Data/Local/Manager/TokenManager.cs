using AppGame.Data.Common;
using AppGame.Util;
using UnityEngine;

namespace AppGame.Data.Local
{
    public class TokenManager : ITokenManager
    {
        [Inject]
        public IGameDataHelper GameDataHelper { set; get; }
        private const string DATA_KEY = "local_token";
        private string token;

        public void SaveToken(string token)
        {
            this.token = token;
            this.GameDataHelper.SaveObject<string>(DATA_KEY, token);
        }
        public string GetToken()
        {
            //#if UNITY_EDITOR
            return "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c24iOiJ1c184QUg4eWZZZUI1NWciLCJleHAiOjE2Mzk1NTUzMjV9.X5bli7dCheHsPHSm7Ex-2LGyOy7XJyuUvbs1Qzz1wsk";
            //#else
            //            if (string.IsNullOrEmpty(this.token))
            //            {
            //                this.token = this.GameDataHelper.GetObject<string>(DATA_KEY, "");
            //            }
            //            //Debug.LogFormat("<><TokenManager.GetToken>token: {0}", this.token);
            //            return this.token;
            //#endif
        }
    }
}