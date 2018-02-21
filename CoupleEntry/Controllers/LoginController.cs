﻿using CoupleEntry;
using CoupleEntry.Models;
using System;
using System.Web;
using System.Web.Mvc;
using static CoupleEntry.SessionService;

namespace UxWeb.Controllers
{
    public class LoginController : Controller
    {

        [HttpGet]
        public ActionResult Login()
        {
            RemoveCookiesAndSession();
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {

            if (loginModel != null)
            {
                SetCookies(loginModel);
                SetProperty(SessionVariableNames.Login_Model, loginModel);
                SetProperty(SessionVariableNames.Email_Id, loginModel.Email);
                bool exists = DALayer.IsEmailPresentInDB(loginModel.Email);
                if (exists)
                {
                    DALayer.UpsertTokenValue(loginModel.Token, loginModel.Email);
                    return Json(new { result = "Redirect", url = Url.Action("Index", "Home") }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "Add", url = Url.Action("AddUser", "Login", JsonRequestBehavior.AllowGet) });
                }

            }

            return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddUserInformation()
        {
            LoginModel model = GetProperty(SessionVariableNames.Login_Model) as LoginModel;
            return View(model);
        }

        [HttpPost]
        public ActionResult AddUserToDB(LoginModel model)
        {
            DALayer.AddNewUser(model.Username, model.Age, model.Email, model.Gender, model.ImageUrl, model.Name);
            DALayer.UpsertTokenValue(model.Token, model.Email);
            return RedirectToAction("Index", "Home");
        }

        private void SetCookies(LoginModel loginModel)
        {
            HttpCookie AuthCookie = new HttpCookie("Authorization", loginModel.Token);
            AuthCookie.Expires = DateTime.Now.AddSeconds(loginModel.Expiry);
            Response.Cookies.Add(AuthCookie);

            HttpCookie EmailCookie = new HttpCookie("UserMail", loginModel.Email);
            EmailCookie.Expires = DateTime.Now.AddSeconds(loginModel.Expiry);
            Response.Cookies.Add(EmailCookie);

        }
        private void RemoveCookiesAndSession()
        {
            if (Request.Cookies["Authorization"] != null)
            {
                var c = new HttpCookie("Authorization");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            if (Request.Cookies["UserMail"] != null)
            {
                var c = new HttpCookie("UserMail");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            Session.Clear();
        }
    }
}