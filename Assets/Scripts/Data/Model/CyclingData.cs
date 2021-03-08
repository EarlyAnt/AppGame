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
        public int learn { get; set; }

        public override string ToString()
        {
            return string.Format("child_sn: {0}, walk: {1}, ride: {2}, train: {3}, learn: {4}", this.child_sn, this.walk, this.ride, this.train, this.learn);
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
