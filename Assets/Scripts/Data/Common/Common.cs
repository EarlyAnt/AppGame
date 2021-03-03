﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AppGame.Data.Common
{
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
