using Microsoft.Extensions.DependencyInjection;
using MQTransaction.Common.Options;
using MQTransaction.Common.Redis;
using MQTransaction.Common.Redis.StrackExchangeRedis;
using MQTransaction.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.Registers
{
    public class StackExchangeRedisRegister : IMqTransRegister
    {
        private readonly RedisOptions redisOptions;
        public StackExchangeRedisRegister(RedisOptions redisOptions)
        {
            this.redisOptions = redisOptions;
            this.redisOptions.CheckNull();
        }

        public void AddServices(IServiceCollection services)
        {
            services.AddSingleton<RedisOptions>();


            //services.AddScoped<IDatabase>(service =>
            //{
            //    return ConnectionMultiplexer.Connect(redisOptions.RedisConnectString).GetDatabase();
            //});


            var connectionMultiplexer = ConnectionMultiplexer.Connect(redisOptions.RedisConnectString);
            services.AddScoped<IDatabase>(service =>
            {
                return connectionMultiplexer.GetDatabase();
            });

            //services.AddSingleton<IDatabase>(service => {
            //    return ConnectionMultiplexer.Connect(redisOptions.RedisConnectString).GetDatabase();
            //});

            services.AddScoped<IMQTransRedis, StackExchangeRedisService>();
        }
    }
}
