using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebSquash.Controllers
{
    public class PlayController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Music(string artist, string album, string track)
        {
            Uri url = new Uri("http://ws.audioscrobbler.com/2.0/").
            AddQuery("method", "album.getinfo").
            AddQuery("api_key", "ae35bc3b28eb6c8460ed87e3749d354b").
            AddQuery("artist", artist).
            AddQuery("album", album).
            AddQuery("format", "json");

            string getinfo = SearchMusicController.GetResult(url);
            getinfo = getinfo.Replace("#text", "text");

            InfoAlbum.Rootobject infoAlbum = JsonConvert.DeserializeObject<InfoAlbum.Rootobject>(getinfo);
            ViewData["Album"] = infoAlbum.album.name;
            ViewData["Artist"] = infoAlbum.album.artist;
            ViewData["Image"] = infoAlbum.album.image.Single(x => x.size == "extralarge").text;
            ViewData["Tracks"] = infoAlbum.album.tracks.track;
            return View("Index");
        }
    }
}