using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Security;
using DotNetOpenAuth.Messaging.Bindings;
using DotNetOpenAuth.OAuth2;
using DotNetOpenAuth.OAuth2.ChannelElements;
using DotNetOpenAuth.OAuth2.Messages;

namespace OpenAutoClientCredsWebAPI.Infrastructure
{
    public class AuthorizationServerHost : IAuthorizationServerHost
    {

        /// Let's assign some private values to internal static properties - consider implementing IoC to use numerous certificates, crypto/nonce key stores, etc.
        internal static ICryptoKeyStore _cryptoKeyStore = new InMemoryCryptoKeyStore();

        /// Create private static fields to store our two server certificates/
        private static X509Certificate2 _authorizationServerCertificate;
        private static X509Certificate2 _resourceServerCertificate;

        /// <summary>
        /// Consider replacing the InMemoryCryptoKeyStore with a database or caching-layer bound implementation.
        /// </summary>
		public ICryptoKeyStore CryptoKeyStore 
        {
            get
            {
                return _cryptoKeyStore;
            }
		}

		/// <summary>
		/// For basic client_credential grants, this does not need to be implemented.
		/// </summary>
		public INonceStore NonceStore 
        {
			get {
                return null;
			}
		}

        /// Facades over the private auth server certificate, and loads a certificate 
        /// from the Windows Certificate Store if one has not already been loaded.
        public static X509Certificate2 AuthorizationServerCertificate
        {
            get
            {
                if (_authorizationServerCertificate == null)
                {
                    _authorizationServerCertificate = LoadCert(ConfigurationManager.AppSettings["AuthorizationServerCertificateThumbprint"]);
                }
                return _authorizationServerCertificate;
            }
        }

        /// Facades over the private resource server certificate, and loads a certificate 
        /// from the Windows Certificate Store if one has not already been loaded.
        public static X509Certificate2 ResourceServerCertificate
        {
            get
            {
                if (_resourceServerCertificate== null)
                {
                    _resourceServerCertificate = LoadCert(ConfigurationManager.AppSettings["ResourceServerCertificateThumbprint"]);
                }
                return _resourceServerCertificate;
            }
        }

        public AutomatedAuthorizationCheckResponse CheckAuthorizeClientCredentialsGrant(IAccessTokenRequest accessRequest)
        {
            if (accessRequest.ClientAuthenticated)
            {
                //If the client request is properly authenticated (in this case, the client_secret properly matches the client secret
                //set up in GetClient()), then we can provide an access token.
                return new AutomatedAuthorizationCheckResponse(accessRequest, true);
            }
            else
            {
                // Only authenticated clients should be given access.
                return new AutomatedAuthorizationCheckResponse(accessRequest, false);
            }
        }

        public AutomatedUserAuthorizationCheckResponse CheckAuthorizeResourceOwnerCredentialGrant(string userName, string password, IAccessTokenRequest accessRequest)
        {
            //We're not demonstrating a RO Credential grant in this exercise, so let's not allow it at all.
                return new AutomatedUserAuthorizationCheckResponse(accessRequest, false, null);
        }

        public AccessTokenResult CreateAccessToken(IAccessTokenRequest accessTokenRequestMessage)
        {
            //Default to a lifespan of one hours for all tokens.
            var accessToken = new AuthorizationServerAccessToken
            {
                Lifetime = TimeSpan.FromHours(1),
            };

            //Provide both signing and encryption keys for the token (the private/public relationships will be reversed
            //when the resource server digests the token
            accessToken.AccessTokenSigningKey =
                     (RSACryptoServiceProvider)AuthorizationServerCertificate.PrivateKey;
            accessToken.ResourceServerEncryptionKey =
                     (RSACryptoServiceProvider)ResourceServerCertificate.PublicKey.Key;

            //Return an access token result.
            var result = new AccessTokenResult(accessToken);
            return result;
        }

        public IClientDescription GetClient(string clientIdentifier)
        {
            //This is where you'd generally query against a table of clients for a client matching this ID, and return a description
            //of what kind of client they are, what their secret key is, and what their callback URI is.

            //For the purposes of this exercise, let's just a return a client with a hardcoded secret key.
            return new ClientDescription("secretsecretsarenofun", new Uri("http://localbroast.com/"), ClientType.Public);
        }

        public bool IsAuthorizationValid(IAuthorizationDescription authorization)
        {
            //TODO: Maybe put some code here. This is mainly used for revocation of authorization grants - if
            //need to do it, we can, but for now assume that we don't need to do any revocation beyond expiry.
            return true;
        }

        public bool CanBeAutoApproved(EndUserAuthorizationRequest authorizationRequest)
        {
            //For the sake of this exercise, don't auto-approve anyone.
            return false;
        }

        /// <summary>
        /// Used to load certificates from the certificate store.
        /// </summary>
        /// <param name="thumbprint">Thumbprint of the certificate you're trying to load.</param>
        /// <returns>An X509Certificate.</returns>
        private static X509Certificate2 LoadCert(string thumbprint)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var certs = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

            if (certs.Count == 0)
            {
                throw new Exception("Could not find cert with thumbprint " + thumbprint);
            }
            var cert = certs[0];
            return cert;
        }
    }
}