using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebSquash.Controllers
{
    public class RedirectController : Controller
    {
        // GET: Redirect
        public ActionResult Redirect(string artist, string track)
        {
            if ((artist == null) || (track == null))
            {
                return NotFound("Insert artist and track for redirection work.");
            }
            try
            {
                var infoTrack = SearchMusicController.MGetInfoTrack(artist, track);
                var album = infoTrack.Album;
                if (infoTrack.Album != null)
                {
                    return RedirectToAction("Music", "Play", new { artist = artist, album = album, track = track });

                    //return Redirect($"/Play/Music/{artist}/{album}/{track}");
                }
            }
            catch (NullReferenceException)
            {
                return NotFound("This track is not contained on database.");
            }

            return NotFound("This track is not contained on some album.");
        }
    }
}