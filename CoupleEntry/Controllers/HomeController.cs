using CoupleEntry.AuthenticationProvider;
using System.Web.Mvc;
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
    }
}