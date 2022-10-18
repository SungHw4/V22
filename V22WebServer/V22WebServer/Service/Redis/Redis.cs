using CloudStructures;

namespace V22WebServer.Service.Redis;

public class Redis
{
    public static RedisConnection _redisConn { get; set; }

    public static void Init(String address)
    {
        try
        {
            var config = new RedisConfig("basic", address);
            _redisConn = new RedisConnection(config);
        }
        catch (Exception e)
        {
            
        }
    }
}