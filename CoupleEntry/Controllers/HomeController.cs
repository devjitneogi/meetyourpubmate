using CoupleEntry.AuthenticationProvider;
using System.Web.Mvc;
using CoupleEntry.Models;
using static CoupleEntry.SessionService;

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
            if (GetProperty(SessionVariableNames.Current_User) == null)
                SetProperty(SessionVariableNames.Current_User, DALayer.GetUserInfo(emailId));

          
            return View();
        }

        public bool AddUserPositionToDB(string latitude, string longitude)
        {
            string emailId = GetProperty(SessionVariableNames.Email_Id) as string;
            DALayer.UpsertUserPosition(emailId, latitude, longitude);
            return true;
        }

        public JsonResult GetOtherUsers()
        {
            string emailId = GetProperty(SessionVariableNames.Email_Id) as string;
            return Json(DALayer.GetAllUsers(emailId), JsonRequestBehavior.AllowGet);

        }

        public ActionResult EditUserDetails()
        {
            User model = GetProperty(SessionVariableNames.Current_User) as User;
            return View(model);

        }
        public ActionResult UpdateUserDetailsToDB(User model)
        {
            User updatedModel = DALayer.UpdateUserInfo(model);
            SetProperty(SessionVariableNames.Current_User, updatedModel);
            return RedirectToAction("Index");
        }
    }
}