using Microsoft.EntityFrameworkCore;
using MQTransaction.Common.DbOperate;
using MQTransaction.Common.Options;
using MQTransaction.Common.Registers;
using MQTransaction.Options;
using MQTransaction.Registers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.Extensions
{
    /// <summary>
    /// MQTransOptions配置的扩展类
    /// </summary>
    public static class MQTransOptionsExtension
    {
        /// <summary>
        /// 使用EF存储。也可用Dapper等框架，但需要开发
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="options"></param>
        /// <param name="mysqption"></param>
        public static void UseEF<TDbContext>(this MQTransOptions options, EFOptions mysqption)
        {
            mysqption.MQDbContext = typeof(TDbContext);
            options.Regist(new EFRegister<TDbContext>(mysqption));
        }

        /// <summary>
        /// 使用RabbitMQ做消息。也可用Kafka，但需要开发
        /// </summary>
        /// <param name="options"></param>
        /// <param name="mqoptions"></param>
        public static void UseRabbitMQ(this MQTransOptions options, RabbitMQOptions mqoptions)
        {
            options.Regist(new RabbitMQRegister(mqoptions));
        }


        /// <summary>
        /// 发消息时必填，防止分布式系统job重复执行。使用StackExchangeRedis操作redis。也可用其他，但需要开发
        /// </summary>
        /// <param name="options"></param>
        /// <param name="redisOptions"></param>
        public static void UseStackExchangeRedis(this MQTransOptions options, RedisOptions redisOptions)
        {
            options.Regist(new StackExchangeRedisRegister(redisOptions));
        }

        /// <summary>
        /// 发消息时必填，防止分布式系统job重复执行。使用CSRedis操作redis。也可用其他，但需要开发
        /// </summary>
        /// <param name="options"></param>
        /// <param name="redisOptions"></param>
        public static void UseCSRedisRedis(this MQTransOptions options, RedisOptions redisOptions)
        {
            options.Regist(new CSRedisRegister(redisOptions));
        }
    }
}
