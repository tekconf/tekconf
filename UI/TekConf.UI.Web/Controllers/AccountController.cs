using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;
using TekConf.UI.Web.Models;
using WebMatrix.WebData;
//using MvcApplication1.Filters;

namespace MvcApplication1.Controllers
{
    [Authorize]
    //[InitializeSimpleMembership]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public void Login()
        {
            string provider = "google";
            OAuthWebSecurity.RequestAuthentication(provider, Url.Action("AuthenticationCallbackAsync"));
        }

        [AllowAnonymous]
        public ActionResult AuthenticationCallbackAsync()
        {
            var result = OAuthWebSecurity.VerifyAuthentication();
            if (result.IsSuccessful)
            {
                // name of the provider we just used
                var provider = result.Provider;
                // provider's unique ID for the user
                var uniqueUserID = result.ProviderUserId;
                // since we might use multiple identity providers, then 
                // our app uniquely identifies the user by combination of 
                // provider name and provider user id
                var uniqueID = provider + "/" + uniqueUserID;

                // we then log the user into our application
                // we could have done a database lookup for a 
                // more user-friendly username for our app
                FormsAuthentication.SetAuthCookie(uniqueID, false);

                // dictionary of values from identity provider
                var userDataFromProvider = result.ExtraData;
                var email = userDataFromProvider["email"];

                var repository = new RemoteDataRepository();
                AsyncManager.OutstandingOperations.Increment();
                MembershipCreateStatus status;
                Membership.Provider.CreateUser(email, string.Empty, email, "", "", true, Guid.NewGuid(), out status);
                //repository.CreateUser(email, user =>
                //{
                //    AsyncManager.Parameters["user"] = user;
                //    AsyncManager.OutstandingOperations.Decrement();
                //});

                //var gender = userDataFromProvider["gender"];

                return RedirectToAction("Index", "Api", result);
            }
            return View("Error", result.Error);
        }

        public ActionResult AuthenticationCallbackCompleted(UserDto user)
        {
            return RedirectToAction("Index", "Home");            
        }


        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
