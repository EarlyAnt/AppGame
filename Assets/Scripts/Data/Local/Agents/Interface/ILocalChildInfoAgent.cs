using AppGame.Util;
using System;

namespace AppGame.Data.Local
{
    public interface ILocalChildInfoAgent
    {
        ILocalDataManager LocalDataManager { get; set; }
        IJsonUtils JsonUtils { get; set; }

        void SaveChildSN(string currentChildSN);
        string GetChildSN();
        void Clear();
    }
}