using AppGame.Config;
using AppGame.Global;
using AppGame.Util;
using Cup.Utils.android;
using UnityEngine;

namespace AppGame.Data.Common
{
    public class GululuNetworkHelper
    {
        public string GetAgent()
        {
            return "GululuCupHank/" + CupBuild.getAppVersionName() + ";" + "Unity/" + SystemInfo.operatingSystem + ";" + SystemInfo.deviceName + "/" + SystemInfo.deviceModel;
        }

        public string GetUdid()
        {
            return SystemInfo.deviceUniqueIdentifier;
        }

        public string GetAcceptLang()
        {
            return this.GetLanguageToAgentKey();
        }

        private string GetLanguageToAgentKey()
        {
            string key = "en";
            switch (AppData.Language.ToUpper())
            {
                case "EN":
                    key = "en";
                    break;
                case "CHS":
                    key = "zh-Hans";
                    break;
                case "CHT":
                    key = "zh-Hant";
                    break;
                case "JP":
                    key = "jp";
                    break;
                default:
                    key = "en";
                    break;
            }
            return key;
        }
    }
}