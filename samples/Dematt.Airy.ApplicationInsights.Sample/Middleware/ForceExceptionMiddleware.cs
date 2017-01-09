using System;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Dematt.Airy.ApplicationInsights.Sample.Middleware
{
    public class ForceExceptionMiddleware : OwinMiddleware
    {
        /// <summary>
        /// The only constructor for this class.
        /// The owin pipeline will pass the next parameter automatically.
        /// </summary>
        public ForceExceptionMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (context.Request.Path.Value.Contains("owinerror"))
            {
                context.Response.StatusCode = 500;
                throw new ApplicationException();
            }
            await Next.Invoke(context);
        }
    }
}
