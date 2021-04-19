using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace What_The_Hike
{
    public class HikeController : Controller
    {
        // GET: Hike
        [HttpGet]
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
        [HttpGet]
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

        /**
         * POST: Hike/LeaderBoard/{id}
         */
        [HttpPost]
        public SortedDictionary<int, double> LeaderBoard(int id)
        {
            List<User> Users;
            List<HikeLog> UsersHikeLogs;
            //List<Tuple<int, double>> UserScore = new List<Tuple<int, double>>();
            SortedDictionary<int, double> UserScore = new SortedDictionary<int, double>(Comparer<int>.Create((x, y) => y.CompareTo(x)));

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

                    //Tuple<int, double> UserDistance = new Tuple<int, double>(u.userID, sum);
                    // UserScore.Add(UserDistance);
                    UserScore.Add(u.userID, sum);
                }

               // UserScore.Sort((a, b) => a.Item2.CompareTo(b.Item2));
                Console.WriteLine(UserScore);

                return UserScore;
               // list.Sort((a, b) => a.Item2.CompareTo(b.Item2)); //sort based on the string element
            }
        }

    }
}