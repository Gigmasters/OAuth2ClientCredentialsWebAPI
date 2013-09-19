using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpenAutoClientCredsWebAPI.Infrastructure;
using DotNetOpenAuth.OAuth2;
using DotNetOpenAuth.Messaging;

namespace OpenAutoClientCredsWebAPI.Controllers
{
    public class TokenController : Controller
    {
        /// <summary>
        /// This route will issue a token if the proper client credentials are passed in. 
        /// </summary>
        /// <returns>An ActionResult, either containing an access token and token information, or an explanation as to why a token request was refused.</returns>
        [HttpPost]
        public ActionResult Issue()
        {
            var authorizationServer = new AuthorizationServer(new AuthorizationServerHost());
            var response = authorizationServer.HandleTokenRequest(Request).AsActionResult();

            return response;
        }

    }
}
