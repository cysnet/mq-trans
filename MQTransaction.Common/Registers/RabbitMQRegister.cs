using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using MQTransaction.Common.ServiceBus.RabbitMQ;
using MQTransaction.Interfaces;
using MQTransaction.Options;

namespace MQTransaction.Registers
{
    /// <summary>
    /// rabitmq 注册
    /// </summary>
    internal class RabbitMQRegister : IMqTransRegister
    {
        private readonly RabbitMQOptions mqoptions;
        public RabbitMQRegister(RabbitMQOptions mqoptions)
        {
            this.mqoptions = mqoptions;
            this.mqoptions.CheckNull();
        }
        public void AddServices(IServiceCollection services)
        {
            //mq配置
            services.AddSingleton<IMQOptions>(mqoptions);

            //ADD 消费 发送
            services.AddSingleton<IMQOperate, RabbitOperate>();

            //mq生辰消息体注入
            services.AddSingleton<IMQGenerateMsgBody, RabbitGenerateMsgBody>();
        }
    }
}
