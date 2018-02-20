using CoupleEntry.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace UxWeb.Controllers
{
    public class LoginController : Controller
    {
     
        [HttpGet]
        public ActionResult Login()
        {
            // LoginModel loginModel = CreateLoginModel();
            Session.Abandon();
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {

            if (loginModel != null)
            {
                //if loginModel.Email in DB
                //update values of token in DB and cookies
               // return Json(new { result = "Redirect", url = Url.Action("Index", "Home") }, JsonRequestBehavior.AllowGet);


                //else (not in DB)
                //Add details to DB and cookies
                return Json(new { result = "Add" ,url = Url.Action("AddUser", "Login",new {name=loginModel.Name,email=loginModel.Email })}, JsonRequestBehavior.AllowGet);

            }

            return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddUser(string name,string email)
        {
            LoginModel model = new LoginModel() { Name=name,Email=email};
            return View(model);
        }

    }
}