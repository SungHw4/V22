using System.Text;
using CloudStructures.Structures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using V22WebServer.Service.Redis;

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
            var AuthToken = (string)obj["AuthToken"];
            if(string.IsNullOrEmpty(userID))
            {
                return;
            }
            else if(string.IsNullOrEmpty(AuthToken))
            {
                return;
            }
            var result = new RedisString<string>(Redis._redisConn, userID, null);
            var RedisToken = await result.GetAsync();
            if (RedisToken.ToString() != AuthToken)
            {
                Console.WriteLine("Diff Token");
                Console.WriteLine(AuthToken);
                Console.WriteLine(RedisToken.ToString());
                return;
            }
            
            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));

        }
        await _next(context);
    }
}