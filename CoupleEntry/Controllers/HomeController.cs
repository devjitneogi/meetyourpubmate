using CoupleEntry.AuthenticationProvider;
using CoupleEntry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CoupleEntry.SessionService;

namespace CoupleEntry.Controllers
{

    public class HomeController : Controller
    {
        //public static HashSet<NearbyUser> nearbyUsers = new HashSet<NearbyUser>();
        [UxWebAuthorize]
        public ActionResult Index()
        {
            if (Request.Cookies["UserMail"] != null)
                SetProperty(SessionVariableNames.Email_Id, Request.Cookies["UserMail"].Value);
            //nearbyUsers.RemoveWhere(r => (DateTime.Now - r.lastSeen).TotalMinutes > 5);
            return View();
        }

        public bool AddUserPositionToDB(string latitude, string longitude)
        {
            string emailId = GetProperty(SessionVariableNames.Email_Id) as string;
            //upsert to DB Position table, date time you can pass from here or in SP itself
            DALayer.UpsertUserPosition(emailId, latitude, longitude);
            return true;
        }

        public JsonResult GetOtherUsers()
        {
            string emailId = GetProperty(SessionVariableNames.Email_Id) as string;
            return Json(DALayer.GetAllUsers(emailId), JsonRequestBehavior.AllowGet);

        }
    }
}