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
        public ActionResult Issue()
        {
            var authorizationServer = new AuthorizationServer(new AuthorizationServerHost());
            var response = authorizationServer.HandleTokenRequest(Request).AsActionResult();

            return response;
        }

    }
}
