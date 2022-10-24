using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CloudStructures;
using System.Threading.Tasks;
using CloudStructures.Structures;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
//using StackExchange.Redis.Extensions;
using StackExchange.Redis.Extensions.Utf8Json;
using StackExchange.Redis.Extensions.Newtonsoft;

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

    public class PlayerInfo
    {
        public Int64 user_id;
        public string Nickname;
        public int Token;
        public int PW;
        public DateTime Last_Conn;
    }
    public static void CreateRedisStructure(string Key)
    {
        var key = Key;
        var TimeLimits = TimeSpan.FromDays(1);
        var redis = new RedisString<PlayerInfo>(_redisConn,key,TimeLimits);
    }
    
    // public static void CallCommand(PlayerInfo Player, RedisString<PlayerInfo> redis)
    // {
    //     var player = Player;
    //     await redis.SetAsync(player);
    //     var result = await redis.GetAsync();
    // }
    
    
}