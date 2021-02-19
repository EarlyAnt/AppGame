using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGame.Data.Common
{
    public class Header
    {
        public List<HeaderData> headers { get; set; }
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

    public class TokenResponseData : DataBase
    {
        public string token;
    }
}
