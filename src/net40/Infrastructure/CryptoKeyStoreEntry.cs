using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.Messaging.Bindings;
using DotNetOpenAuth.OAuth2;
using DotNetOpenAuth.OAuth2.ChannelElements;
using DotNetOpenAuth.OAuth2.Messages;

namespace OpenAutoClientCredsWebAPI.Infrastructure
{
    /// <summary>
    /// This is an incredibly lightweight implementation of the object that will be stored in the ICryptoKeyStore implementation.
    /// </summary>
    public class CryptoKeyStoreEntry
    {
        public string Bucket { get; set; }
        public string Handle { get; set; }
        public CryptoKey Key { get; set; }
    }
}