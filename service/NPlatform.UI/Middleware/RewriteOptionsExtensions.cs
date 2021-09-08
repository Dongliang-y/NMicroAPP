using Microsoft.AspNetCore.Rewrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPlatform.UI.Middleware
{
    public static class RewriteOptionsExtensions
    {
        public static RewriteOptions AddCnblogsRedirectToHttps(this RewriteOptions options)
        {
            options.Rules.Add(new RedirectToHttpsRule());
            return options;
        }
    }
}
