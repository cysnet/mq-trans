using MQTransaction.Common.Models.Entity;
using MQTransaction.Common.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MQTransaction.Interfaces
{
    /// <summary>
    /// 不同消息中间件操作，都需实现此接口
    /// </summary>
    public interface IMQOperate
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="mqconfig"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        MQResult DirectPublish(IPublishQueueConfig mqconfig, Message message);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="mqconfig"></param>
        /// <returns></returns>
        MQResult DirectPublish(mq_published published);

        /// <summary>
        /// 消费消息
        /// </summary>
        /// <param name="config"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        MQResult DirectConsume(IConsumerQueueConfig config, Func<string, Task<bool>> func);

        
    }
}
