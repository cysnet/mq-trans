using MQTransaction.Common.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Interfaces
{
    /// <summary>
    /// 生成消息实体方法
    /// </summary>
    public interface IMQGenerateMsgBody
    {
        /// <summary>
        /// 生成实体
        /// </summary>
        /// <param name="publishQueueConfig"></param>
        /// <param name="msg"></param>
        /// <param name="queryurl"></param>
        /// <param name="callbakcurl"></param>
        /// <returns></returns>
        mq_published GenerateMsgEntity(IPublishQueueConfig publishQueueConfig,
            object msg,
            string queryurl = null,
            string callbakcurl = null);

        /// <summary>
        /// 从实体生成mq配置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        IPublishQueueConfig GenerateQueueConfig(mq_published entity);
    }
}
