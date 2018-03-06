using CoupleEntry.AuthenticationProvider;
using System.Web.Mvc;
using CoupleEntry.Models;
using static CoupleEntry.SessionService;
using System.Collections.Generic;
using System;

namespace CoupleEntry.Controllers
{

    public class HomeController : Controller
    {
        [UxWebAuthorize]
        public ActionResult Index()
        {

            string emailId = "";
            if (Request.Cookies["UserMail"] != null)
            {
                emailId = Request.Cookies["UserMail"].Value;
                SetProperty(SessionVariableNames.Email_Id, emailId);
                bool exists = DALayer.IsEmailPresentInDB(emailId);
                if (!exists)
                {
                    return RedirectToAction("Login", "Login");
                }
            }

            SetProperty(SessionVariableNames.Current_User, DALayer.GetUserInfo(emailId));

            return View();
        }

        public bool AddUserPositionToDB(string latitude, string longitude)
        {
            string emailId = GetProperty(SessionVariableNames.Email_Id) as string;
            DALayer.UpsertUserPosition(emailId, latitude, longitude);
            return true;
        }

        [UxWebAuthorize]
        public JsonResult GetOtherUsers()
        {
            string emailId = GetProperty(SessionVariableNames.Email_Id) as string;
            if (emailId == null && Request.Cookies["UserMail"] != null)
            {
                emailId = Request.Cookies["UserMail"].Value;
            }
            List<User> users = DALayer.GetAllUsers(emailId);
            users.ForEach(x => x.LastSeenDiff = (DateTime.UtcNow - x.LastSeen).TotalSeconds.ToString());
            return Json(users, JsonRequestBehavior.AllowGet);

        }

        [UxWebAuthorize]
        public ActionResult EditUserDetails()
        {
            User model = GetProperty(SessionVariableNames.Current_User) as User;
            if (model == null)
                return RedirectToAction("Index");
            return View(model);

        }
        [UxWebAuthorize]
        public ActionResult UpdateUserDetailsToDB(User model)
        {
            User updatedModel = DALayer.UpdateUserInfo(model);
            SetProperty(SessionVariableNames.Current_User, updatedModel);
            return RedirectToAction("Index");
        }

        [UxWebAuthorize]
        public bool AddOrRemoveLike(int targetId, bool liked)
        {
            User model = GetProperty(SessionVariableNames.Current_User) as User;
            if (model == null && Request.Cookies["UserMail"] != null)
            {
                string emailId = Request.Cookies["UserMail"].Value;
                model = DALayer.GetUserInfo(emailId);
            }
            return DALayer.AddOrRemoveLike(model.UserId, targetId, liked);
        }
    }
}