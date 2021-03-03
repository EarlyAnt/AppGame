using AppGame.Data.Common;
using AppGame.Data.Model;
using System;
using System.Collections.Generic;

namespace AppGame.Data.Remote
{
    public interface ICyclingDataUtil
    {
        void GetBasicData(Action<BasicData> callback = null, Action<string> errCallback = null);

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

        public BasicData ToBasicData()
        {
            return new BasicData()
            {
                child_sn = this.child_sn,
                child_name = this.nickname,
                child_avatar = this.avatar_index.ToString(),
                gender = this.gender,
                birthday = this.birthday
            };
        }
    }

    public class GetGameDataResponse : DataBase
    {
        public List<NetPlayerData> data { get; set; }

        public List<PlayerData> ToPlayerDataList()
        {
            if (this.data != null)
            {
                List<PlayerData> playerDataList = new List<PlayerData>();
                foreach (var item in this.data)
                {
                    playerDataList.Add(item.ToPlayerData());
                }
                return playerDataList;
            }
            else return null;
        }
    }

    public class NetPlayerData
    {
        public string child_sn { get; set; }
        public string nickname { get; set; }
        public string gender { get; set; }
        public string birthday { get; set; }
        public int avatar_index { get; set; }

        public PlayerData ToPlayerData()
        {
            return new PlayerData()
            {
                child_sn = this.child_sn
            };
        }
    }
}