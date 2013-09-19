using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using DotNetOpenAuth.OAuth2;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2.Messages;

namespace OpenAutoClientCredsWebAPI.Infrastructure
{


    /// <summary>
    /// An HTTP server message handler that detects OAuth 2 bearer tokens in the authorization header
    /// and applies the appropriate principal to the request when found.
    /// </summary>
    public class BearerTokenHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization != null)
            {
                if (request.Headers.Authorization.Scheme == "Bearer")
                {
                    //If there are authorization headers that use the Bearer token scheme, try to digest the token and, if available,
                    //set the current user context.
                    try
                    {
                        var AuthServerPublicKey = (RSACryptoServiceProvider)AuthorizationServerHost.AuthorizationServerCertificate.PublicKey.Key;
                        var ResourceServerPrivateKey = (RSACryptoServiceProvider)AuthorizationServerHost.ResourceServerCertificate.PrivateKey;
                        var resourceServer = new ResourceServer(new StandardAccessTokenAnalyzer(AuthServerPublicKey, ResourceServerPrivateKey));
                        var principal = resourceServer.GetPrincipal(request);
                        HttpContext.Current.User = principal;
                        Thread.CurrentPrincipal = principal;
                    }
                    catch (ProtocolFaultResponseException ex) 
                    {
                        //A protocol fault response exception is thrown in the case that a client cannot be properly authenticated by the provided
                        //token. To diagnose any errors you may be getting, you can re-throw or log ex.ErrorResponseMessage.
                        //For now, let's just inform the client that they're unauthorized for this resource.
                        SendUnauthorizedResponse();
                    }
                }
            }
            else
            {
                //This API does not allow unauthenticated access, return an unauthorized status code.
                SendUnauthorizedResponse();
            }
            return base.SendAsync(request, cancellationToken);
        }

        private Task<HttpResponseMessage> SendUnauthorizedResponse()
        {
            return Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                return response;
            });
        }
    }
}