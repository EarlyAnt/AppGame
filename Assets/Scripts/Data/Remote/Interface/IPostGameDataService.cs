using AppGame.Data.Common;
using System;

namespace AppGame.Data.Remote
{
    public interface IPostGameDataService
    {
        void PostGameData(string jsonString, Action<string> success, Action<ResponseErroInfo> failure);
    }
}
