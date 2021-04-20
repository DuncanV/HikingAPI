using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
//using System.Web.Http;
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
        [HttpGet]
        public JsonResult Index()
        {
            List<HikeGeneralView> hikes = new List<HikeGeneralView>();
            using (HikeContext db = new HikeContext())
            {
                var hikesDB = db.Hike.ToList();
                foreach (var item in hikesDB)
                {
                    Difficulty difficulty = db.Difficulty.Where(e => e.difficultyID == item.difficultyID).FirstOrDefault();
                    Facility facility = db.Facility.Where(e => e.facilityID == item.facilityID).FirstOrDefault();
                    Duration duration = db.Duration.Where(e => e.durationID == item.avgDuration).FirstOrDefault();
                    hikes.Add(new HikeGeneralView
                    {
                        hikeID = item.hikeID,
                        name = item.name,
                        map = item.map,
                        description = item.description,
                        coordinates = item.coordinates,
                        distance = item.distance,
                        enteranceFee = item.enteranceFee,
                        maxGroupSize = item.maxGroupSize,
                        difficulty = difficulty.description,
                        duration = duration.time,
                        facilityId = facility.facilityID,
                        facilityName = facility.name,
                    }
                    );
                }
            }
            this.HttpContext.Response.StatusCode = 200;
            this.HttpContext.Response.ContentType = "application/json; charset=utf-8";
            if (hikes.Count > 0)
            {
                return Json(hikes.OrderBy(e => e.hikeID).ToList(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string Ret = string.Empty;
                Ret = "{\"Success\": false, \"message\": \"No Hikes available\"}";
                return Json(Ret, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Hike/getHike/{id}
        [HttpGet]
        public JsonResult GetHike(int id)
        {
            HikeDetailView hike = null;

            using (HikeContext db = new HikeContext())
            {
                Hike hikeDB = db.Hike.Where(e => e.hikeID == id).FirstOrDefault();
                if (hikeDB != null)
                {
                    Difficulty difficultyDB = db.Difficulty.Where(e => e.difficultyID == hikeDB.difficultyID).FirstOrDefault();
                    Facility facilityDB = db.Facility.Where(e => e.facilityID == hikeDB.facilityID).FirstOrDefault();
                    Duration durationDB = db.Duration.Where(e => e.durationID == hikeDB.avgDuration).FirstOrDefault();

                    FacilityView facility = new FacilityView
                    {
                        facilityID = facilityDB.facilityID,
                        name = facilityDB.name,
                        latitude = facilityDB.latitude,
                        longitude = facilityDB.longitude,
                        parking = facilityDB.parking,
                        pets = facilityDB.pets,
                        bookingRequired = facilityDB.bookingRequired,
                        HikesInFacility = getHikesByFacilityId(facilityDB.facilityID)
                    };

                    hike = new HikeDetailView
                    {
                        hikeID = hikeDB.hikeID,
                        name = hikeDB.name,
                        map = hikeDB.map,
                        description = hikeDB.description,
                        coordinates = hikeDB.coordinates,
                        distance = hikeDB.distance,
                        enteranceFee = hikeDB.enteranceFee,
                        maxGroupSize = hikeDB.maxGroupSize,
                        difficulty = difficultyDB.description,
                        duration = durationDB.time,
                        facility = facility,
                        pointsOfInterest = PointsOfInterest(hikeDB.hikeID)
                    };
                }
            }
            this.HttpContext.Response.StatusCode = 200;
            this.HttpContext.Response.ContentType = "application/json; charset=utf-8";
            if (hike != null)
            {
                return Json(hike, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string Ret = string.Empty;
                Ret = "{\"Success\": false, \"message\": \"Invalid Hike ID\"}";
                return Json(Ret, JsonRequestBehavior.AllowGet);
            }
        }

        private List<PointOfInterestView> PointsOfInterest(int hikeID)
        {
            List<PointOfInterestView> poi = new List<PointOfInterestView>();
            using (HikeContext db = new HikeContext())
            {
                var poiIDs = db.HikeInterestLink.Where(e => e.hikeID == hikeID).Select(e => e.pointOfInterestID).ToList();
                var points = db.PointOfInterest.Where(e => poiIDs.Contains(e.pointOfInterestID)).ToList();
                foreach (var item in points)
                    if (poi.Where(e => e.poiID == item.pointOfInterestID).FirstOrDefault() == null)
                        poi.Add(new PointOfInterestView { poiID = item.pointOfInterestID, PointOfInterestDescription = item.description });
            }
            return poi;
        }

        public List<HikeSimpleView> getHikesByFacilityId(int facilityId)
        {
            List<HikeSimpleView> hikes = new List<HikeSimpleView>();
            using (HikeContext db = new HikeContext())
            {
                var hikesDB = db.Hike.Where(e => e.facilityID == facilityId).ToList();
                foreach (var item in hikesDB)
                {
                    hikes.Add(new HikeSimpleView { hikeID = item.hikeID, name = item.name });
                }
            }

            return hikes;
        }

        // GET: Hike/weather/{id}
        [HttpGet]
        public String Weather(int id)
        {
            Hike hike = null;
            Facility facility = null;
            using (HikeContext db = new HikeContext())
            {
                hike = db.Hike.Where(e => e.hikeID == id).FirstOrDefault();
                if (hike != null)
                {
                    facility = db.Facility.Where(e => e.facilityID == hike.facilityID).FirstOrDefault();
                }
            }
            string Ret = string.Empty;

            if (facility != null)
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
                if (hike == null)
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
            //return Json(Ret, JsonRequestBehavior.AllowGet);
        }

        //Get: Hike/logs
        [HttpGet]
        public string GetAllHikeLogs()
        {
            List<HikeLog> hikeLogs;
            using (HikeContext db = new HikeContext())
            {
                hikeLogs = db.HikeLog.Where(e => e.hikeID == e.Hike.hikeID).ToList();
                if (hikeLogs.Count > 0)
                {
                    foreach (var log in hikeLogs)
                    {
                        log.User = db.User.Find(log.userID);
                        log.Hike = db.Hike.Find(log.hikeID);
                    }
                    return "{\"Success\": True, \"message\": \"Hike Logs returned\"}" + hikeLogs;
                }

                return "{\"Success\": false, \"message\": \"No Hike Log with the given ID\"}";
            }
        }
        //Get: Hike/logs/{id}
        [HttpGet]
        public IEnumerable<string> GetLogDetails(int? id)
        {
            HikeLog hikeLogs;
            List<string> details = new List<string>();
            using (HikeContext db = new HikeContext())
            {
                hikeLogs = db.HikeLog.SingleOrDefault(e => e.hikeLogID == id);
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
        [HttpPost]
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
            return "{\"method\": Create New Log, \"result\": Failed}"; ;
        }

        [HttpPost]
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

                    foreach (HikeLog hikeLog in Log)
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