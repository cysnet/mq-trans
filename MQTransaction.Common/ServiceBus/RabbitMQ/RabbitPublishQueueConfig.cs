using MQTransaction.Common.Helpers;
using MQTransaction.Common.Models.ConstValue;
using MQTransaction.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.ServiceBus.RabbitMQ
{
    /// <summary>
    /// 发送消息时的参数
    /// </summary>
    public class RabbitPublishQueueConfig : IPublishQueueConfig
    {
        /// <summary>
        /// 交换机
        /// </summary>
        public string ExchangeName { get; set; } = ConstValues.DeafaultExchangeName;

        /// <summary>
        /// 路由
        /// </summary>
        public string RouteKey { get; set; } = ConstValues.DeafaultRouteKey;

        /// <summary>
        /// 队列名
        /// </summary>
        public string QueueName { get; set; } = ConstValues.DeafaultQueueName;

        /// <summary>
        /// 配置
        /// </summary>
        public IDictionary<string, object> Args { get; set; }

        public void CheckNull()
        {
            this.ExchangeName.CheckStringEmpty(nameof(this.ExchangeName));
            this.RouteKey.CheckStringEmpty(nameof(this.RouteKey));
            this.QueueName.CheckStringEmpty(nameof(this.QueueName));
        }
    }
}
