﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration;

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
        public JsonResult pointsOfInterest(string poiName, int hikeID)
        {
            List<PointOfInterest> poi = new List<PointOfInterest>();
            using (HikeContext db = new HikeContext())
            {
                if (poiName != "")
                {
                    poi.AddRange(db.PointOfInterest.Where(e => e.pointOfInterestID == 2).ToList());
                }
                if (hikeID > 0)
                {
                    var poiIDs = db.HikeInterestLink.Where(e => e.hikeID == hikeID).Select(e => e.pointOfInterestID).ToList();
                    poi.AddRange(db.PointOfInterest.Where(e => poiIDs.Contains(e.pointOfInterestID)).ToList());
                }
                if (poi.Count > 0)
                {
                    poi = poi.Distinct().OrderBy(i => i.pointOfInterestID).ToList();
                }
            }
            return Json(poi);
        }

        [HttpGet]
        public JsonResult getTempWeather()
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
            // this.HttpContext.Response.ContentType = "application/json; charset=utf-8";
            return Json(Ret);
        }
    }
}