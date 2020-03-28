using MQTransaction.Common.Models.Entity;
using MQTransaction.Interfaces;
using MQTransaction.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MQTransaction.Common.ServiceBus.RabbitMQ
{
    /// <summary>
    /// Rabbitmq发送/消费消息
    /// </summary>
    internal class RabbitOperate : IMQOperate
    {
        private readonly IConnection mqConnection;
        private readonly IMQGenerateMsgBody generateMsgBody;
   
        public RabbitOperate(IMQOptions options, IMQGenerateMsgBody generateMsgBody)
        {
            var mqoptions = options as RabbitMQOptions;
            var factory = new ConnectionFactory() 
            { 
                HostName = mqoptions.Host,
                Port = mqoptions.Port, 
                UserName = mqoptions.User, 
                Password = mqoptions.Pwd,
                VirtualHost = mqoptions.VHost 
            };
            mqConnection = factory.CreateConnection();

            this.generateMsgBody = generateMsgBody;
        }

        public MQResult DirectPublish(IPublishQueueConfig mqconfig, Message message)
        {
            try
            {
                var config = mqconfig as RabbitPublishQueueConfig;
                message.MsgSeq = 1;
                using (var channel = mqConnection.CreateModel())
                {
                    channel.ConfirmSelect();
                    channel.QueueDeclare(queue: config.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: config.Args);
                    channel.ExchangeDeclare(config.ExchangeName, ExchangeType.Direct);
                    channel.QueueBind(config.QueueName, config.ExchangeName, config.RouteKey, null);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    channel.BasicPublish(exchange: config.ExchangeName, routingKey: config.RouteKey, basicProperties: properties, body: message.ToBytes());
                    if (!channel.WaitForConfirms(new TimeSpan(0, 0, 5)))
                    {
                        return new MQResult() { IsSucess = false, ExceptionMsg = "sent message failed" };
                    }
                    Console.WriteLine($"[P] sent_message,queue={config.QueueName},msg={message.ToString()}");
                    return new MQResult() { IsSucess = true, ChannelNumber = channel.ChannelNumber };
                }
            }
            catch (Exception ex)
            {
                //记log
                return new MQResult() { IsSucess = false, ExceptionMsg = ex.Message };
            }
        }

        public MQResult DirectPublish(mq_published published)
        {
            try
            {
                var config = generateMsgBody.GenerateQueueConfig(published) as RabbitPublishQueueConfig;
                Message message = JsonConvert.DeserializeObject<Message>(published.message);
                message.MsgSeq = 1;
                using (var channel = mqConnection.CreateModel())
                {
                    channel.ConfirmSelect();
                    channel.QueueDeclare(queue: config.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: config.Args);
                    channel.ExchangeDeclare(config.ExchangeName, ExchangeType.Direct);
                    channel.QueueBind(config.QueueName, config.ExchangeName, config.RouteKey, null);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    channel.BasicPublish(exchange: config.ExchangeName, routingKey: config.RouteKey, basicProperties: properties, body: message.ToBytes());
                    if (!channel.WaitForConfirms(new TimeSpan(0, 0, 5)))
                    {
                        return new MQResult() { IsSucess = false, ExceptionMsg = "sent message failed" };
                    }
                    Console.WriteLine($"[P] sent_message,queue={config.QueueName},msg={message.ToString()}");
                    return new MQResult() { IsSucess = true, ChannelNumber = channel.ChannelNumber };
                }
            }
            catch (Exception ex)
            {
                //记log
                return new MQResult() { IsSucess = false, ExceptionMsg = ex.Message };
            }
        }


        public MQResult DirectConsume(IConsumerQueueConfig config, Func<string, Task<bool>> func)
        {
            try
            {
                RabbitConsumeConfig mqconfig = config as RabbitConsumeConfig;
                
                var channel = mqConnection.CreateModel();

                #region 声明队列
                try
                {
                    channel.QueueDeclarePassive(mqconfig.QueueName);
                }
                catch
                {
                    channel = mqConnection.CreateModel();
                    channel.QueueDeclare(queue: mqconfig.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                }
                #endregion

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body);
                    var bSucess = await func(message);
                    if (bSucess && !mqconfig.AutoAck) channel.BasicAck(ea.DeliveryTag, false);
                    Console.WriteLine($"[C] consumer_message,queue={mqconfig.QueueName},msg={message}");
                };
                channel.BasicQos(0, 1, false);
                channel.BasicConsume(queue: mqconfig.QueueName, autoAck: mqconfig.AutoAck, consumer: consumer);

                return new MQResult() { IsSucess = true, ChannelNumber = channel.ChannelNumber };
            }
            catch (Exception ex)
            {
                //记log
                return new MQResult() { IsSucess = false, ExceptionMsg = ex.Message };
            }
        }
    }
}
