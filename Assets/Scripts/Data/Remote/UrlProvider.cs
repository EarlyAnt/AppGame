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

        public string GetTokenUrl(string child_sn)
        {
            string tokenUrl = ApiUrls.GET_TOKEN.Replace("<x_user_sn>", child_sn);
            return AddServerPrefix(tokenUrl);
        }

        public string GetRegisterUserUrl(string prod_name, string cup_hw_sn)
        {
            string registerUrl = ApiUrls.REGISTER_USER.Replace("<prod_name>", prod_name).Replace("<cup_hw_sn>", cup_hw_sn);

            return AddServerPrefix(registerUrl);
        }

        public string GetBasicDataUrl(string child_sn)
        {
            string getBasicDataUrl = ApiUrls.GET_BASIC_DATA.Replace("<child_sn>", child_sn);
            return AddServerPrefix(getBasicDataUrl);
        }

        public string GetGameDataUrl(string child_sn)
        {
            string registerUrl = ApiUrls.GET_GAME_DATA.Replace("<child_sn>", child_sn);
            return AddServerPrefix(registerUrl);
        }

        public string PutGameDataUrl(string child_sn)
        {
            string putGameDataUrl = ApiUrls.PUT_GAME_DATA.Replace("<child_sn>", child_sn);
            return AddServerPrefix(putGameDataUrl);
        }
    }

    public class ApiUrls
    {
        public const string BASE_URL = "http://api.gululu-a.com:9000/api/v3/m/";
        public const string LOG_URL = "http://api.gululu-a.com:9000/api/v3/cd/";
        public const string DEV_BASE_URL = "http://dev.mygululu.com:9000/api/v3/m/";
        public const string DEV2_BASE_URL = "http://dev2.mygululu.com:9000/api/v3/m/";

        public const string GET_TOKEN = "auth/<x_user_sn>/token";
        public const string REGISTER_USER = "auth/session";
        public const string GET_BASIC_DATA = "child/<child_sn>/info";
        public const string GET_GAME_DATA = "child/<child_sn>/friends_data";
        public const string PUT_GAME_DATA = "child/<child_sn>/gamedata";
    }
}
