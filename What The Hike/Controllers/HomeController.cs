using System;
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
    }
}