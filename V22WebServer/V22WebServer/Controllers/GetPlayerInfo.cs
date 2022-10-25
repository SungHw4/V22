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

public class GetPlayerInfo : Controller
{
    [HttpPost]
    public async Task<PkGetPlayerInfoResponse> Post(PkLoginRequest request)
    {
        var response = new PkGetPlayerInfoResponse {Result = ErrorCode.None};

        using (var connection = await Database.GetAccountDbConnection())
        {
            try
            {
                //var count = await connection.ExecuteAsync()
                
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                response.Result = ErrorCode.Get_PlayerInfo_Fail;
                return response;
            }
        }
        
        return response;    
    }
    
}

public class PkGetPlayerInfoRequest
{
    public string ID { get; set; }
    public string AuthToken { get; set; }
}

public class PkGetPlayerInfoResponse
{
    public ErrorCode Result { get; set; }
}