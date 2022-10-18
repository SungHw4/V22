using Dapper;
using Microsoft.AspNetCore.Mvc;
using V22WebServer.Service.Database;

//using MySqlConnector;

namespace V22WebServer.Controllers;

[ApiController]
[Route("[controller")]
public class CreateAccount : Controller
{
    [HttpPost]
    public async Task<PkCreateAccountResponse> Post(PkCreateAccountRequest request)
    {
        var response = new PkCreateAccountResponse {Result = ErrorCode.None};

        //var saltValue;
        
        return response;
    }
}

public class PkCreateAccountRequest
{
    public string ID { get; set; }
    public string PW { get; set; }
    public string NickName { get; set; }
}

public class PkCreateAccountResponse
{
    public ErrorCode Result { get; set; }
}