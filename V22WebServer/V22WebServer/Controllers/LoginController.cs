using Dapper;
using CloudStructures.Structures;
using Microsoft.AspNetCore.Mvc;
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
                var user = await connection.QueryFirstOrDefaultAsync<DBUserInfo>(@"SELECT ID,PW,NickName,Salt,Last_Conn,AuthToken FROM users where ID=@ID",
                    new {ID = request.ID});
                if(user.ID == null)
                {
                    response.Result = ErrorCode.Login_Fail_NotUser;
                    return response;
                }
                if (user.PW == Database.MakeHashingPassWord(user.Salt, request.PW))
                {
                    string tokenValue = Database.AuthToken();
                    var idDefaultExpiry = TimeSpan.FromDays(1);
                    //var redisId = new RedisString<string>(Redis._redisConn, request.ID, idDefaultExpiry);
                    Redis.CreateRedisString(request.ID, tokenValue);
                    
                    //await redisId.SetAsync(tokenValue);
                    
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
                    count = await connection.ExecuteAsync(
                        "UPDATE inventory SET AuthToken = @AuthToken WHERE ID = @ID",
                        new {AuthToken = user.AuthToken, ID = request.ID});
                    if (count != 1)
                    {
                        response.Result = ErrorCode.Login_Fail_Update_Inventory;
                        
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