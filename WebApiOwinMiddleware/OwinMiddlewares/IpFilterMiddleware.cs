namespace WebApiOwinMiddleware.OwinMiddlewares
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    public class IpFilterMiddleware
    {
        private const string Html404 = "<!doctype html><html><head><meta charset=\"utf-8\"><title>404 Not Found</title></head><body>The resource cannot be found.</body></html>";

        private readonly AppFunc nextMiddleware;

        private readonly Func<IPAddress, bool> rejectRequest;

        public IpFilterMiddleware(AppFunc nextMiddleware, Func<IPAddress, bool> rejectRequest)
        {
            if (nextMiddleware == null)
            {
                throw new ArgumentNullException("nextMiddleware");
            }

            this.nextMiddleware = nextMiddleware;
            this.rejectRequest = rejectRequest;
        }

        public Task Invoke(IDictionary<string, object> environment)
        {
            var remoteAddress = IPAddress.Parse((string)environment["server.RemoteIpAddress"]).MapToIPv4();

            if (this.rejectRequest(remoteAddress))
            {
                var responseStream = (Stream)environment["owin.ResponseBody"];
                var responseHeaders =
                    (IDictionary<string, string[]>)environment["owin.ResponseHeaders"];

                var responseBytes = Encoding.UTF8.GetBytes(Html404);

                responseHeaders["Content-Type"] = new[] { "text/html" };
                responseHeaders["Content-Length"] = new[] { responseBytes.Length.ToString(CultureInfo.InvariantCulture) };

                environment["owin.ResponseStatusCode"] = 404;

                return responseStream.WriteAsync(responseBytes, 0, responseBytes.Length);
            }

            return this.nextMiddleware(environment);
        }
    }
}