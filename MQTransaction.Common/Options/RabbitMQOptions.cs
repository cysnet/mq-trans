using MQTransaction.Common.Helpers;
using MQTransaction.Common.Models.ConstValue;
using MQTransaction.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Options
{
    /// <summary>
    /// 使用Rabbit作为消息中间件的配置
    /// </summary>
    public class RabbitMQOptions : IMQOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Pwd { get; set; }
        public string VHost { get; set; } = "/";

        /// <summary>
        /// 默认交换机
        /// </summary>
        public string DeafaultExchangeName { get; set; } = ConstValues.DeafaultExchangeName;

        /// <summary>
        /// 默认路由
        /// </summary>
        public string DeafaultRouteKey { get; set; } = ConstValues.DeafaultRouteKey;

        /// <summary>
        /// 默认队列名
        /// </summary>
        public string DeafaultQueueName { get; set; } = ConstValues.DeafaultQueueName;

        public void CheckNull()
        {
            this.Host.CheckStringEmpty(nameof(this.Host));
            this.Port.CheckIntZero(nameof(this.Port));
            this.Pwd.CheckStringEmpty(nameof(this.Pwd));
            this.User.CheckStringEmpty(nameof(this.User));
            this.VHost.CheckStringEmpty(nameof(this.VHost));
            this.DeafaultExchangeName.CheckStringEmpty(nameof(this.DeafaultExchangeName));
            this.DeafaultQueueName.CheckStringEmpty(nameof(this.DeafaultQueueName));
            this.DeafaultRouteKey.CheckStringEmpty(nameof(this.DeafaultRouteKey));
        }
    }
}
