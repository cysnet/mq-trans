using Microsoft.Extensions.DependencyInjection;
using MQTransaction.Common.Options;
using MQTransaction.Common.Redis;
using MQTransaction.Common.Redis.CSRedisImp;
using MQTransaction.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.Registers
{
    public class CSRedisRegister : IMqTransRegister
    {
        private readonly RedisOptions redisOptions;
        public CSRedisRegister(RedisOptions redisOptions)
        {
            this.redisOptions = redisOptions;
            this.redisOptions.CheckNull();
        }
        public void AddServices(IServiceCollection services)
        {
            services.AddSingleton<RedisOptions>();
            var csredis = new CSRedis.CSRedisClient(redisOptions.RedisConnectString);
            RedisHelper.Initialization(csredis);
            services.AddScoped<IMQTransRedis, CSRedisService>();
        }
    }
}
