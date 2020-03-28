using MQTransaction.Common.Models.ConstValue;
using MQTransaction.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.ServiceBus.RabbitMQ
{
    /// <summary>
    /// RabbitMq消费配置
    /// </summary>
    public class RabbitConsumeConfig : IConsumerQueueConfig
    {
        public bool AutoAck { get; set; } = true;
        public string QueueName { get; set; } = ConstValues.DeafaultQueueName;
    }
}
