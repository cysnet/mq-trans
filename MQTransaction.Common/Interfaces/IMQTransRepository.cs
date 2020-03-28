using MQTransaction.Common.Models.Entity;
using MQTransaction.Common.Models.Requests;
using MQTransaction.Common.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Interfaces
{
    /// <summary>
    /// 不同数据库框架操作消息表，都需要实现此接口
    /// </summary>
    public interface IMQTransRepository
    {
        /// <summary>
        /// 查询需发送的消息
        /// </summary>
        /// <param name="size"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        List<mq_published> GetNeedPublishMsgs(int size, string serviceName);

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        bool SaveChange();

        /// <summary>
        /// 写入消息接收表
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool ComsumeMsg(Message message);

        /// <summary>
        /// 根据Id查询消息发送表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        mq_published GetPublishMsgById(string id);

        /// <summary>
        /// 根据多个Id，查询消息接收表，并生成查询请求参数
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<QueryStatusRequest> GetReceiveMsgByIds(List<string> ids);

        /// <summary>
        /// 查询已发送但未处理完成的消息
        /// </summary>
        /// <param name="size"></param>
        /// <param name="serviceName"></param>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        List<mq_published> GetDealingMsgs(int size, string serviceName, int retryCount);

        /// <summary>
        /// 初始化表结构体
        /// </summary>
        void Migrate();

        /// <summary>
        /// 发送成功后，更新实体字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="expiresMinute"></param>
        void UpdateSuccessStatus(mq_published entity, int expiresMinute);

        /// <summary>
        /// 主动查到结果后，更新实体字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="queryResult"></param>
        /// <param name="retryCount"></param>
        void UpdateCallbackStatus(mq_published entity, bool queryResult, int retryCount);
    }
}
