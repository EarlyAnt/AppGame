using AppGame.Util;
using System.Collections.Generic;
using UnityEngine;

namespace AppGame.Data.Common
{
    public class NetUtils : INetUtils
    {
        [Inject]
        public IJsonUtils mJsonUtils { get; set; }

        public bool isNetworkEnable()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        public Dictionary<string, string> transforDictionarHead(string data)
        {
            Dictionary<string, string> headDic = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(data))
            {
                return headDic;
            }

            Header header = mJsonUtils.String2Json<Header>(data);
            List<HeaderData> headerdata = header.headers;
            foreach (HeaderData headerData in headerdata)
            {
                headDic.Add(headerData.key, headerData.value);
            }
            return headDic;
        }
    }
}