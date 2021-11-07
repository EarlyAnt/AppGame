namespace AppGame.Data.Local
{
    public class ChildInfoManager : IChildInfoManager
    {
        [Inject]
        public ILocalDataHelper LocalDataHelper { get; set; }
        private const string DATA_KEY = "user_child_sn";
        private const string DEFAULT_CHILD_SN = "gululu_2021";
        private string childSn;

        public void SaveChildSN(string currentChildSN)
        {
            this.childSn = currentChildSN;
            this.LocalDataHelper.SaveObject<string>(DATA_KEY, currentChildSN);
            //Debug.LogFormat("<ChildInfoManager.SaveChildSN>currentChildSN: {0}", currentChildSN);
        }

        public string GetChildSN()
        {
//#if UNITY_EDITOR
//            if (AppData.DebugMode)
//                return "";

//            this.SaveChildSN(DEFAULT_CHILD_SN);
//#endif
//            this.childSn = this.LocalDataHelper.GetObject<string>(DATA_KEY, "");
//            //Debug.LogFormat("<><ChildInfoManager.GetChildSN>childSn: {0}", this.childSn);
//            return childSn;
            return "BWR8ODUPYX3C";//哈密瓜
        }

        public void Clear()
        {
            this.childSn = string.Empty;
        }
    }
}