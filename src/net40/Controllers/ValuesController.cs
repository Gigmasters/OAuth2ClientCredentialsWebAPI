using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OpenAutoClientCredsWebAPI.Controllers
{
    public class ValuesController : ApiController
    {   
        //This route can be accessed by anyone with any scope.
        [Authorize]
        public HttpResponseMessage Get(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, id);
        }

        //This role can only be accessed by tokens containing the scope "Administrator".
        [Authorize(Roles = "Administrator")]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Here's the keys to the castle, boss!");
        }
    }
}