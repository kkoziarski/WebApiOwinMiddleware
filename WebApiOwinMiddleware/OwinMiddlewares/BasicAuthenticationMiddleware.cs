namespace WebApiOwinMiddleware.OwinMiddlewares
{
    using System;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.Owin;

    public class BasicAuthenticationMiddleware : OwinMiddleware
    {
        public const string AuthMode = "Basic";

        private readonly Func<string, string, Task<IIdentity>> indentityVerificationCallback;

        public BasicAuthenticationMiddleware(OwinMiddleware next)
            : base(next)
        {}

        public BasicAuthenticationMiddleware(OwinMiddleware next, Func<string, string, Task<IIdentity>> validationCallback)
            : this(next)
        {
            this.indentityVerificationCallback = validationCallback;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var request = context.Request;
            var response = context.Response;

            response.OnSendingHeaders(state =>
            {
                var resp = (IOwinResponse)state;

                if (resp.StatusCode == 401)
                {
                    resp.Headers["WWW-Authenticate"] = AuthMode;
                }

                // resp.Headers.Set("X-MyResponse-Header", "Some Value");
                // resp.StatusCode = 403;
                // resp.ReasonPhrase = "Forbidden";
            }, response);

            var authorizationHeaderRaw = request.Headers["Authorization"];

            if (!string.IsNullOrWhiteSpace(authorizationHeaderRaw))
            {
                var authHeader = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(authorizationHeaderRaw);

                if (AuthMode.Equals(authHeader.Scheme, StringComparison.OrdinalIgnoreCase))
                {
                    var parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter));
                    var parts = parameter.Split(':');

                    var userName = parts[0];
                    var password = parts[1];

                    if (this.indentityVerificationCallback != null)
                    {
                        var identity = await this.indentityVerificationCallback(userName, password);
                        if (identity != null)
                        {
                            request.User = new ClaimsPrincipal(identity);
                        }
                    }
                    /*
                    if (userName == password) // Just a dumb check
                    {
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, userName),
                            new Claim(ClaimTypes.Email, "kk@kk.com")
                        };

                        var identity = new ClaimsIdentity(claims, AuthMode);

                        request.User = new ClaimsPrincipal(identity);
                    } 
                    */
                }
            }

            await this.Next.Invoke(context);
        }
    }
}