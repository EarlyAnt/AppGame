using System;
using System.Collections.Generic;
using System.Linq;

namespace AppGame.Data.Model
{
    public class BasicData
    {
        public string child_sn { get; set; }
        public string child_name { get; set; }
        public string child_avatar { get; set; }
    }

    public class OriginData
    {
        public string child_sn { get; set; }
        public int walk { get; set; }
        public int ride { get; set; }
        public int train { get; set; }
        public int learn { get; set; }
    }

    public class PlayerData
    {
        public string child_sn { get; set; }
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
        public int mp_yestoday { get; set; }
        public int hp { get; set; }
    }
}
