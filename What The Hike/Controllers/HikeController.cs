using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace What_The_Hike
{
    public class HikeController : Controller
    {
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
        public JsonResult Index(int id)
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
                        facility = facility
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
        public JsonResult weather(int id)
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
            return Json(Ret, JsonRequestBehavior.AllowGet);
        }
    }
}