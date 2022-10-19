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

        var saltValue = Database.SaltString();
        var hashingPassword = Database.MakeHashingPassWord(saltValue, request.PW);
        
        using (var connection = await Database.GetAccountDbConnection())
        {
            try
            {
                var count = await connection.ExecuteAsync(@"INSERT Users(ID,PW,NickName, Salt) Values(@id, @pw, @nickname, @salt)",
                    new
                    {
                        id = request.ID,
                        pw = hashingPassword,
                        nickname = request.NickName,
                        salt = saltValue
                    });
                if(count != 1)
                {
                    response.Result = ErrorCode.Create_Account_Fail_Duplicate;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                response.Result = ErrorCode.Create_Account_Fail_Exception;
                return response;
            }
        }

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