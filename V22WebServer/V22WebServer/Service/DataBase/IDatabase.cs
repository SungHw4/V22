using MySqlConnector;

namespace V22WebServer.Service.Database;

public interface IDatabase
{
    public  Task<MySqlConnection> GetAccountDbConnection();

    public  Task<MySqlConnection> GetOpenMySqlConnection(string connectionString);
}