﻿using CoupleEntry.AuthenticationProvider;
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

            string emailId = GetEmailIdAndRefreshUserSession(true);
            if (emailId != null)
            {
                bool exists = DALayer.IsEmailPresentInDB(emailId);
                if (!exists)
                {
                    return RedirectToAction("Login", "Login");
                }
            }

            return View();
        }

        public bool AddUserPositionToDB(string latitude, string longitude)
        {
            string emailId = GetEmailIdAndRefreshUserSession(false);
            DALayer.UpsertUserPosition(emailId, latitude, longitude);
            return true;
        }

        [UxWebAuthorize]
        public JsonResult GetOtherUsers()
        {
            string emailId = GetEmailIdAndRefreshUserSession(false);
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
            bool matched = DALayer.AddOrRemoveLike(model.UserId, targetId, liked);
            if (liked)
                model.Likes.Add(targetId.ToString());
            else
                model.Likes.Remove(targetId.ToString());
            if (matched)
                model.Matches.Add(targetId.ToString());

            //SetProperty(SessionVariableNames.Current_User, model);
            return matched;
        }

        private string GetEmailIdAndRefreshUserSession(bool refreshUserSession)
        {
            string emailId = GetProperty(SessionVariableNames.Email_Id) as string;
            if (emailId == null && Request.Cookies["UserMail"] != null)
            {
                emailId = Request.Cookies["UserMail"].Value;
                SetProperty(SessionVariableNames.Email_Id, emailId);
            }

            if (refreshUserSession)
            {
                SetProperty(SessionVariableNames.Current_User, DALayer.GetUserInfo(emailId));
            }
            else
            {
                User userModel = GetProperty(SessionVariableNames.Current_User) as User;
                if (userModel == null && emailId != null)
                    SetProperty(SessionVariableNames.Current_User, DALayer.GetUserInfo(emailId));
            }
            return emailId;
        }
    }
}