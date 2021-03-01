using AppGame.Global;
using AppGame.Util;

namespace AppGame.Data.Local
{
    public class LocalChildInfoAgent : ILocalChildInfoAgent
    {
        [Inject]
        public ILocalDataManager LocalDataManager { get; set; }
        [Inject]
        public IJsonUtils JsonUtils { get; set; }
        private const string DATA_KEY = "user_child_sn";
        private const string DEFAULT_CHILD_SN = "gululu_2021";
        private string childSn;

        public void SaveChildSN(string currentChildSN)
        {
            this.childSn = currentChildSN;
            LocalDataManager.SaveObject<string>(DATA_KEY, currentChildSN);
            //Debug.LogFormat("<LocalChildInfoAgent.SaveChildSN>currentChildSN: {0}", currentChildSN);
        }

        public string GetChildSN()
        {
            //#if UNITY_EDITOR
            //            if (AppData.DebugMode)
            //                return "";

            //            this.SaveChildSN(DEFAULT_CHILD_SN);
            //#endif
            //            this.childSn = LocalDataManager.GetObject<string>(DATA_KEY, "");
            //            //Debug.LogFormat("<><LocalChildInfoAgent.GetChildSN>childSn: {0}", this.childSn);
            //            return childSn;
            return "BWR8ODUPYX3C";//¹þÃÜ¹Ï
        }

        public void Clear()
        {
            this.childSn = string.Empty;
        }
    }
}