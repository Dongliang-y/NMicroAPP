using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPlatform.UI.Middleware
{
    public class RedirectToHttpsRule : IRule
    {
        private const string HEADER_HAME = "X-Forwarded-Proto";

        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;

            if (request.Headers.TryGetValue(HEADER_HAME, out var forwardedProto))
            {
                if (forwardedProto.ToString() == "http")
                {
                    var isHttpGet = request.Method.Equals("get", StringComparison.OrdinalIgnoreCase);
                    var statusCode = isHttpGet ? StatusCodes.Status301MovedPermanently : StatusCodes.Status307TemporaryRedirect;

                    var host = context.HttpContext.Request.Host;
                    var newUrl = new StringBuilder()
                        .Append("https://")
                        .Append(host)
                        .Append(request.PathBase)
                        .Append(request.Path)
                        .Append(request.QueryString);

                    var response = context.HttpContext.Response;
                    response.StatusCode = statusCode;
                    response.Headers[HeaderNames.Location] = newUrl.ToString();
                    context.Result = RuleResult.EndResponse;
                }
            }
        }
    }
}
