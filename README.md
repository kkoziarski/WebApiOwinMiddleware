# Web API with Owin and middlewares

Exmaple web app with WebAPI, Owin and middlewares, e.g. **OWIN Basic Authentication**

## Middlewares
### Filtering Middlewares
* [HeaderFilterMiddleware](\WebApiOwinMiddleware\OwinMiddlewares\HeaderFilterMiddleware.cs) - requires an configured HTTP header (e.g. `X-my-sample-header`) to be preset in every request configured in `Startup -> app.UseHeaderFiltering(...)`. 

    Configuration in `web.config`
    ```
    AppSettings["TokenHeaderName"]: the required header name
    AppSettings["TokenHeaderValue"]: the required header's value
    AppSettings["TokenHeaderFilteringEnabled"]: enable/disable header filtering. You can disable it for debug and enable for release.
    ```

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

Configuration in `web.config`

```
AppSettings["ApiUserName"]: user name for authentication
AppSettings["ApiPassword"]: password for authentication
```

## REST Api
Example of using correct HTTP method in API:
* `GET` - get single or all
* `POST` - create or create lazy. Return `Location` header with URL to newly created object (status code: 201 Created) or where the object will be created when cannot be created immediatelly - the lazy option (status code: 202 Accepted) with `Location` header
* `PUT` - update an object
* `PATCH` - update only one property
* `HEAD` - check if an object exists without returing data 
* `DELETE` - delete an object

## Database
Database used is [LiteDB](http://www.litedb.org/) - Embedded NoSQL database for .NET, stored in a single file.
The database is initially setup in `DatabaseSetup.cs`


## Resources:
* [IP filtering middleware](http://southworks.com/blog/2013/07/17/intro-to-owin-talk-and-a-simple-ip-filtering-middleware-sample)
* [Basic Authentication Middleware](https://lbadri.wordpress.com/2013/07/13/basic-authentication-with-asp-net-web-api-using-owin-middleware/)
* [OWIN Basic Authentication][OWIN Basic Authentication]
* [Choosing an HTTP Status Code](http://racksburg.com/choosing-an-http-status-code/)

[OWIN Basic Authentication]:https://www.scottbrady91.com/Katana/OWIN-Basic-Authentication