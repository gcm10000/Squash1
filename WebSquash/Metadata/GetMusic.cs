using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSquash.Metadata
{
    public class GetMusic
    {
        public class Rootobject
        {
            public Array[] Array { get; set; }
        }

        public class Array
        {
            public string id { get; set; }
            public int duration { get; set; }
            public string tit_art { get; set; }
            public string url { get; set; }
            public string extra { get; set; }
        }

    }
}
