using System.Diagnostics;

namespace Restaurants.API.Middlewares;

public class RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var watch = Stopwatch.StartNew();
        await next.Invoke(context);
        watch.Stop();
        if (watch.ElapsedMilliseconds > 4000)
            logger.LogWarning(
                $"The request [{context.Request.Method}] {context.Request.Path} took {watch.ElapsedMilliseconds / 1000} seconds.");
    }
}