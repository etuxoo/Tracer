using System.Threading.Tasks;
using TraceService.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace TraceService.WebAPI.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        private const string API_KEY = "Api-Key";
        private const string API_SECRET = "Api-Secret";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(API_KEY, out Microsoft.Extensions.Primitives.StringValues extractedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Api Key was not provided. (Using ApiKeyMiddleware) ");
                return;
            }

            if (!context.Request.Headers.TryGetValue(API_SECRET, out Microsoft.Extensions.Primitives.StringValues extractedApiSecret))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Api Secret was not provided. (Using ApiKeyMiddleware) ");
                return;
            }

            IApiKeyService аpiKeyService = context.RequestServices.GetRequiredService<IApiKeyService>();
            bool authorizeResult = await аpiKeyService.AuthorizeAsync(extractedApiKey, extractedApiSecret);

            if (!authorizeResult)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized client. (Using ApiKeyMiddleware)");
                return;
            }
            await this._next(context);
        }
    }
}
