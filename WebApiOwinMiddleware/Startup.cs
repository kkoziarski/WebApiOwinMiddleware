using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WebApiOwinMiddleware.Startup))]

namespace WebApiOwinMiddleware
{
    using System.Web.Http;

    using WebApiOwinMiddleware.Extensions;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfiguration = new HttpConfiguration();


            app.UseIpFiltering(remoteAddress =>
            {
                var bytes = remoteAddress.GetAddressBytes();
                bool addressToReject = bytes[0] != 192 && bytes[0] != 172 && bytes[0] != 10 && bytes[0] != 127 && bytes[0] != 0;
                return addressToReject;
            });

            app.UseHeaderFiltering(headers =>
            {
                string[] headerValues;
                bool hasHeader = headers.TryGetValue("X-my-sample-header", out headerValues);
                return !(hasHeader && headerValues != null && headerValues.Length > 0);
            });

            // Configure Web API Routes:
            // - Enable Attribute Mapping
            // - Enable Default routes at /api.
            httpConfiguration.MapHttpAttributeRoutes();
            httpConfiguration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseWebApi(httpConfiguration);
        }
    }
}
