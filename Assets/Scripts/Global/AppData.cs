using AppGame.Config;

namespace AppGame.Global
{
    public static class AppData
    {
        public static string Language = LanConfig.Languages.ChineseSimplified;
        public static string UserID { get; set; }

        public static string Version { get; set; }
        public static int VersionCode { get; set; }
    }
}
