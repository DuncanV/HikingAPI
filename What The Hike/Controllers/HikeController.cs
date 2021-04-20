using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System.Web.Script.Serialization;

namespace What_The_Hike
{
    public class HikeController : Controller
    {
        //private readonly HikeContext db;
        // GET: Hike
        [System.Web.Mvc.HttpGet]
        public String Index()
        {
            List<Hike> hikes = new List<Hike>();
            using (HikeContext db = new HikeContext())
            {
                hikes.AddRange(db.Hike.ToList());
            }
            string Ret = string.Empty;
            Ret = "{\"Success\": false, \"message\": \"WeatherApi call failed\"}";
            this.HttpContext.Response.StatusCode = 200;
            this.HttpContext.Response.ContentType = "application/json; charset=utf-8";
            return Ret;
        }

        // GET: Hike/weather/{id}
        [System.Web.Mvc.HttpGet]
        public String weather(int id)
        {
            Hike hike = null;
            Facility facility = null;
            using (HikeContext db = new HikeContext())
            {
                hike = db.Hike.Where(e => e.hikeID == id).FirstOrDefault();
                if(hike!= null)
                {
                    facility = db.Facility.Where(e => e.facilityID == hike.facilityID).FirstOrDefault();
                }
            }
            string Ret = string.Empty;

            if (facility!= null)
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://api.openweathermap.org/");
                var latitude = facility.latitude;
                var longitude = facility.longitude;
                var apiKey = "7db484b6881763be58be890766009069";
                string endpoint = $"data/2.5/onecall?lat={latitude}&lon={longitude}&units=metric&appid={apiKey}";

                HttpResponseMessage response = client.GetAsync(endpoint).Result;
                if (response.IsSuccessStatusCode)
                {
                    Ret = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                    Ret = "{\"Success\": false, \"message\": \"WeatherApi call failed\"}";
                }
            }
            else
            {
                if(hike == null)
                {
                    Ret = "{\"Success\": false, \"message\": \"No Hike with the given ID\"}";
                }
                else
                {
                    Ret = "{\"Success\": false, \"message\": \"No Facility linked to given Hike\"}";
                }
            }

            this.HttpContext.Response.StatusCode = 200;
            this.HttpContext.Response.ContentType = "application/json; charset=utf-8";
            return Ret;
        }
       
        //Get: Hike/logs
        public string GetAllHikeLogs()
        {
            List<HikeLog> hikeLogs;
            using (HikeContext db = new HikeContext())
            {
                hikeLogs =  db.HikeLog.Where(e => e.hikeID == e.Hike.hikeID).ToList();
                if (hikeLogs.Count > 0)
                {
                    foreach (var log in hikeLogs)
                    {
                        log.User =  db.User.Find(log.userID);
                        log.Hike =  db.Hike.Find(log.hikeID);
                    }
                    return "{\"Success\": True, \"message\": \"Hike Logs returned\"}" + hikeLogs;
                }

                return "{\"Success\": false, \"message\": \"No Hike Log with the given ID\"}";
            }
        }
        //Get: Hike/logs/{id}
        [System.Web.Mvc.HttpGet]
        public IEnumerable<string> GetLogDetails(int? id)
        {
            HikeLog hikeLogs;
            List<string> details = new List<string>();
            using (HikeContext db = new HikeContext())
            {
                hikeLogs =  db.HikeLog.SingleOrDefault(e => e.hikeLogID == id);
                if (hikeLogs != null)
                {
                    details.Add(hikeLogs.Hike.name);
                    details.Add(hikeLogs.Hike.description);
                    details.Add(hikeLogs.Hike.Difficulty.description);
                    details.Add(hikeLogs.Hike.distance.ToString());
                    details.Add(hikeLogs.Hike.Facility.name);
                    details.Add(hikeLogs.Hike.Duration.time.ToString());
                }
                HttpContext.Response.StatusCode = 200;
                HttpContext.Response.ContentType = "application/json; charset=utf-8";
                return details;
            }

            //return "{\"method\":GetLogDetails,\"results\":Failed}";
        }
        
        //POST: Hike/logs
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public string Create(HikeLog hikeLog)
        {
            if (ModelState.IsValid)
            {
                using (HikeContext db = new HikeContext())
                {
                    db.HikeLog.Add(hikeLog);
                    db.SaveChanges();
                    return "{\"method\": Create New Log, \"result\": Difficulty changed}";
                }
            }
            HttpContext.Response.StatusCode = 200;
            HttpContext.Response.ContentType = "application/json; charset=utf-8";
            return "{\"method\": Create New Log, \"result\": Failed}";;
        }
        
        public string ChangeDifficulty(int? id, Hike hike)
        {
            using (HikeContext db = new HikeContext())
            {
                var data = db.Hike.SingleOrDefault(x => x.hikeID == id);
                if (data != null)
                {
                    data.Difficulty = hike.Difficulty;
                    db.SaveChanges();
                }

                return "";
            }
        }
        
        /**
         * GET: Hike/LeaderBoard/
         */
        [HttpGet]
        public string LeaderBoard()
        {
            List<User> Users;
            List<HikeLog> UsersHikeLogs;

            Dictionary<int, double> UserScore = new Dictionary<int, double>();

            using (HikeContext db = new HikeContext())
            {
                Users = db.User
                    .ToList();

                UsersHikeLogs = db.HikeLog
                    .ToList();

                foreach (User u in Users)
                {
                    List<HikeLog> Log = UsersHikeLogs
                    .Where(l => l.userID == u.userID)
                    .ToList();

                    double sum = 0.0;

                    foreach(HikeLog hikeLog in Log)
                    {
                        Hike hike = db.Hike
                            .Where(h => h.hikeID == hikeLog.hikeID)
                            .FirstOrDefault();

                        sum += hike.distance;
                    }

                    UserScore.Add(u.userID, sum);
                }

                var items = from pair in UserScore
                            orderby pair.Value descending,
                                    pair.Key
                            select pair;

                var json = JsonConvert.SerializeObject(items, Formatting.Indented);

                return json;
               
            }
        }
    }
}