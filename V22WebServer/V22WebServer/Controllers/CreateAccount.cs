using Dapper;
using Microsoft.AspNetCore.Mvc;
using V22WebServer.Service.Database;

//using MySqlConnector;

namespace V22WebServer.Controllers;

[ApiController]
[Route("[controller]")]
public class CreateAccount : Controller
{
    [HttpPost]
    public async Task<PkCreateAccountResponse> Post(PkCreateAccountRequest request)
    {
        var response = new PkCreateAccountResponse {Result = ErrorCode.None};

        var saltValue = Database.SaltString();
        var hashingPassword = Database.MakeHashingPassWord(saltValue, request.PW);
        var ConnectDay = DateTime.Now;
        using (var connection = await Database.GetAccountDbConnection())
        {
            try
            {
                var count = await connection.ExecuteAsync(@"INSERT Users(ID,PW,NickName, Salt, Last_Conn) Values(@id, @pw, @nickname, @salt, @LastConn)",
                    new
                    {
                        id = request.ID,
                        pw = hashingPassword,
                        nickname = request.NickName,
                        salt = saltValue,
                        LastConn = ConnectDay
                        
                    });
                if(count != 1)
                {
                    response.Result = ErrorCode.Create_Account_Fail_Duplicate;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine(request.ID);
                Console.WriteLine(hashingPassword);
                Console.WriteLine(request.NickName);
                Console.WriteLine(saltValue);
                Console.WriteLine(ConnectDay);
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