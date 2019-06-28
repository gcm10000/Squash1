using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSquash.InfoTrack
{
    public class InfoTrack
    {
        public string Track { get; set; }
        public int Duration { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public Image[] Images { get; set; }
    }
    public class Rootobject
    {
        public Track track { get; set; }
    }

    public class Track
    {
        public string name { get; set; }
        public string mbid { get; set; }
        public string url { get; set; }
        public string duration { get; set; }
        public Streamable streamable { get; set; }
        public string listeners { get; set; }
        public string playcount { get; set; }
        public Artist artist { get; set; }
        public Album album { get; set; }
        public Toptags toptags { get; set; }
        public Wiki wiki { get; set; }
    }

    public class Streamable
    {
        public string text { get; set; }
        public string fulltrack { get; set; }
    }

    public class Artist
    {
        public string name { get; set; }
        public string mbid { get; set; }
        public string url { get; set; }
    }

    public class Album
    {
        public string artist { get; set; }
        public string title { get; set; }
        public string mbid { get; set; }
        public string url { get; set; }
        public Image[] image { get; set; }
        public Attr attr { get; set; }
    }

    public class Attr
    {
        public string position { get; set; }
    }

    public class Image
    {
        public string text { get; set; }
        public string size { get; set; }
    }

    public class Toptags
    {
        public Tag[] tag { get; set; }
    }

    public class Tag
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Wiki
    {
        public string published { get; set; }
        public string summary { get; set; }
        public string content { get; set; }
    }

}
