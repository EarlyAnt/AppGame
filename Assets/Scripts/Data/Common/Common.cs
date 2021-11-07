using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}
