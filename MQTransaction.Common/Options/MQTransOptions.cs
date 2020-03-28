using Microsoft.EntityFrameworkCore;
using MQTransaction.Common.Helpers;
using MQTransaction.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Options
{
    /// <summary>
    /// 消息一致性配置
    /// </summary>
    public class MQTransOptions
    {
        /// <summary>
        /// 服务名,每个服务唯一
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 注册类仓库
        /// </summary>
        internal IList<IMqTransRegister> Registers { get; } = new List<IMqTransRegister>();

        /// <summary>
        /// 默认查询地址，主动查询消息结果
        /// </summary>
        public string DeafaultQueryUrl { get; set; }

        /// <summary>
        /// 默认回调地址，向消息发送方推送结果
        /// </summary>
        public string DeafaultCallbackUrl { get; set; }

        /// <summary>
        /// 发送失败时，重试次数
        /// </summary>
        public int DefaultRetryCount { get; set; } = 3;

        /// <summary>
        /// 消费端配置
        /// </summary>
        internal IList<ConsumerOptions> ConsumerOptions { get; set; } = new List<ConsumerOptions>();

        /// <summary>
        /// 主动查询时间（分钟）
        /// </summary>
        public int ExpiresMinute { get; set; } = 3;

        /// <summary>
        /// 定时任务查询频率（分钟）
        /// </summary>
        public int JobFrequencyMinute { get; set; } = 3;

        /// <summary>
        /// 注册配置
        /// </summary>
        /// <param name="register"></param>
        public void Regist(IMqTransRegister register)
        {
            Registers.Add(register);
        }

        /// <summary>
        /// 添加消费端
        /// </summary>
        /// <param name="register"></param>
        public void AddConsumer<IConsumer>(IConsumerQueueConfig consumerQueueConfig) where IConsumer : IBaseConsumer
        {
            ConsumerOptions.Add(new ConsumerOptions
            {
                ConsumerQueueConfig = consumerQueueConfig,
                ConsumerType = typeof(IConsumer)
            });
        }


        public void CheckNull()
        {
            this.ServiceName.CheckStringEmpty(nameof(this.ServiceName));
            this.DeafaultCallbackUrl.CheckStringEmpty(nameof(this.DeafaultCallbackUrl));
            this.DeafaultQueryUrl.CheckStringEmpty(nameof(this.DeafaultQueryUrl));
            this.ExpiresMinute.CheckIntZero(nameof(this.ExpiresMinute));
            this.DefaultRetryCount.CheckIntZero(nameof(this.DefaultRetryCount));
            this.JobFrequencyMinute.CheckIntZero(nameof(this.JobFrequencyMinute));
        }
    }
}
