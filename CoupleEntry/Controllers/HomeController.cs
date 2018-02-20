using CoupleEntry.AuthenticationProvider;
using CoupleEntry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoupleEntry.Controllers
{

    public class HomeController : Controller
    {
        public static HashSet<NearbyUser> nearbyUsers = new HashSet<NearbyUser>();
        public ActionResult Index()
        {
            nearbyUsers.RemoveWhere(r => (DateTime.Now - r.lastSeen).TotalMinutes > 5);
            return View();
        }
        public ActionResult GoogleLogin(string email, string name)
        {
            return RedirectToAction("Index", "Home");
           
        }
        public ActionResult Index2()
        {
            nearbyUsers.RemoveWhere(r => (DateTime.Now - r.lastSeen).TotalMinutes > 5);
            return null;
        }
        public bool AddUser(string uname, int age, string gender, string latitude, string longitude)
        {
            if (nearbyUsers.Where(x => x.uname == uname).Count() == 0)
                nearbyUsers.Add(new NearbyUser() { uname = uname, age = age, gender = gender, latitude = latitude, longitude = longitude, lastSeen = DateTime.Now });
            else
            {
                nearbyUsers.FirstOrDefault(s => s.uname == uname).lastSeen = DateTime.Now;
                nearbyUsers.FirstOrDefault(s => s.uname == uname).latitude = latitude;
                nearbyUsers.FirstOrDefault(s => s.uname == uname).longitude = longitude;
                nearbyUsers.FirstOrDefault(s => s.uname == uname).age = age;
                nearbyUsers.FirstOrDefault(s => s.uname == uname).gender = gender;
            }
            return true;
        }

        public JsonResult GetOtherUsers()
        {
            return Json(nearbyUsers, JsonRequestBehavior.AllowGet);
        }
    }
}