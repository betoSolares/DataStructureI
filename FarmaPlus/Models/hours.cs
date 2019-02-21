using System;

namespace FarmaPlus.Models {

    public class hours {

        public employee employee { get; set; }
        public TimeSpan entryTime { get; set; }
        public TimeSpan appointmentsTime { get; set; }
        public int appointments { get; set; }
        public TimeSpan returnTime { get; set; }

    }

}