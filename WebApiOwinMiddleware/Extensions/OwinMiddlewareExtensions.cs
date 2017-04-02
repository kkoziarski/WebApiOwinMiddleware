namespace WebApiOwinMiddleware.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Security.Principal;
    using System.Threading.Tasks;

    using Microsoft.Owin.Extensions;

    using Owin;

    using WebApiOwinMiddleware.OwinMiddlewares;

    public static class OwinMiddlewareExtensions
    {
        public static IAppBuilder UseIpFiltering(this IAppBuilder appBuilder, Func<IPAddress, bool> rejectRequest)
        {
            appBuilder.Use(typeof(IpFilterMiddleware), rejectRequest);
            return appBuilder;
        }

        public static IAppBuilder UseHeaderFiltering(this IAppBuilder appBuilder, Func<Microsoft.Owin.IHeaderDictionary, bool> rejectRequest)
        {
            appBuilder.Use(typeof(HeaderFilterMiddleware), rejectRequest);
            return appBuilder;
        }

        public static IAppBuilder UseBasicAuthentication(this IAppBuilder app, Func<string /* username */, string /* password */, Task<IIdentity>> validationCallback)
        {
            app.Use<BasicAuthenticationMiddleware>(validationCallback);
            return app.UseStageMarker(PipelineStage.Authenticate);
        }

        //public static IAppBuilder UseSimpleBasicAuthentication(this IAppBuilder appBuilder)
        //{
        //    appBuilder.Use(typeof(SimpleBasicAuthenticationMiddleware));
        //    appBuilder.UseStageMarker(PipelineStage.Authenticate);
        //    return appBuilder;
        //}
    }
}