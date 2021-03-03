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
        public string gender { get; set; }
        public string birthday { get; set; }
        public int relation { get; set; }

        public override string ToString()
        {
            return string.Format("child_sn: {0}, child_name: {1}, child_avatar: {2}, relation: {3}", this.child_sn, this.child_name, this.child_avatar, this.relation);
        }
    }

    public enum Relations
    {
        Self = 0,
        Family = 1,
        Friend = 2
    }
}
