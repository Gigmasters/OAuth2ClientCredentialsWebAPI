REM Create a certificate authority and revocation list for that authority.
makecert -n "CN=DNOA_WebAPI_Demo" -pe -r -sv DNOA_WebAPI_Demo.pvk DNOA_WebAPI_Demo.cer
makecert -crl -n "CN=DA_DNOA_DEMO" -r -sv DNOA_WebAPI_Demo.pvk ca_dnoa.crl

REM Create certificates for the resource server and authorization server.
makecert -iv DNOA_WebAPI_Demo.pvk -n "CN=DNOA_WebAPI_Demo_STS" -ic DNOA_WebAPI_Demo.cer -sv DNOA_WebAPI_Demo_STS.pvk DNOA_WebAPI_Demo_STS.cer -sky exchange -pe -a sha1
makecert -iv DNOA_WebAPI_Demo.pvk -n "CN=DNOA_WebAPI_Demo_Service" -ic DNOA_WebAPI_Demo.cer -sv DNOA_WebAPI_Demo_Service.pvk DNOA_WebAPI_Demo_Service.cer -sky exchange -pe -a sha1

REM Create installable PFX files for those two certificates.
pvk2pfx -pvk DNOA_WebAPI_Demo_STS.pvk -spc DNOA_WebAPI_Demo_STS.cer -pfx DNOA_WebAPI_Demo_STS.pfx -po yourpasswordhere
pvk2pfx -pvk DNOA_WebAPI_Demo_Service.pvk -spc DNOA_WebAPI_Demo_Service.cer -pfx DNOA_WebAPI_Demo_Service.pfx -po yourpasswordhere