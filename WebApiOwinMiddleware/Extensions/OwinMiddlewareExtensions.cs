namespace WebApiOwinMiddleware.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Net;

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

        public static IAppBuilder UseHeaderFiltering(this IAppBuilder appBuilder, Func<IDictionary<string, string[]>, bool> rejectRequest)
        {
            appBuilder.Use(typeof(HeaderFilterMiddleware), rejectRequest);
            return appBuilder;
        }

        public static IAppBuilder UseSimpleBasicAuthentication(this IAppBuilder appBuilder)
        {
            appBuilder.Use(typeof(SimpleBasicAuthenticationMiddleware));
            appBuilder.UseStageMarker(PipelineStage.Authenticate);
            return appBuilder;
        }
    }
}