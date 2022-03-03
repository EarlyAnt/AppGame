using System.Collections.Generic;
using UnityEngine;

namespace AppGame.Data.Common
{
    public class DontDeleteDataItem
    {
        public string Key { get; set; }
        public string ItemInfo { get; set; }
    }

    public class Header
    {
        public List<HeaderData> headers { get; set; }
        public Header() { this.headers = new List<HeaderData>(); }
    }

    public class HeaderData
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class DataBase
    {
        public string status { get; set; }

        public int code { get; set; }
        public string msg { get; set; }
        public bool success { get; set; }
    }

    public static class SpineColors
    {
        public static Color TRANSPARENT = new Color(0, 0, 0, 0);
        public static Color NORMAL = new Color(1, 1, 1, 1);
    }
}
