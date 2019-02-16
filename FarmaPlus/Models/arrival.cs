using System;

namespace FarmaPlus.Models {

    public class arrival {

        public employee employee { get; set; }
        public TimeSpan entryTime { get; set; }
        public int appointments { get; set; }

    }

}