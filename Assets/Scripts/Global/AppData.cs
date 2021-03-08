using AppGame.Config;

namespace AppGame.Global
{
    public enum VersionTypes
    {
        Release,
        Staging,
        Development
    }

    public static class AppData
    {
        public static string GameName { get { return "Galaxy"; } }

        public static string Language = LanConfig.Languages.ChineseSimplified;
        public static string UserID { get; set; }

        public static string Channel { get { return "GALAXY"; } }
        public static string Version { get; set; }
        public static int VersionCode { get; set; }
        public static VersionTypes VersionType
        {
            get
            {
                return VersionTypes.Release;

                //#if (UNITY_ANDROID) && (!UNITY_EDITOR)
                //                return VersionTypes.Release;
                //#else
                //                return VersionTypes.Development;
                //#endif
            }
        }
        public static bool DebugMode { get; set; }
    }
}
