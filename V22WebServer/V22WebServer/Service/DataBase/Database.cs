using CloudStructures;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
namespace V22WebServer.Service.Database;

public class Database : IDatabase
{
    static string DataBaseConnectionString;
    static string RedisAddress;

    public static RedisConnection RedisConn { get; set; }
    
    public static void Init(string address)
    {
        
        DataBaseConnectionString = address;
    }

    public async Task<MySqlConnection> GetAccountDbConnection()
    {
        return await GetOpenMySqlConnection(DataBaseConnectionString);
    }
    
    public async Task<MySqlConnection> GetOpenMySqlConnection(string connectionString) 
    {
        var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();
        return connection;
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
    
}