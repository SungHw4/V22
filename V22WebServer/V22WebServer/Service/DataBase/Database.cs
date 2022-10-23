using CloudStructures;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
namespace V22WebServer.Service.Database;

public class Database //: IDatabase
{
    private static string DataBaseConnectionString;
    private static Dictionary<string, string> DBConnDic;
    static string RedisAddress;

    public static RedisConnection RedisConn { get; set; }
    
    public static void Init(string address)
    {
        
        DataBaseConnectionString = address;
    }

    public static async Task<MySqlConnection> GetAccountDbConnection()
    {
        return await GetOpenMySqlConnection(DataBaseConnectionString);
    }
    
    static async Task<MySqlConnection> GetOpenMySqlConnection(string connectionString) 
    {
        var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();
        return connection;
    }
    
    public static string SaltString()
    {

        const string allowableCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";
        var bytes = new byte[64];
        using(var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(bytes);
        }

        return new string(bytes.Select(x => allowableCharacters[x % allowableCharacters.Length]).ToArray());
    }

    public  static string MakeHashingPassWord(string saltValue, string pw)
    {
        var sha = new SHA256Managed();
        byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes((saltValue+pw)));
        StringBuilder stringBuilder = new StringBuilder();
        foreach (byte b in hash)
        {
            stringBuilder.AppendFormat("{0:x2}", b);
        }
        
        return stringBuilder.ToString();
    }
    private const string AllowableCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";
    public static string AuthToken()
    {
        var bytes = new byte[25];
        using (var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(bytes);
        }
        return new string(bytes.Select(x => AllowableCharacters[x % AllowableCharacters.Length]).ToArray());
    }
}