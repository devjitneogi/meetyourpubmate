using CoupleEntry;
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
            // LoginModel loginModel = CreateLoginModel();
            Session.Abandon();
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {

            if (loginModel != null)
            {
                HttpCookie Cookie = new HttpCookie("Authorization", loginModel.Token);
                Cookie.Expires = DateTime.Now.AddSeconds(loginModel.Expiry);
                Response.Cookies.Add(Cookie);
                SetProperty(SessionVariableNames.Login_Model, loginModel, loginModel.Email);

                bool exists = DALayer.IsEmailPresentInDB(loginModel.Email);
                 
                if(exists)
                { //update values of token in DB
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
        public ActionResult AddUser(string email)
        {

            LoginModel model = GetProperty(SessionVariableNames.Login_Model, email) as LoginModel;
            return View(model);
        }

        [HttpPost]
        public ActionResult AddUserToDB(LoginModel model)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}