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

        public override string ToString()
        {
            return string.Format("child_sn: {0}, child_name: {1}, child_avatar: {2}", this.child_sn, this.child_name, this.child_avatar);
        }
    }
}
