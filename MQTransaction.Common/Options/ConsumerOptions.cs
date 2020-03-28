using MQTransaction.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Options
{
    /// <summary>
    /// 消费端注入的信息
    /// </summary>
    internal class ConsumerOptions
    {
        /// <summary>
        /// 消费端类型
        /// </summary>
        public Type ConsumerType { get; set; }

        /// <summary>
        /// 消费时的参数
        /// </summary>
        public IConsumerQueueConfig ConsumerQueueConfig { get; set; }
    }
}
