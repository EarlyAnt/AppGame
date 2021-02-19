using AppGame.Util;

namespace AppGame.Data.Local
{
    public class LocalCupAgent : ILocalCupAgent
    {
        [Inject]
        public ILocalDataManager LocalDataManager { set; get; }
        private string cupToken;
        private string cupID;

        public void SaveCupToken(string cupSN, string token)
        {
            this.cupToken = token;
            LocalDataManager.SaveObject<string>(GetTokenDataKey(cupSN), token);
        }
        public string GetCupToken(string cupSN)
        {
            if (this.cupToken == null || this.cupToken == "")
            {
                this.cupToken = LocalDataManager.GetObject<string>(GetTokenDataKey(cupSN), "");
            }
            return this.cupToken;
        }
        public string GetCupToken()
        {
            return this.GetCupToken(CupBuild.getCupSn());
        }
        private string GetTokenDataKey(string cupSN)
        {
            return cupSN + "LOCAL_CUP_TOKEN";
        }

        public void SaveCupID(string cupSN, string cupID)
        {
            this.cupID = cupID;
            LocalDataManager.SaveObject<string>(GetCupDataKey(cupSN), cupID);
        }
        public string GetCupID(string cupSN)
        {
            if (this.cupID == null || this.cupID == "")
            {
                this.cupID = LocalDataManager.GetObject<string>(GetCupDataKey(cupSN), "");
            }
            return this.cupID;
        }
        private string GetCupDataKey(string cupSN)
        {
            return cupSN + "LOCAL_CUP_ID";
        }
    }
}