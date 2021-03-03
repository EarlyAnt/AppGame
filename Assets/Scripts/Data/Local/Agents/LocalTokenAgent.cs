using AppGame.Util;

namespace AppGame.Data.Local
{
    public class LocalTokenAgent : ILocalTokenAgent
    {
        [Inject]
        public ILocalDataManager LocalDataManager { set; get; }
        private const string DATA_KEY = "local_token";
        private string token;

        public void SaveToken(string token)
        {
            this.token = token;
            LocalDataManager.SaveObject<string>(DATA_KEY, token);
        }
        public string GetToken()
        {
            if (string.IsNullOrEmpty(this.token))
            {
                this.token = LocalDataManager.GetObject<string>(DATA_KEY, "");
            }
            return this.token;
        }
    }
}