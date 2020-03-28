using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MQTransaction.Common.Helpers;
using MQTransaction.Common.Models.Entity;
using MQTransaction.Common.ServiceBus;
using MQTransaction.Interfaces;
using MQTransaction.Options;
using Newtonsoft.Json;

namespace MQTransaction.Common.Stores.EF
{
    /// <summary>
    /// EF写表
    /// </summary>
    internal class EFMQPublisher : IMQPublisher
    {
        private readonly MQTransOptions options;
        private readonly IServiceProvider serviceProvider;
        private readonly EFOptions mySqlEFOptions;
        private readonly IMQGenerateMsgBody mQGenerateMsgBody;
        private readonly IMQTransRepository repository;
        private readonly IMQOperate mQOperate;
        public EFMQPublisher( MQTransOptions options
            , IServiceProvider serviceProvider
            , EFOptions mySqlEFOptions
            , IMQGenerateMsgBody mQGenerateMsgBody
            , IMQTransRepository repository
            , IMQOperate mQOperate)
        {
            this.options = options;
            this.serviceProvider = serviceProvider;
            this.mySqlEFOptions = mySqlEFOptions;
            this.mQGenerateMsgBody = mQGenerateMsgBody;
            this.repository = repository;
            this.mQOperate = mQOperate;
        }

        public mq_published AddMsgToDb(
            object message,
            IPublishQueueConfig mQConfig,
            string queryurl = null,
            string callbakcurl = null
            )
        {
            //message.CheckObjectNull(nameof(message));
            if (string.IsNullOrEmpty(queryurl)) queryurl = options.DeafaultQueryUrl;
            if (string.IsNullOrEmpty(callbakcurl)) callbakcurl = options.DeafaultCallbackUrl;
            //生成发布的消息实体
            var publishentity = mQGenerateMsgBody.GenerateMsgEntity(mQConfig, message, queryurl, callbakcurl);
            repository.UpdateSuccessStatus(publishentity, options.ExpiresMinute);
            //写入
            var dbContext = serviceProvider.GetService(mySqlEFOptions.MQDbContext) as DbContext;
            dbContext.Add(publishentity);

            return publishentity;
        }

        public MQResult DirectPublish(mq_published published)
        {
            return this.mQOperate.DirectPublish(published);
        }

    }
}

