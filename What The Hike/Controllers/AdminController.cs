using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using What_The_Hike.Models;

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
        public JsonResult GetLocation(int id)
        {
            FinalFacility final;

            using (HikeContext db = new HikeContext())
            {
                Facility TempFac = db.Facility
                    .Find(id);

                if (TempFac == null)
                {
                    var res = new ReturnObject
                    {
                        success = false,
                        message = "Facility does not exist",
                        data = new { }
                    };
                    return Json(res, JsonRequestBehavior.AllowGet);
                }

                var Link = db.FacilityHoursLink
                    .Where(f => f.facilityID == id)
                    .ToList();

                List<OperatingHours> OpHours = new List<OperatingHours>();
                Dictionary<string, string> dict = new Dictionary<string, string>();

                foreach (FacilityHoursLink link in Link)
                {
                    var Hours = db.OperatingHours
                        .Where(o => o.operatingHoursID == link.operatingHoursID)
                        .FirstOrDefault();

                    if (Hours != null)
                    {
                        dict.Add(Hours.day.ToString(), Hours.time_from + " - " + Hours.time_to);
                    }

                }

                for (int i = 1; i <= 7; i++)
                {
                    if (!dict.ContainsKey(i.ToString()))
                    {
                        dict.Add(i.ToString(), "No information");
                    }
                }

                final = new FinalFacility
                {
                    name = TempFac.name,
                    latitude = TempFac.latitude,
                    longitude = TempFac.longitude,
                    pets = TempFac.pets,
                    parking = TempFac.parking,
                    bookingRequired = TempFac.bookingRequired,
                    MondayOpHours = dict["1"],
                    TuesdayOpHours = dict["2"],
                    WednesdayOpHours = dict["3"],
                    ThursdayOpHours = dict["4"],
                    FridayOpHours = dict["5"],
                    SaturdayOpHours = dict["6"],
                    SundayOpHours = dict["7"]
                };

                var response = new ReturnObject
                {
                    success = true,
                    message = "Location Retrieved succesfully",
                    data = Json(final).Data
                };
                return Json(response, JsonRequestBehavior.AllowGet);

            }

        }

        // POST: /Admin/LoadFacilityHours/
        [HttpPost]
        public JsonResult LoadFacilityHours(int FacilityId, int OperatingHoursId)
        {
            FacilityHoursLink FacilityHours = new FacilityHoursLink { facilityID = FacilityId, operatingHoursID = OperatingHoursId };
            var res = new ReturnObject { };

            using (HikeContext db = new HikeContext())
            {

                var Hours = db.FacilityHoursLink
                    .Where(h => (h.facilityID == FacilityId) && (h.operatingHoursID == OperatingHoursId))
                    .FirstOrDefault();

                if (Hours != null)
                {
                    res = new ReturnObject
                    {
                        success = false,
                        message = "Facility Hours already exists",
                        data = new { }
                    };
                    return Json(res, JsonRequestBehavior.AllowGet);
                }

                var Facility = db.Facility
                    .Where(f => f.facilityID == FacilityId)
                    .FirstOrDefault();

                var OpHours = db.OperatingHours
                    .Where(o => o.operatingHoursID == OperatingHoursId)
                    .FirstOrDefault();

                if (Facility == null)
                {
                    res = new ReturnObject
                    {
                        success = false,
                        message = "Could not add FacilityHoursLink because Facility Id does not exist",
                        data = new { }
                    };
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                else if (OpHours == null)
                {
                    res = new ReturnObject
                    {
                        success = false,
                        message = "Could not add FacilityHoursLink because Operating Hours id does not exist",
                        data = new { }
                    };
                    return Json(res, JsonRequestBehavior.AllowGet);
                }

                db.FacilityHoursLink.Add(FacilityHours);
                db.SaveChanges();
            }

            res = new ReturnObject
            {
                success = true,
                message = "Added Facility Hours",
                data = new { }
            };
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        // POST: /Admin/LoadOperatingHours
        [HttpPost]
        public JsonResult LoadOperatingHours(int TimeFrom, int TimeTo, int Day)
        {
            OperatingHours hours = new OperatingHours { time_from = TimeFrom, time_to = TimeTo, day = Day };
            var res = new ReturnObject { };

            using (HikeContext db = new HikeContext())
            {

                var OpHours = db.OperatingHours
                    .Where(o => (o.time_from == TimeFrom) && (o.time_to == TimeTo) && (o.day == Day))
                    .FirstOrDefault();

                if (OpHours != null)
                {
                    res = new ReturnObject
                    {
                        success = false,
                        message = "Operating Hours already exists",
                        data = new { }
                    };
                    return Json(res, JsonRequestBehavior.AllowGet);
                }

                db.OperatingHours.Add(hours);
                db.SaveChanges();
            }
            res = new ReturnObject
            {
                success = true,
                message = "Added Operating Hours",
                data = new { }
            };
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        // POST: /Admin/LoadLocation
        [HttpPost]
        public JsonResult LoadLocation(string Name, float Longitude, float Latitude, bool Parking, bool Pets, bool BookingRequired)
        {
            Facility Fac = new Facility { name = Name, longitude = Longitude, latitude = Latitude, parking = Parking, pets = Pets, bookingRequired = BookingRequired };
            var res = new ReturnObject { };

            using (HikeContext db = new HikeContext())
            {
                var TempFacility = db.Facility
                    .Where(f => (f.name.CompareTo(Name) == 0) /*&& (f.longitude.Equals(Longitude)) && (f.latitude == Latitude) && (f.parking == Parking) && (f.pets == Pets) && (f.bookingRequired == BookingRequired)*/)
                    .FirstOrDefault();

                if (TempFacility != null)
                {
                    res = new ReturnObject
                    {
                        success = false,
                        message = "Facility already exists",
                        data = new { }
                    };
                    return Json(res, JsonRequestBehavior.AllowGet);
                }

                db.Facility.Add(Fac);
                db.SaveChanges();

                res = new ReturnObject
                {
                    success = true,
                    message = "Facility added",
                    data = new { }
                };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }

        // PATCH: /Admin/UpdateLocation/{id}
        [HttpPatch]
        public JsonResult UpdateLocation(int id, string Name, float Longitude, float Latitude, bool Parking, bool Pets, bool BookingRequired)
        {

            Facility Fac = new Facility { facilityID = id, name = Name, longitude = Longitude, latitude = Latitude, parking = Parking, pets = Pets, bookingRequired = BookingRequired };
            var res = new ReturnObject { };

            using (HikeContext db = new HikeContext())
            {
                var TempFacility = db.Facility.Find(id);

                if (TempFacility == null)
                {
                    res = new ReturnObject
                    {
                        success = false,
                        message = "Facility does not exist",
                        data = new { }
                    };

                    return Json(res, JsonRequestBehavior.AllowGet);
                }

                TempFacility.name = Name;
                TempFacility.longitude = Longitude;
                TempFacility.latitude = Latitude;
                TempFacility.parking = Parking;
                TempFacility.pets = Pets;
                TempFacility.bookingRequired = BookingRequired;
                db.Entry(TempFacility).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                res = new ReturnObject
                {
                    success = true,
                    message = "Facility updated",
                    data = new { }
                };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }

        // DELETE: /Admin/RemoveLocation/{id}
        [HttpDelete]
        public JsonResult RemoveLocation(int id)
        {
            var res = new ReturnObject { };
            using (HikeContext db = new HikeContext())
            {
                Facility facility = db.Facility.Find(id);

                if (facility == null)
                {
                    res = new ReturnObject
                    {
                        success = false,
                        message = "Facility could not be found",
                        data = new { }
                    };
                }
                else
                {
                    db.Facility.Remove(facility);
                    db.SaveChanges();
                    res = new ReturnObject
                    {
                        success = true,
                        message = "Facility deleted",
                        data = new { }
                    };

                }
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }
    }
}