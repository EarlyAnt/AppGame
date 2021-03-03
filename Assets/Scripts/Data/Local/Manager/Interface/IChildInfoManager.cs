using AppGame.Util;
using System;

namespace AppGame.Data.Local
{
    public interface IChildInfoManager
    {
        void SaveChildSN(string currentChildSN);
        string GetChildSN();
        void Clear();
    }
}