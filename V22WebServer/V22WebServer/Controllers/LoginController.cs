using Dapper;
using CloudStructures.Structures;
using CsvHelper.Expressions;
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

        using (var connection = await Database.GetAccountDbConnection())
        {
            try
            {
                var user = await connection.QueryFirstOrDefaultAsync<DBUserInfo>(@"SELECT ID,PW,NickName,Salt FROM users where ID=@ID",
                    new {ID = request.ID});
                if (user.PW == Database.MakeHashingPassWord(user.Salt, request.PW))
                {
                    string tokenValue = Database.AuthToken();
                    var idDefaultExpiry = TimeSpan.FromDays(1);
                    var redisId = new RedisString<string>(Redis._redisConn, request.ID, idDefaultExpiry);
                    await redisId.SetAsync(tokenValue);
        
                    response.AuthToken = tokenValue;
                    
                    user.AuthToken = tokenValue;
                    user.Last_Conn = DateTime.Now;
                    var count = await connection.ExecuteAsync(
                        "UPDATE users SET AuthToken = @AuthToken, Last_Conn = @Last_Conn WHERE ID = @ID",
                        user);
                    if (count != 1)
                    {
                        response.Result = ErrorCode.Login_Fail_Update;
                        
                    }

                    return response;
                }
                else
                {
                    response.Result = ErrorCode.Login_Fail_PW;
                    return response;
                }
                
                //var count = await connection.ExecuteAsync(
                  //  @"UPDATE Users SET AuthToken = @AuthToken, Last_Conn = LastToken WHERE ID = @ID, PW = @PW", userInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }
        
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
    public string ID { get; set; }
    public string PW { get; set; }
    public string NickName { get; set; }
    public string Salt { get; set; }
    public DateTime Last_Conn { get; set; }
    public string AuthToken { get; set; }
}