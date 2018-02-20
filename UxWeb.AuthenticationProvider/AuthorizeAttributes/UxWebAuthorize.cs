using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace CoupleEntry.AuthenticationProvider
{
    /// <summary>
    /// Provides a custom Authorize Attribute which ensured the user is authoirzed before allowing access to externally facing APIs.
    /// </summary>
    public class UxWebAuthorize : AuthorizeAttribute
    {
        /// <summary>
        /// Checks windows authentication or verifies the access token presented.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //if (HttpContext.Current.User != null &&
            //    HttpContext.Current.User.Identity.IsAuthenticated)
            //{
            //    return true;
            //}

            // Get the auth cookie.
            HttpCookieCollection cookies = httpContext.Request.Cookies;
            HttpCookie envCookie = cookies.Get("PivotalEnvironmentName");
            HttpCookie loggedInUserCookie = cookies.Get("loggedInUser");
            HttpCookie authCookie = cookies.Get("Authorization");
            HttpCookie refreshCookie = cookies.Get("Refresh_Token");
            HttpCookie userMailIdCookie = cookies.Get("userMailId");
            string envName = "";

            if (envCookie != null)
            {
                envName = envCookie.Value;
            }
            if (userMailIdCookie != null && authCookie != null && loggedInUserCookie != null && refreshCookie != null && !string.IsNullOrEmpty(envName))
            {
                return true;
            }

            //if refresh token is there, try to get new access token by sending refresh token.
            if ((authCookie == null || loggedInUserCookie == null) && refreshCookie != null && !string.IsNullOrEmpty(envName) && userMailIdCookie != null)
            {
              //  LoginWithRefreshToken(httpContext);
                loggedInUserCookie = cookies.Get("loggedInUser");
                authCookie = cookies.Get("Authorization");

                if (userMailIdCookie != null && authCookie != null && loggedInUserCookie != null && refreshCookie != null && !string.IsNullOrEmpty(envName))
                {
                    // UxWebAuthorize.SetAuthenticatedIdentity(loggedInUserCookie.Value);
                    return true;
                }
            }
            return false;
        }

     
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Login" }));
        }


        //*This code is no needed anymore.

        /// <summary>
        /// Set the current user's authenticated identity as 'userName' with a generic identity.
        /// </summary>
        /// <param name="userName">The user name the current user is authenticated as.</param>
        //internal static void SetAuthenticatedIdentity(string userName)
        //{
        //    // Check provided 'userName' is valid.
        //    if (!string.IsNullOrEmpty(userName))
        //    {
        //        // Create generic identity & principal and assign it to the current HttpContext.
        //        System.Security.Principal.GenericIdentity identity = new System.Security.Principal.GenericIdentity(userName);
        //        System.Security.Principal.GenericPrincipal principal = new System.Security.Principal.GenericPrincipal(identity, new string[0]);
        //        HttpContext.Current.User = (System.Security.Principal.IPrincipal)principal;
        //    }
        //}
    }
}
