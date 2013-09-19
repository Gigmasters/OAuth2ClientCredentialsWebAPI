##Secured Web API

This is a rough scaffold of a .NET Web API secured by OAuth2 (using DotNetOpenAuth), specifically focusing on the client_credentials authentication grant. In this example, the WebAPI is acting as both the authorization server *and* the resource server.

####Prerequesites
* .NET 4.0 (no async/await)
* DotNetOpenAuth 4.3 (installed via package restore)

###How to use
```csharp
//TODO: lol you expected documentation, joke's on you, i should probably write documentation. i promise i'll do that soon. @alexdancho
```

###Security Note
This isn't fully implemented - it's simply a starting place for you to get an idea of how all the parts of OAuth and Client Creds fit together. If you're going to be actually implementing a secured Web API, please consider reading up on DotNetOpenAuth security (easy places to start would be disabling the "RelaxSSL" setting in web.config, implementing a nonce store, and using certificates signed by a CA as opposed to self-generated certificates.)

###Other resources
```csharp
//TODO: omg links on linx on lynx
```