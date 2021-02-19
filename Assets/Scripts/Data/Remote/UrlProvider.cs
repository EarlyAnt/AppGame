using AppGame.Global;

namespace AppGame.Data.Remote
{
    public class UrlProvider : IUrlProvider
    {
        private string urlPrefix = ApiUrls.BASE_URL;

        public UrlProvider()
        {
            switch (AppData.VersionType)
            {
                case VersionTypes.Release:
                    urlPrefix = ApiUrls.BASE_URL;
                    break;
                case VersionTypes.Development:
                    urlPrefix = ApiUrls.DEV_BASE_URL;
                    break;
            }
        }

        private string AddServerPrefix(string targetUrl)
        {
            return urlPrefix + targetUrl;
        }

        public string GetCupTokenUrl(string cup_hw_sn)
        {
            string tokenUrl = ApiUrls.CUP_TOKEN.Replace("<cup_hw_sn>", cup_hw_sn);
            return AddServerPrefix(tokenUrl);
        }

        public string GetRegisterUrl(string prod_name, string cup_hw_sn)
        {
            string registerUrl = ApiUrls.CUP_REGISTER.Replace("<prod_name>", prod_name).Replace("<cup_hw_sn>", cup_hw_sn);

            return AddServerPrefix(registerUrl);
        }

        public string GetGameDataUrl(string child_sn, string game_name, string cup_hw_sn)
        {
            string registerUrl = ApiUrls.CUP_GAMEDATA.Replace("<x_child_sn>", child_sn).Replace("<game_name>", game_name).Replace("<cup_hw_sn>", cup_hw_sn);

            return AddServerPrefix(registerUrl);
        }
    }

    public class ApiUrls
    {
        public const string BASE_URL = "http://api.gululu-a.com:9000/api/v5/c/";
        public const string LOG_URL = "http://api.gululu-a.com:9000/api/v5/cd/";
        public const string DEV_BASE_URL = "http://dev.mygululu.com:9000/api/v5/c/";
        public const string DEV2_BASE_URL = "http://dev2.mygululu.com:9000/api/v5/c/";

        public const string CUP_TOKEN = "cup/<cup_hw_sn>/token";
        public const string CUP_REGISTER = "prod/<prod_name>/cup/<cup_hw_sn>";
        public const string CUP_GAMEDATA = "child/<x_child_sn>/cup/<cup_hw_sn>/game/<game_name>/game_data";
    }
}
