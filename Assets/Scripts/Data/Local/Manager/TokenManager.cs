using AppGame.Data.Common;
using AppGame.Util;

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
#if UNITY_EDITOR
            return "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c24iOiJ1c184QUg4eWZZZUI1NWciLCJleHAiOjE2Mzg0MDYyNTZ9.IxFOH-lNJ0YHXpw1-iXzza97tjNF265fRIWO1Vz-fj8";
#else
            if (string.IsNullOrEmpty(this.token))
            {
                this.token = this.GameDataHelper.GetObject<string>(DATA_KEY, "");
            }
            return this.token;
#endif
        }
    }
}