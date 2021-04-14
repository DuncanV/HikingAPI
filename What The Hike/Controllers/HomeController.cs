using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration;
using System.Net.Http;

namespace What_The_Hike
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //! Stuart To Fix
        //[HttpPost]
        //public JsonResult GetAllHikesLogged()
        //{
        //    List<HikeLog> hikeLog;
        //    using (HikeContext db = new HikeContext())
        //    {
        //        hikeLog = new List<HikeLog>(db.HikeLog.ToList());
        //        if (hikeLog.Count > 0)
        //        {
        //            foreach (var item in hikeLog)
        //            {
        //                item.User = db.User.Where(e => item.userID == e.userID).FirstOrDefault();
        //                item.Hike = db.Hike.Where(e => item.hikeID == e.hikeID).FirstOrDefault();
        //            }
        //        }
        //    }
        //    if (hikeLog.Count > 0)
        //    {
        //        var retObj = new List<List<string>>();
        //        foreach (var item in hikeLog)
        //        {
        //            var tempList = new List<string>();
        //            tempList.Add(item.Hike.name);
        //            tempList.Add(item.User.name + item.User.surname);
        //            retObj.Add(tempList);
        //        }

        //        return Json(hikeLog);
        //    }
        //    else { return null; }
        //}

        [HttpPost]
        public JsonResult PointsOfInterest(string poiName = null, int hikeID = 0)
        {
            List<PointOfInterestView> poi = new List<PointOfInterestView>();
            using (HikeContext db = new HikeContext())
            {
                if (poiName != null)
                {
                    var points = db.PointOfInterest.Where(e => e.pointOfInterestID == hikeID).ToList();
                    foreach (var item in points)
                        poi.Add(new PointOfInterestView { poiID = item.pointOfInterestID, PointOfInterestDescription = item.description });
                }
                if (hikeID > 0)
                {
                    var poiIDs = db.HikeInterestLink.Where(e => e.hikeID == hikeID).Select(e => e.pointOfInterestID).ToList();
                    var points = db.PointOfInterest.Where(e => poiIDs.Contains(e.pointOfInterestID)).ToList();
                    foreach (var item in points)
                        if (poi.Where(e => e.poiID == item.pointOfInterestID).FirstOrDefault() == null)
                            poi.Add(new PointOfInterestView { poiID = item.pointOfInterestID, PointOfInterestDescription = item.description });
                }
            }
            return Json(poi.OrderBy(e=>e.poiID).ToList());
        }

        [HttpGet]
        public String getTempWeather()
        {
            string Ret = string.Empty;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.openweathermap.org/");
            var latitude = -25.8321;
            var longitude = 28.2914;
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
                Ret = "{Success: false, message: \"WeatherApi call failed\"}";
            }
            this.HttpContext.Response.StatusCode = 200;
            this.HttpContext.Response.ContentType = "application/json; charset=utf-8";
            return Ret;
        }
    }
}