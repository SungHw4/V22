using CloudStructures;
using CloudStructures.Structures;


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
            Console.WriteLine(e.Message);
        }
    }

    public class DBUserInfo
    {
        public string ID { get; set; }
        public string PW { get; set; }
        public string NickName { get; set; }
        public string Salt { get; set; }
        public DateTime Last_Conn { get; set; }
        public string AuthToken { get; set; }
    }
    public static void CreateRedisString(string Key, string value)
    {
        var key = Key;
        var TimeLimits = TimeSpan.FromDays(1);
        var redis = new RedisString<string>(_redisConn,key,TimeLimits);
        redis.SetAsync(value);
    }

    public static void CreateRedisStructure(DBUserInfo player, string Key)
    {
        var TimeLimits = TimeSpan.FromDays(1);
        var redis = new RedisString<DBUserInfo>(_redisConn,Key,TimeLimits);
        redis.SetAsync(player);
    }
    
    // public static void CallCommand(PlayerInfo Player, RedisString<PlayerInfo> redis)
    // {
    //     var player = Player;
    //     await redis.SetAsync(player);
    //     var result = await redis.GetAsync();
    // }
    
    
}