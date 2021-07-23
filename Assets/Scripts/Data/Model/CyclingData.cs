using System;
using System.Collections.Generic;
using System.Linq;

namespace AppGame.Data.Model
{
    public class OriginData
    {
        public string child_sn { get; set; }
        public int walk { get; set; }
        public int ride { get; set; }
        public int train { get; set; }
        public int monitor { get; set; }

        public override string ToString()
        {
            return string.Format("child_sn: {0}, walk: {1}, ride: {2}, train: {3}, monitor: {4}", this.child_sn, this.walk, this.ride, this.train, this.monitor);
        }
    }

    public class PlayerData
    {
        public string child_sn { get; set; }
        public string child_name { get; set; }
        public string child_avatar { get; set; }
        public string gender { get; set; }
        public string birthday { get; set; }
        public int relation { get; set; }
        public string map_id { get; set; }
        public string map_position { get; set; }
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

        public Remote.PostGameDataRequest ToGameData()
        {
            Remote.NetGameData gameData = new Remote.NetGameData()
            {
                child_sn = this.child_sn,
                child_name = this.child_name,
                child_avatar = this.child_avatar,
                birthday = this.birthday,
                gender = this.gender,
                relation = this.relation,
                map_id = this.map_id,
                map_position = this.map_position,
                walk_expend = this.walk_expend,
                walk_today = this.walk_today,
                ride_expend = this.ride_expend,
                train_expend = this.train_expend,
                learn_expend = this.learn_expend,
                mp = this.mp,
                mp_expend = this.mp_expend,
                mp_today = this.mp_today,
                mp_date = this.mp_date,
                mp_yestoday = this.mp_yestoday,
                hp = this.hp
            };

            return new Remote.PostGameDataRequest() { gamedata = gameData };
        }

        public override string ToString()
        {
            return string.Format("child_sn: {0}, map_id: {1}, map_position: {2}", this.child_sn, this.map_id, this.map_position);
        }
    }

    public class MpData
    {
        public DateTime Date { get; set; }
        public int Mp { get; set; }
        public bool Uploaded { get; set; }
    }
}
