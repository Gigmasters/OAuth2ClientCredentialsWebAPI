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
    public class CryptoKeyStoreEntry
    {
        public string Bucket { get; set; }
        public string Handle { get; set; }
        public CryptoKey Key { get; set; }
    }
}