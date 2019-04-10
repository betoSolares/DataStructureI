using System.Collections.Generic;

namespace Health.Models {

    public class Orders {

        public string name { get; set; }
        public string address { get; set; }
        public string nit { get; set; }
        public List<Meds> products { get; set; }

    } 

}