# Web API with Owin and middlewares

Exmaple web app with WebAPI, Owin and middlewares, e.g. **OWIN Basic Authentication**

### Filtering Middlewares
* [HeaderFilterMiddleware](\WebApiOwinMiddleware\OwinMiddlewares\HeaderFilterMiddleware.cs) - requires `X-my-sample-header` HTTP header to be preset in every request, configured in `Startup -> app.UseHeaderFiltering(...)`
* [IpFilterMiddleware](\WebApiOwinMiddleware\OwinMiddlewares\IpFilterMiddleware.cs) - disabled, configured in `Startup -> app.UseIpFiltering(...)`

### Authentication Middleware
* [BasicAuthenticationMiddleware](WebApiOwinMiddleware\OwinMiddlewares\BasicAuthenticationMiddleware.cs) - 
A resource that is protected by basic authentication - `[Authorize]` attribute - requires incoming requests to include the Authorization HTTP header using the basic scheme. This scheme uses a base64 encoded username and password separated by a colon (base64 encoding is used to avoid characters that would cause issues when sent over HTTP). [OWIN Basic Authentication][OWIN Basic Authentication]

```
Plain text
Authorization: Basic username:password

Encoded
Authorization: Basic dXNlcm5hbWU6cGFzc3dvcmQ=
```

## Resources:
* [IP filtering middleware](http://southworks.com/blog/2013/07/17/intro-to-owin-talk-and-a-simple-ip-filtering-middleware-sample)
* [Basic Authentication Middleware](https://lbadri.wordpress.com/2013/07/13/basic-authentication-with-asp-net-web-api-using-owin-middleware/)
* [OWIN Basic Authentication][OWIN Basic Authentication]


[OWIN Basic Authentication]:https://www.scottbrady91.com/Katana/OWIN-Basic-Authentication