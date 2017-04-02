namespace WebApiOwinMiddleware.OwinMiddlewares
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.Owin;

    using AcceptRequestFunc = System.Func<Microsoft.Owin.IHeaderDictionary, bool>;

    public class HeaderFilterMiddleware : OwinMiddleware
    {
        private const string Html404 = "<!doctype html><html><head><meta charset=\"utf-8\"><title>404 Not Found</title></head><body>The resource cannot be found. Incorrect header.</body></html>";

        private readonly AcceptRequestFunc acceptRequestFunc;

        public HeaderFilterMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public HeaderFilterMiddleware(OwinMiddleware next, AcceptRequestFunc acceptRequest)
            : this(next)
        {
            this.acceptRequestFunc = acceptRequest;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var request = context.Request;
            var response = context.Response;
            var headers = context.Request.Headers;
            if (!this.acceptRequestFunc(headers))
            {

                var responseStream = response.Body;
                var responseBytes = Encoding.UTF8.GetBytes(Html404);

                response.Headers.Set("Content-Type", "text/html");
                response.Headers.Set("Content-Length", responseBytes.Length.ToString(CultureInfo.InvariantCulture));
                response.StatusCode = 404;

                await responseStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                return;
            }

            await this.Next.Invoke(context);
        }
    }
}