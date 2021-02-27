using AppGame.Data.Common;
using AppGame.Data.Model;
using System;
using System.Collections.Generic;

namespace AppGame.Data.Remote
{
    public interface ICyclingDataUtil
    {
        void GetBasicData(Action<List<BasicData>> callback = null, Action<string> errCallback = null);

        void GetGameData(Action<List<PlayerData>> callback = null, Action<string> errCallback = null);

        void PutGameData(string childSN, PlayerData playerData, Action<Result> callback = null, Action<Result> errCallback = null);
    }

    public class GetBasicDataResponse : DataBase
    {
        public NetBasicData child { get; set; }
    }

    public class NetBasicData
    {
        public string child_sn { get; set; }
        public string nickname { get; set; }
        public string gender { get; set; }
        public string birthday { get; set; }
        public int avatar_index { get; set; }
    }

    public class GetGameDataResponse : DataBase
    {
        public List<NetPlayerData> data { get; set; }
    }

    public class NetPlayerData
    {
        
    }
}