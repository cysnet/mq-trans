using System;
using System.Collections.Generic;
using System.Text;
using MQTransaction.Common.Helpers;
using MQTransaction.Common.Models.Entity;
using MQTransaction.Interfaces;
using MQTransaction.Options;
using Newtonsoft.Json;

namespace MQTransaction.Common.ServiceBus.RabbitMQ
{
    /// <summary>
    /// 生成数据库Rabbitmq 实体
    /// </summary>
    internal class RabbitGenerateMsgBody : IMQGenerateMsgBody
    {
        private readonly RabbitMQOptions mqoptions;
        private readonly MQTransOptions options;
    
        public RabbitGenerateMsgBody(IMQOptions mqoptions, MQTransOptions options)
        {
            this.mqoptions = mqoptions as RabbitMQOptions;
            this.options = options ;
         
        }
        public mq_published GenerateMsgEntity(IPublishQueueConfig publishQueueConfig, object message, string queryurl = null, string callbakcurl = null)
        {
            var config = publishQueueConfig as RabbitPublishQueueConfig;
            config.CheckObjectNull(nameof(config));

            var publishentity = new mq_published();
          
            publishentity.service_name = options.ServiceName;
            publishentity.exchange_name = config.ExchangeName;
            publishentity.route_key = config.RouteKey;
            publishentity.queue_name = config.QueueName;
            if (string.IsNullOrEmpty(publishentity.exchange_name)) publishentity.exchange_name = mqoptions.DeafaultExchangeName;
            if (string.IsNullOrEmpty(publishentity.route_key)) publishentity.route_key = mqoptions.DeafaultRouteKey;
            if (string.IsNullOrEmpty(publishentity.queue_name)) publishentity.queue_name = mqoptions.DeafaultQueueName;
            publishentity.message = (new Message { MsgBody = message, CallbackUrl = callbakcurl, SourceMsgId = publishentity.id, QueueName = publishentity.queue_name }).ToString();
            publishentity.query_url = queryurl;
            if (config.Args != null)
                publishentity.remark = JsonConvert.SerializeObject(config.Args);
            return publishentity;
        }

        public IPublishQueueConfig GenerateQueueConfig(mq_published entity)
        {
            RabbitPublishQueueConfig rabbitQueueConfig = new RabbitPublishQueueConfig();
            if (!string.IsNullOrEmpty(entity.remark))
                rabbitQueueConfig.Args = JsonConvert.DeserializeObject<Dictionary<string, object>>(entity.remark);
            rabbitQueueConfig.ExchangeName = entity.exchange_name;
            rabbitQueueConfig.QueueName = entity.queue_name;
            rabbitQueueConfig.RouteKey = entity.route_key;
            return rabbitQueueConfig;
        }
    }
}
