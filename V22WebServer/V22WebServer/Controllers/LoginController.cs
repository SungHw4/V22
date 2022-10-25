using Dapper;
using CloudStructures.Structures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using ZLogger;
using V22WebServer.Service.Database;
using V22WebServer.Service.Redis;

namespace V22WebServer.Controllers;

[ApiController]
[Route("[controller]")]

public class LoginController : Controller
{
    readonly ILogger Logger;
    private IMemoryCache MemoryCache;

    public LoginController(ILogger<LoginController> logger)
    {
        Logger = logger;
        //MemoryCache = memoryCache;
    }

    [HttpPost]
    public async Task<PkLoginResponse> Post(PkLoginRequest request)
    {
        Logger.ZLogInformation($"[Request Login] ID:{request.ID}, PW:{request.PW}");
        
        var response = new PkLoginResponse();
        response.Result = ErrorCode.None;
        
        string tokenValue = Database.AuthToken();
        var idDefaultExpiry = TimeSpan.FromDays(1);
        var redisId = new RedisString<string>(Redis._redisConn, request.ID, idDefaultExpiry);
        await redisId.SetAsync(tokenValue);

        response.AuthToken = tokenValue;
        return response;
    }
}

public class PkLoginRequest
{
    public string ID { get; set; }
    public string PW { get; set; }
}
public class PkLoginResponse
{
    public ErrorCode Result { get; set; }
    public string AuthToken { get; set; }
}

class DBUserInfo
{
    public UInt64 UID { get; set; }
    public string PW { get; set; }
    public string NickName { get; set; }
    public string Salt { get; set; }
}