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
        public ActionResult Login(string email, string name)
        {

            //if (loginModel == null)
            //{
            //    ModelState.AddModelError("", "A username and password is required.");
            ////    LoginModel passBackLoginModel = CreateLoginModel();
            //    // Remove password.
            //  //  passBackLoginModel.Password = null;
            //    return View();
            //}

            return null;
        }


       
    }
}