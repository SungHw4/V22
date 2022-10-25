using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZLogger;

namespace V22WebServer.MiddleWare;

public class UserSessionMiddleWare
{
    //ILogger Logger;
    private readonly RequestDelegate _next;

    public UserSessionMiddleWare(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if(context.Request.Path != "/Login" && context.Request.Path != "/CreateAccount")
        {
            StreamReader bodystream = new StreamReader(context.Request.Body, Encoding.UTF8);
            string body = bodystream.ReadToEndAsync().Result;

            var obj = (JObject)JsonConvert.DeserializeObject(body);

            var userID = (string)obj["ID"];
            if(string.IsNullOrEmpty(userID))
            {
                return;
            }

            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));

        }
        await _next(context);
    }
}