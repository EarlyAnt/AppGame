using AppGame.Util;
using System;

namespace AppGame.Data.Local
{
    public interface IChildInfoManager
    {
        ILocalDataManager LocalDataManager { get; set; }
        IJsonUtil JsonUtils { get; set; }

        void SaveChildSN(string currentChildSN);
        string GetChildSN();
        void Clear();
    }
}