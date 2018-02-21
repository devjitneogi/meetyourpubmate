using CoupleEntry;
using CoupleEntry.Models;
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
                //if loginModel.Email in DB
                //update values of token in DB and cookies
                // return Json(new { result = "Redirect", url = Url.Action("Index", "Home") }, JsonRequestBehavior.AllowGet);


                //else (not in DB)
                //Add details cookies
                SetProperty(SessionVariableNames.Login_Model, loginModel, loginModel.Email);
                return Json(new { result = "Add", url = Url.Action("AddUser", "Login", JsonRequestBehavior.AllowGet) });

            }

            return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddUser(string email)
        {

            LoginModel model = GetProperty(SessionVariableNames.Login_Model, email) as LoginModel;
            return View(model);
        }

    }
}