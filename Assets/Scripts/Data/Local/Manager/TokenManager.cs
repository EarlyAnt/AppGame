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
            if (string.IsNullOrEmpty(this.token))
            {
                this.token = this.GameDataHelper.GetObject<string>(DATA_KEY, "");
            }
            return this.token;
        }
    }
}