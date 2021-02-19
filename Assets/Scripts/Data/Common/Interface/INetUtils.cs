using System.Collections.Generic;

namespace AppGame.Data.Common
{
    public interface INetUtils
    {
        bool isNetworkEnable();

        Dictionary<string,string> transforDictionarHead(string data);
    }
}