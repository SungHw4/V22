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

public class GetPlayerInventory : Controller
{
    [HttpPost]
    public async Task<PkGetPlayerInventoryResponse> Post(PkGetPlayerInventoryRequest request)
    {
        var response = new PkGetPlayerInventoryResponse {Result = ErrorCode.None};

        using (var connection = await Database.GetAccountDbConnection())
        {
            try
            {
                var count = await connection.ExecuteAsync(
                    "SELECT ID,AuthToken FROM inventory WHERE ID=@ID AND AuthToken = @AuthToken",request);
                if (count == 0)
                {
                    DBUserInventory inven = new DBUserInventory();
                    inven.ID = request.ID;
                    inven.AuthToken = request.AuthToken;
                    inven.BallCount = 5;
                    inven.StarCount = 0;
                    var userinven = await connection.QueryFirstOrDefaultAsync<DBUserInventory>(
                        @"INSERT inventory(ID, AuthToken, StarCount, BallCount) Values(@id,@authtoken,@starcount,@ballcount)",
                        inven);
                    
                    response.Result = ErrorCode.GET_Inventory_Create;
                    return response;
                }
                else
                {
                    var userinven = await connection.QueryFirstOrDefaultAsync<DBUserInventory>(
                        @"SELECT StarCount, BallCount FROM inventory WHERE ID=@ID AND AuthToken = @AuthToken",
                        response);
                    
                    response.Result = ErrorCode.None;
                    return response;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                response.Result = ErrorCode.GET_Inventory_Fail;
                return response;
            }
        }
        
    }
    
}

public class PkGetPlayerInventoryRequest
{
    public string ID { get; set; }
    public string AuthToken { get; set; }
}

public class PkGetPlayerInventoryResponse
{
    public ErrorCode Result { get; set; }
    public int StarCount { get; set; }
    public int BallCount { get; set; }
}

public class DBUserInventory
{
    public string ID { get; set; }
    public string AuthToken { get; set; }
    public int StarCount { get; set; }
    public int BallCount { get; set; }
}