using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace What_The_Hike.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Admin/GetLocation/{id}
        /*[HttpGet]
        public String GetLocation(int id)
        {
            Facility facility;

            using(HikeContext db = new HikeContext())
            {
                
            }

        }*/

        // POST: /Adming/LoadFacilityHours/{id}
        /*[HttpPost]
        public string LoadFaciltyHours(int id)
        {

        }*/

        // POST: /Admin/LoadOperatingHours
        [HttpPost]
        public string LoadOperatingHours(int TimeFrom, int TimeTo, int Day)
        {
            OperatingHours hours = new OperatingHours { time_from = TimeFrom, time_to = TimeTo, day = Day };

            using (HikeContext db = new HikeContext())
            {

                var OpHours = db.OperatingHours
                    .Where(o => (o.time_from == TimeFrom) && (o.time_to == TimeTo) && (o.day == Day))
                    .ToList();

                if (OpHours != null)
                {
                    return "Operating hours could not be loaded because it already exists";
                }

                db.OperatingHours.Add(hours);
                db.SaveChanges();
            }

            return "Added Operation Hours => TimeFrom: " + TimeFrom + " TimeTo: " + TimeTo + " Day: " + Day ;
        }

        // POST: /Admin/LoadLocation
       /* [HttpPost]
        public String LoadLocation(string Name, float Longitude, float Latitude, bool Parking, bool Pets, bool BookingRequired, int FacilityHoursLink)
        {
            Facility facility;

            using (HikeContext db = new HikeContext())
            {

            }
        }*/

        // PATCH: /Admin/UpdateLocation/{id}
        /*[HttpPatch]
        public String UpdateLocation(int id)
        {

        }*/

        // DELETE: /Admin/RemoveLocation/{id}
       /* [HttpDelete]
        public String RemoveLocation()
        {

        }*/
    }
}