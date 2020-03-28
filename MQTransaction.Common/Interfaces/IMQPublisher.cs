using Microsoft.EntityFrameworkCore;
using MQTransaction.Common.DbOperate;
using MQTransaction.Common.Models.Entity;
using MQTransaction.Common.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Interfaces
{
    /// <summary>
    /// 使用不同数据库操作框架写消息，都需实现此接口
    /// </summary>
    public interface IMQPublisher
    {
        /// <summary>
        /// 消息入库
        /// </summary>
        /// <typeparam name="DbOperate"></typeparam>
        /// <typeparam name="TMQConfig"></typeparam>
        /// <param name="message"></param>
        /// <param name="mQConfig"></param>
        /// <param name="queryurl"></param>
        /// <param name="callbakcurl"></param>
        /// <returns></returns>
        mq_published AddMsgToDb(
           object message,
           IPublishQueueConfig mQConfig,
           string queryurl = null,
           string callbakcurl = null
           );

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="published"></param>
        /// <returns></returns>
        MQResult DirectPublish(mq_published published);
    }

}
