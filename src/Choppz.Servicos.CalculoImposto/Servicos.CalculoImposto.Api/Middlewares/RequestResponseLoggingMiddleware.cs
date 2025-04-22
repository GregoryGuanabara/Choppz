using Serilog;
using System.Text;

namespace Servicos.CalculoImposto.Api.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;

            if (path.Contains("/openapi", StringComparison.OrdinalIgnoreCase) ||
                path.Contains("/scalar", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            context.Request.EnableBuffering();
            var requestBody = await ReadRequestBodyAsync(context.Request);
            Log.Information("Incoming Request: {Method} {Path} - Body: {Body}",
                context.Request.Method, context.Request.Path, requestBody);

            var originalBodyStream = context.Response.Body;
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            var responseBody = await ReadResponseBodyAsync(context.Response);
            Log.Information("Outgoing Response: {StatusCode} - Body: {Body}",
                context.Response.StatusCode, responseBody);

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalBodyStream);
        }

        private static async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            request.Body.Seek(0, SeekOrigin.Begin);
            var bodyAsText = await new StreamReader(request.Body, Encoding.UTF8).ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);
            return bodyAsText;
        }

        private static async Task<string> ReadResponseBodyAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var bodyAsText = await new StreamReader(response.Body, Encoding.UTF8).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return bodyAsText;
        }
    }
}