using CoupleEntry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoupleEntry
{
    public class UserSessionVariables
    {
        public LoginModel loginModel;

    }
    public class SessionService
    {
        public class SessionVariableNames
        {
            public static string Login_Model { get { return "loginModel"; } }

        }
        private static void setPropertyValue(ref UserSessionVariables obj, string propertyName, object propertyValue)
        {
            switch (propertyName)
            {
                case "loginModel":
                    obj.loginModel = propertyValue as LoginModel;
                    break;
                default:
                    break;
            }
        }
        private static object findPropertyValue(UserSessionVariables obj, string propertyName)
        {
            switch (propertyName)
            {
                case "loginModel":
                    return obj.loginModel;
                default:
                    return null;
            }
        }
        public static object GetProperty(string propertyName, string mailId)
        {
            UserSessionVariables obj = HttpContext.Current.Session[mailId] as UserSessionVariables;
            if (obj != null)
                return findPropertyValue(obj, propertyName);
            else
                return null;
        }

        public static void SetProperty(string propertyName, object propertyValue, string mailId)
        {
            UserSessionVariables obj = HttpContext.Current.Session[mailId] as UserSessionVariables;
            if (obj == null)
            {
                obj = new UserSessionVariables();
            }
            setPropertyValue(ref obj, propertyName, propertyValue);
            HttpContext.Current.Session[mailId] = obj;
        }
    }
}