using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CorrelationId.Abstractions;
using TraceService.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TraceService.WebAPI.Middlewares
{
    public class SerilogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SerilogMiddleware> _logger;
        private readonly IFilterLogService _filterLogService;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public SerilogMiddleware(RequestDelegate next, ILogger<SerilogMiddleware> logger,
            IFilterLogService filterLogService, ICorrelationContextAccessor correlationContextAccessor)
        {
            this._next = next;
            this._logger = logger;
            this._filterLogService = filterLogService;
            this._correlationContextAccessor = correlationContextAccessor;
        }

        public async Task Invoke(HttpContext context)
        {
            string correlationId = this._correlationContextAccessor.CorrelationContext.CorrelationId;
            //First, get the incoming request
            string request = await FormatRequest(context.Request, this._filterLogService, correlationId);

            this._logger.LogInformation(request);

            //Copy a pointer to the original response body stream
            Stream originalBodyStream = context.Response.Body;

            //Create a new memory stream...
            using MemoryStream responseBody = new();
            //...and use that for the temporary response body
            context.Response.Body = responseBody;

            //Continue down the Middleware pipeline, eventually returning to this class
            await this._next(context);

            //Format the response from the server
            string response = await FormatResponse(context.Response, correlationId);
            this._logger.LogInformation(response);

            //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
            await responseBody.CopyToAsync(originalBodyStream);
        }

        private static async Task<string> FormatRequest(HttpRequest request, IFilterLogService filterLogService, string correlationId)
        {
            //This line allows us to set the reader for the request back at the beginning of its stream.
            request.EnableBuffering();

            Stream body = request.Body;

            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            byte[] buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //...Then we copy the entire request stream into the new buffer.
            await request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length)).ConfigureAwait(false);

            //We convert the byte[] into a string using UTF8 encoding...
            string bodyAsText = Encoding.UTF8.GetString(buffer);

            string filteredBodyAsText = filterLogService.Filter(bodyAsText);


            body.Seek(0, SeekOrigin.Begin);
            //..and finally, assign the read body back to the request body, which is allowed because of EnableBuffering()
            request.Body = body;

            return $"[{correlationId}] {request.Scheme} {request.Host}{request.Path} {request.QueryString} {filteredBodyAsText}";
        }

        private static async Task<string> FormatResponse(HttpResponse response, string correlationId)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return $"[{correlationId}]{response.StatusCode}: {text}";
        }
    }
}
