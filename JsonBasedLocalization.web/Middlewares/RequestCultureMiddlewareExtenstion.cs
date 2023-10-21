namespace JsonBasedLocalization.web.Middlewares;

public static class RequestCultureMiddlewareExtenstion
{
    public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder builder) 
    {
        return builder.UseMiddleware<RequestCultureMiddleware>();
    }
}
