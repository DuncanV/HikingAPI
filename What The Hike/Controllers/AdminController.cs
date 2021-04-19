using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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
        [HttpGet]
        public string GetLocation(int id)
        {
            Facility Fac;

            using(HikeContext db = new HikeContext())
            {
                var TempFac = db.Facility.Find(id);

                if (TempFac != null)
                {
                    return new JavaScriptSerializer().Serialize(TempFac); ;
                } else
                {
                    return "{\"Success\": false, \"message\": \"Facility does not exist\"}";
                }
            }

        }

        // POST: /Admin/LoadFacilityHours/
        [HttpPost]
        public string LoadFacilityHours(int FacilityId, int OperatingHoursId)
        {
            FacilityHoursLink FacilityHours = new FacilityHoursLink { facilityID = FacilityId, operatingHoursID = OperatingHoursId };

            using (HikeContext db = new HikeContext())
            {

                var Hours = db.FacilityHoursLink
                    .Where(h => (h.facilityID == FacilityId) && (h.operatingHoursID == OperatingHoursId))
                    .FirstOrDefault();

                if (Hours != null)
                {
                    return "{\"Success\": false, \"message\": \"Facility Hours already exists\"}";
                }

                var Facility = db.Facility
                    .Where(f => f.facilityID == FacilityId)
                    .FirstOrDefault();

                var OpHours = db.OperatingHours
                    .Where(o => o.operatingHoursID == OperatingHoursId)
                    .FirstOrDefault();

                if (Facility == null)
                {
                    return "{\"Success\": false, \"message\": \"Could not add FacilityHoursLink because Facility id does not exist\"}";
                } else if (OpHours == null)
                {
                    return "{\"Success\": false, \"message\": \"Could not add FacilityHoursLink because Operating Hours id does not exist\"}";
                }

                db.FacilityHoursLink.Add(FacilityHours);
                db.SaveChanges();
            }

            return "{\"Success\": true, \"message\": \"Added Facility Hours\"}"; 
        }

        // POST: /Admin/LoadOperatingHours
        [HttpPost]
        public string LoadOperatingHours(int TimeFrom, int TimeTo, int Day)
        {
            OperatingHours hours = new OperatingHours { time_from = TimeFrom, time_to = TimeTo, day = Day };

            using (HikeContext db = new HikeContext())
            {

                var OpHours = db.OperatingHours
                    .Where(o => (o.time_from == TimeFrom) && (o.time_to == TimeTo) && (o.day == Day))
                    .FirstOrDefault();

                if (OpHours != null)
                {
                    return "{\"Success\": false, \"message\": \"Operating hours already exists\"}";
                }

                db.OperatingHours.Add(hours);
                db.SaveChanges();
            }

            return "{\"Success\": true, \"message\": \"Added Operating Hours\"}";
        }

        // POST: /Admin/LoadLocation
        [HttpPost]
        public String LoadLocation(string Name, float Longitude, float Latitude, bool Parking, bool Pets, bool BookingRequired)
        {
            Facility Fac = new Facility { name = Name, longitude = Longitude, latitude = Latitude, parking = Parking, pets = Pets, bookingRequired = BookingRequired };

            using (HikeContext db = new HikeContext())
            {
                var TempFacility = db.Facility
                    .Where(f => (f.name == Name) && (f.longitude == Longitude) && (f.latitude == Latitude) && (f.parking == Parking) && (f.pets == Pets) && (f.bookingRequired == BookingRequired))
                    .FirstOrDefault();

                if (TempFacility != null)
                {
                   return "{\"Success\": false, \"message\": \"Facility already exists\"}";
                }

                db.Facility.Add(Fac);
                db.SaveChanges();

                return "{\"Success\": true, \"message\": \"Facility added\"}";
            }
        }

        // PATCH: /Admin/UpdateLocation/{id}
        [HttpPatch]
        public String UpdateLocation(int id, string Name, float Longitude, float Latitude, bool Parking, bool Pets, bool BookingRequired)
        {

            Facility Fac = new Facility { facilityID = id, name = Name, longitude = Longitude, latitude = Latitude, parking = Parking, pets = Pets, bookingRequired = BookingRequired };

            using (HikeContext db = new HikeContext())
            {
                var TempFacility = db.Facility.Find(id);

                if (TempFacility == null)
                {
                    return "{\"Success\": false, \"message\": \"Facility does not exist\"}";
                }

                TempFacility.name = Name;
                TempFacility.longitude = Longitude;
                TempFacility.latitude = Latitude;
                TempFacility.parking = Parking;
                TempFacility.pets = Pets;
                TempFacility.bookingRequired = BookingRequired;
                db.Entry(TempFacility).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return "{\"Success\": true, \"message\": \"Facility updated\"}";
            }
        }

        // DELETE: /Admin/RemoveLocation/{id}
        /*[HttpDelete]
        public String RemoveLocation()
        {

        }*/
    }
}