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

        [HttpGet]
        public String getLocation()
        {

        }

        [HttpPost]
        public String LoadLocation()
        {

        }

        [HttpPatch]
        public String updateLocation()
        {

        }

        [HttpDelete]
        public String removeLocation()
        {

        }
    }
}