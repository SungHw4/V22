namespace V22WebServer.MiddleWare;

public static class MiddleWareExtention
{
    public static IApplicationBuilder UserSessionMiddleWare(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserSessionMiddleWare>();
    }
    
}