using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using DotNetOpenAuth.Messaging.Bindings;
using DotNetOpenAuth.OAuth2;
using DotNetOpenAuth.OAuth2.ChannelElements;
using DotNetOpenAuth.OAuth2.Messages;


namespace OpenAutoClientCredsWebAPI.Infrastructure
{
    public class InMemoryCryptoKeyStore : ICryptoKeyStore
    {

        public static List<CryptoKeyStoreEntry> keys = new List<CryptoKeyStoreEntry>();

        public CryptoKey GetKey(string bucket, string handle)
        {
            return keys.Where(k => k.Bucket == bucket && k.Handle == handle)
                                    .Select(k => k.Key)
                                    .FirstOrDefault();
        }

        public IEnumerable<KeyValuePair<string, CryptoKey>> GetKeys(string bucket)
        {
            return keys.Where(k => k.Bucket == bucket)
                                   .OrderByDescending(k => k.Key.ExpiresUtc)
                                   .Select(k => new KeyValuePair<string, CryptoKey>(k.Handle, k.Key));
        }

        public void RemoveKey(string bucket, string handle)
        {
            keys.RemoveAll(k => k.Bucket == bucket && k.Handle == handle);
        }

        public void StoreKey(string bucket, string handle, CryptoKey key)
        {
            var entry = new CryptoKeyStoreEntry();
            entry.Bucket = bucket;
            entry.Handle = handle;
            entry.Key = key;
            keys.Add(entry);
        }
    }
}