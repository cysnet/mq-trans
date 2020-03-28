using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MQTransaction.Common.DbOperate;
using MQTransaction.Common.Models.Entity;
using MQTransaction.Common.Models.Requests;
using MQTransaction.Common.ServiceBus;
using MQTransaction.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MQTransaction.Common.Stores.EF
{
    /// <summary>
    /// 操作EF仓库
    /// </summary>
    internal class EFMQTransRepository : IMQTransRepository
    {
        private readonly MQTransDbContext dbContext;
        private readonly ILogger<EFMQTransRepository> logger;
        public EFMQTransRepository(MQTransDbContext dbContext, ILogger<EFMQTransRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }
        public List<mq_published> GetNeedPublishMsgs(int size, string serviceName)
        {
            return this.dbContext.published.Where(e => e.status == published_status.未发送 && e.service_name == serviceName).OrderBy(e => e.create_time).Take(size).ToList();
        }

        public List<mq_published> GetDealingMsgs(int size, string serviceName, int retryCount)
        {
            return this.dbContext.published.Where(e => e.status == published_status.已发送 && e.service_name == serviceName && e.retries <= retryCount && e.expires_time <= DateTime.Now).OrderBy(e => e.create_time).Take(size).ToList();
        }

        /// <summary>
        /// 消费
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ComsumeMsg(Message message)
        {
            try
            {
                var comsumer_msg = new mq_received();
                comsumer_msg.source_msg_id = message.SourceMsgId;
                comsumer_msg.queue_name = message.QueueName;
                comsumer_msg.message = JsonConvert.SerializeObject(message);
                comsumer_msg.status = received_status.未处理;
                comsumer_msg.callback_url = message.CallbackUrl;
                this.dbContext.Add(comsumer_msg);
                return this.dbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"插入接收表失败->{ message.SourceMsgId}->{ex.ToString()}");
                return false;
            }

        }

        public mq_published GetPublishMsgById(string id)
        {
            return this.dbContext.published.FirstOrDefault(e => id == e.id);
        }

        public List<QueryStatusRequest> GetReceiveMsgByIds(List<string> ids)
        {
            return this.dbContext.received.Where(e => ids.Contains(e.source_msg_id)).Select(e => new QueryStatusRequest { Id = e.source_msg_id, Status = e.status }).ToList();
        }

        public void UpdateSuccessStatus(mq_published entity, int expiresMinute)
        {
            entity.status = published_status.已发送;
            //10分钟后不是“处理成功状态”，则发起查询，未查到改为“发送失败”，查到改为处理成功
            entity.expires_time = DateTime.Now.AddMinutes(expiresMinute);
        }

        public void UpdateCallbackStatus(mq_published entity, bool queryResult, int retryCount)
        {
            if (queryResult)
            {
                entity.status = Models.Entity.published_status.处理完成;
            }
            else //没找到
            {
                if (entity.retries + 1 > retryCount)
                {
                    entity.status = Models.Entity.published_status.发送失败;
                }
                else
                {
                    entity.retries += 1;
                    entity.status = Models.Entity.published_status.未发送;
                }
            }
        }

        public bool SaveChange()
        {
            return this.dbContext.SaveChanges() > 0;
        }

        public void Migrate()
        {
            while (true)
            {
                try
                {
                    this.dbContext.Database.Migrate();
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError($"数据库自动迁移失败{ex.ToString()}");
                }
            }
        }
    }
}
