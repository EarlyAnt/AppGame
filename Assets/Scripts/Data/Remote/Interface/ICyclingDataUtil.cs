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

        void PostGameData(PlayerData playerData, Action<Result> callback = null, Action<Result> errCallback = null);
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
        public int relation { get; set; }
        public NetGameData gamedata { get; set; }

        public PlayerData ToPlayerData()
        {
            return new PlayerData()
            {
                child_sn = this.child_sn,
                child_name = this.nickname,
                child_avatar = this.avatar_index.ToString(),
                birthday = this.birthday,
                gender = this.gender,
                relation = this.relation,
                walk_expend = this.gamedata.walk_expend,
                walk_today = this.gamedata.walk_today,
                ride_expend = this.gamedata.ride_expend,
                train_expend = this.gamedata.train_expend,
                learn_expend = this.gamedata.learn_expend,
                mp = this.gamedata.mp,
                mp_expend = this.gamedata.mp_expend,
                mp_today = this.gamedata.mp_today,
                mp_date = this.gamedata.mp_date,
                mp_yestoday = this.gamedata.mp_yestoday,
                hp = this.gamedata.hp
            };
        }
    }

    public class NetGameData
    {
        public string child_sn { get; set; }
        public string child_name { get; set; }
        public string child_avatar { get; set; }
        public string gender { get; set; }
        public string birthday { get; set; }
        public int relation { get; set; }
        public string map_id { get; set; }
        public string map_position { get; set; }
        public int walk { get; set; }
        public int ride { get; set; }
        public int train { get; set; }
        public int monitor { get; set; }
        public int walk_expend { get; set; }
        public int walk_today { get; set; }
        public int ride_expend { get; set; }
        public int train_expend { get; set; }
        public int learn_expend { get; set; }
        public int mp { get; set; }
        public int mp_expend { get; set; }
        public int mp_today { get; set; }
        public DateTime mp_date { get; set; }
        public int mp_yestoday { get; set; }
        public int hp { get; set; }

        public override string ToString()
        {
            return string.Format("child_sn: {0}, map_id: {1}, map_position: {2}", this.child_sn, this.map_id, this.map_position);
        }
    }

    public class PostGameDataRequest
    {
        public NetGameData gamedata { get; set; }
    }
}