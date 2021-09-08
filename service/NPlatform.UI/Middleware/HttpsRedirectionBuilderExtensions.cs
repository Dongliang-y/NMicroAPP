using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPlatform.UI.Middleware
{
    public static class HttpsRedirectionBuilderExtensions
    {
        public static IApplicationBuilder UseCnblogsHttpsRedirection(this IApplicationBuilder app)
        {
            IApplicationBuilder applicationBuilder = app.UseRewriter(new RewriteOptions().AddCnblogsRedirectToHttps());
            return app;
        }
    }
}
