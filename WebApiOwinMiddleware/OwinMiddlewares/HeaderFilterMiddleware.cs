namespace WebApiOwinMiddleware.OwinMiddlewares
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    public class HeaderFilterMiddleware
    {
        private const string Html404 = "<!doctype html><html><head><meta charset=\"utf-8\"><title>404 Not Found</title></head><body>The resource cannot be found. Incorrect header.</body></html>";

        private readonly AppFunc nextMiddleware;

        private readonly Func<IDictionary<string, string[]>, bool> rejectRequest;

        public HeaderFilterMiddleware(AppFunc nextMiddleware, Func<IDictionary<string, string[]>, bool> rejectRequest)
        {
            if (nextMiddleware == null)
            {
                throw new ArgumentNullException("nextMiddleware");
            }

            this.nextMiddleware = nextMiddleware;
            this.rejectRequest = rejectRequest;
        }

        public Task Invoke(IDictionary<string, object> requestContext)
        {
            var headers = (IDictionary<string, string[]>)requestContext["owin.RequestHeaders"];
            if (this.rejectRequest(headers))
            {
                var responseStream = (Stream)requestContext["owin.ResponseBody"];
                var responseHeaders = (IDictionary<string, string[]>)requestContext["owin.ResponseHeaders"];


                var responseBytes = Encoding.UTF8.GetBytes(Html404);

                responseHeaders["Content-Type"] = new[] { "text/html" };
                responseHeaders["Content-Length"] = new[] { responseBytes.Length.ToString(CultureInfo.InvariantCulture) };

                requestContext["owin.ResponseStatusCode"] = 404;

                return responseStream.WriteAsync(responseBytes, 0, responseBytes.Length);
            }

            return this.nextMiddleware(requestContext);
        }
    }
}