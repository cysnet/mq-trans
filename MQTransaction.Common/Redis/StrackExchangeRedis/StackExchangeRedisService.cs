using MQTransaction.Common.Models.ConstValue;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MQTransaction.Common.Redis.StrackExchangeRedis
{
    public class StackExchangeRedisService : IMQTransRedis
    {
        private readonly IDatabase redis;
        public StackExchangeRedisService(IDatabase redis)
        {
            this.redis = redis;
        }

        public bool LockTake(string key, string value, TimeSpan span)
        {
            return redis.LockTake(ConstValues.RedisPrefix + nameof(LockTake) + ":" + key, value, span);
        }
        public bool LockRelease(string key, string value)
        {
            return redis.LockRelease(ConstValues.RedisPrefix + nameof(LockRelease) + ":" + key, value);
        }

    }
}
