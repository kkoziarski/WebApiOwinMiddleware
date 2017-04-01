namespace WebApiOwinMiddleware.OwinMiddlewares
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Owin;

    public class SimpleBasicAuthenticationMiddleware : OwinMiddleware
    {
        private const string AuthMode = "Basic";

        public SimpleBasicAuthenticationMiddleware(OwinMiddleware next) : base(next)
        { }

        public override async Task Invoke(IOwinContext context)
        {
            var response = context.Response;
            var request = context.Request;

            response.OnSendingHeaders(state =>
            {
                var resp = (OwinResponse)state;

                if (resp.StatusCode == 401)
                {
                    resp.Headers.Set("WWW-Authenticate", AuthMode);
                }
            }, response);

            var authorizationHeaderRaw = context.Request.Headers["Authorization"];
//            response.OnSendingHeaders(state =>
//            {
//                var resp = (OwinResponse)state;
//                resp.Headers.Set("X-MyResponse-Header", "Some Value");
//                resp.StatusCode = 403;
//                resp.ReasonPhrase = "Forbidden";
//            }, response);

            if (!String.IsNullOrWhiteSpace(authorizationHeaderRaw))
            {
                var authHeader = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(authorizationHeaderRaw);

                if (AuthMode.Equals(authHeader.Scheme, StringComparison.OrdinalIgnoreCase))
                {
                    string parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter));
                    var parts = parameter.Split(':');

                    string userName = parts[0];
                    string password = parts[1];

                    if (userName == password) // Just a dumb check
                    {
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, userName)
                        };

                        var identity = new ClaimsIdentity(claims, AuthMode);

                        request.User = new ClaimsPrincipal(identity);
                    }
                }
            }

            await this.Next.Invoke(context);
        }

        private void SingIn(IOwinContext context)
        {
            // user login
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "kk"));
            claims.Add(new Claim(ClaimTypes.Email, "kk@kk.com"));

            var id = new ClaimsIdentity(claims, "cookie");

            context.Authentication.SignIn(id);
        }
    }
}