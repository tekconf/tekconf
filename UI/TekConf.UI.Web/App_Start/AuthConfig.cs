using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using TekConf.UI.Web.Models;

namespace TekConf.UI.Web
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            OAuthWebSecurity.RegisterTwitterClient(
                consumerKey: "3mYNb4Jt33Ttdgw4Q4Ppjw",
                consumerSecret: "RwRTOPu6tYoP2Yh0RBLOfQeWiTKPBjlfDv7IbNJ3G7k");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "417883241605228",
                appSecret: "c2df6f0a2ed2a01f6f0553b3e58ad715");

            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
