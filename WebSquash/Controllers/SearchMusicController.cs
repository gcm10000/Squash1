using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebSquash.Controllers
{
    public class SearchMusicController : Controller
    {
        // GET: SearchMusic
        public ActionResult Index()
        {
            return Content("Test");
        }

        // GET: SearchMusic/MetaData/?query=Lighthouse+Family
        public ActionResult MetaData(string query)
        {
            Uri url = new Uri("http://ws.audioscrobbler.com/2.0/").
            AddQuery("method", "track.search").
            AddQuery("track", query).
            AddQuery("api_key", "ae35bc3b28eb6c8460ed87e3749d354b").
            AddQuery("format", "json");

            var result = GetResult(url);
            Rootobject root = JsonConvert.DeserializeObject<Rootobject>(result);
            List<InfoSearchTrack> infotracks = new List<InfoSearchTrack>();
            
            foreach (var track in root.results.trackmatches.track)
            {
                InfoSearchTrack infotrack = new InfoSearchTrack();
                infotrack.Track = track.name;
                infotrack.Artist = track.artist;
                infotracks.Add(infotrack);
            }

            string json = JsonConvert.SerializeObject(infotracks);
            return Content(json);
        }

        // GET: SearchMusic/GetInfoAlbum/?artist=Lighthouse+Family&album=Ocean+Drive
        public ActionResult GetInfoAlbum(string artist, string album)
        {
            // /2.0/?method=album.getinfo&api_key=YOUR_API_KEY&artist=Cher&album=Believe&format=json
            Uri url = new Uri("http://ws.audioscrobbler.com/2.0/").
            AddQuery("method", "album.getinfo").
            AddQuery("api_key", "ae35bc3b28eb6c8460ed87e3749d354b").
            AddQuery("artist", artist).
            AddQuery("album", album).
            AddQuery("format", "json");

            return Content(GetResult(url));
        }

        //GET: SearchMusic/GetInfoTrack/?artist=Lighthouse+Famly&track=Loving+Every+Minute
        public ActionResult GetInfoTrack(string artist, string track)
        {
            string json = JsonConvert.SerializeObject(MGetInfoTrack(artist, track));
            return Content(json);
        }

        public static WebSquash.InfoTrack.InfoTrack MGetInfoTrack(string artist, string track)
        {
            // /2.0/?method=track.getinfo&api_key=ae35bc3b28eb6c8460ed87e3749d354b&artist=Michael+Jackson&track=Billie+Jean&format=json
            //http://ws.audioscrobbler.com/2.0/?method=track.getinfo&api_key=ae35bc3b28eb6c8460ed87e3749d354b&artist=Michael+Jackson&track=Billie+Jean&format=json
            Uri url = new Uri("http://ws.audioscrobbler.com/2.0/").
            AddQuery("method", "track.getinfo").
            AddQuery("api_key", "ae35bc3b28eb6c8460ed87e3749d354b").
            AddQuery("artist", artist).
            AddQuery("track", track).
            AddQuery("format", "json");

            var result = GetResult(url);
            result = result.Replace("#text", "text");
            WebSquash.InfoTrack.Rootobject root = JsonConvert.DeserializeObject<WebSquash.InfoTrack.Rootobject>(result);

            WebSquash.InfoTrack.InfoTrack infoTrack = new InfoTrack.InfoTrack();
            infoTrack.Track = root.track.name;
            infoTrack.Duration = Convert.ToInt32(root.track.duration);
            infoTrack.Artist = root.track.artist.name;
            infoTrack.Album = (root.track.album == null) ? null : root.track.album.title;
            infoTrack.Images = (root.track.album == null) ? null : root.track.album.image;

            return infoTrack;
        }

        // GET: SearchMusic/GetMusic/?music=Ain't+No+Sunshine&artist=Lighthouse+Family&duration=244
        public ActionResult GetMusic(string music, string artist, string duration)
        {
            string result = string.Empty;
            string query = music + " " + artist;

            try
            {
                Uri url = new Uri("http://slider.kz/vk_auth.php").
                AddQuery("q", query);
                result = GetResult(url);
            }
            catch (Exception ex)
            {
                Content(ex.Message);
            }
            var jsongetted = JObject.Parse(result);
            var jsonresolved = "{\"Array\": " + jsongetted.First.First.First.First.ToString() + "}";
            Metadata.GetMusic.Rootobject array = JsonConvert.DeserializeObject<WebSquash.Metadata.GetMusic.Rootobject>(jsonresolved);

            int d = Convert.ToInt32(duration);
            List<double> numbers = new List<double>();
            double maior = 0.00f;
            double menor = 0.00f;
            int posicao_menor = 0, posicao_maior = 0;

            List<Metadata.GetMusic.Array> musicsSelecteds = new List<Metadata.GetMusic.Array>();
            try
            {
                if (array.Array.First(x => x.tit_art.ToLower() == (artist.ToLower() + " - " + music.ToLower())) != null)
                {
                    foreach (var item in array.Array)
                    {
                        if (item.tit_art.ToLower() == (artist.ToLower() + " - " + music.ToLower()))
                        {
                            musicsSelecteds.Add(item);
                        }
                    }
                }
            }
            //Tratar problema com IF
            catch (InvalidOperationException)
            {
                string[] tags = query.ToLower().Split(' ');
                foreach (var dataMusic in array.Array)
                {
                    if (CheckStringofArray(dataMusic.tit_art.ToLower(), tags, 0))
                    {
                        musicsSelecteds.Add(dataMusic);
                    }
                }
            }


            for (int i = 0; i < musicsSelecteds.Count; i++)
            {
                double x = 0.00;
                if (musicsSelecteds[i].duration > d)
                {
                    x = (double)d / (double)musicsSelecteds[i].duration;
                }
                else if (d > musicsSelecteds[i].duration)
                {
                    x = (double)musicsSelecteds[i].duration / (double)d;
                }
                else if (d == musicsSelecteds[i].duration)
                {
                    x = (double)musicsSelecteds[i].duration / (double)d;
                }
                numbers.Add(x);

                if (i == 0)
                {
                    menor = numbers[0];
                    maior = numbers[0];
                }
                
                if (numbers[i] < menor)
                {

                    menor = numbers[i];
                    posicao_menor = i;
                }
                else if (numbers[i] > maior)
                {
                    maior = numbers[i];
                    posicao_maior = i;
                }

            }
            string json = JsonConvert.SerializeObject(musicsSelecteds[posicao_maior]);
            return Content(json);
        }

        public bool CheckStringofArray(string str, string[] tags, int index)
        {
            bool check = false;
            for (int i = index; i < tags.Length; i++)
            {
                if ((str.Contains(tags[i])))
                {
                    check = true;
                }
                else
                {
                    return false;
                }
            }
            return check;
        }

        //public bool Contains(string s, params string[] predicates)
        //{
        //    return predicates.All(s.Contains);
        //}

        public static string GetResult(Uri url)
        {

            // /2.0/?method=album.getinfo&api_key=YOUR_API_KEY&artist=Cher&album=Believe&format=json
            string result = string.Empty;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url.ToString());
                request.KeepAlive = false;
                request.UserAgent = @"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
                request.Method = "GET";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    result = reader.ReadToEnd();

                    reader.Close();
                    dataStream.Close();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return result;
        }

        // GET: SearchMusic/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SearchMusic/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SearchMusic/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SearchMusic/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SearchMusic/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SearchMusic/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}