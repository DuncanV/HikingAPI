using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using What_The_Hike.Models;

namespace What_The_Hike.Controllers
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
        [HttpGet]
        [Authorize]
        public ActionResult ViewHikeLog()
        {
            HikeLog_UserHikeID logs = new HikeLog_UserHikeID() { User = new List<User>(), Hike = new List<Hike>() };
            using (HikeContext db = new HikeContext())
            {
                logs.User.AddRange(
                   db.User.ToList());
                logs.Hike.AddRange(
                   db.Hike.ToList());
            }

            return View(logs);
        }
    }
}