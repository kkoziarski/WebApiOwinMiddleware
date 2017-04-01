namespace WebApiOwinMiddleware.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Owin;

    using WebApiOwinMiddleware.OwinMiddlewares;

    public static class OwinMiddlewareExtensions
    {
        public static IAppBuilder UseIpFiltering(this IAppBuilder appBuilder, Func<IPAddress, bool> rejectRequest)
        {
            appBuilder.Use(typeof(IpFilterMiddleware), rejectRequest);
            return appBuilder;
        }
    }
}