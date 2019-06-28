using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSquash
{

    public class InfoSearchTrack
    {
        public string Track { get; set; }
        public string Artist { get; set; }
    }

    public class Rootobject
    {
        public Results results { get; set; }
    }

    public class Results
    {
        public OpensearchQuery opensearchQuery { get; set; }
        public string opensearchtotalResults { get; set; }
        public string opensearchstartIndex { get; set; }
        public string opensearchitemsPerPage { get; set; }
        public Trackmatches trackmatches { get; set; }
        public Attr attr { get; set; }
    }

    public class OpensearchQuery
    {
        public string text { get; set; }
        public string role { get; set; }
        public string startPage { get; set; }
    }

    public class Trackmatches
    {
        public Track[] track { get; set; }
    }

    public class Track
    {
        //se o requsição estiver contida no Name, logo é uma música
        //se o requsição estiver contida no Artista, logo é um artista
        //o que tiver mais repetições no Artista, será o artista principal na pesquisa
        //
        public string name { get; set; }
        public string artist { get; set; }
        public string url { get; set; }
        public string streamable { get; set; }
        public string listeners { get; set; }
        public Image[] image { get; set; }
        public string mbid { get; set; }
    }

    public class Image
    {
        public string text { get; set; }
        public string size { get; set; }
    }

    public class Attr
    {
    }

}
