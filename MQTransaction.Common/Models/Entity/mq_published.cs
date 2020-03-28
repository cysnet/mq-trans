using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.Models.Entity
{
    /// <summary>
    /// 消息发送表
    /// </summary>
    public class mq_published : base_entity
    {
        /// <summary>
        /// rabbitmq交换机
        /// </summary>
        public string exchange_name { get; set; }
        /// <summary>
        /// rabbitmq路由
        /// </summary>
        public string route_key { get; set; }
        /// <summary>
        /// 队列名
        /// </summary>
        public string queue_name { get; set; }
        /// <summary>
        /// 消息体
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 发送状态
        /// </summary>
        public published_status status { get; set; } = published_status.未发送;
        /// <summary>
        /// 重试次数
        /// </summary>
        public int retries { get; set; } = 0;
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? expires_time { get; set; }

        /// <summary>
        /// 查询结果地址
        /// </summary>
        public string query_url { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>
        public string service_name { get; set; }
    }


    public enum published_status
    {
        发送失败 = -1,
        未发送 = 0,
        已发送 = 1,
        处理完成=2
    }


    public class mq_published_map : IEntityTypeConfiguration<mq_published>
    {
        public void Configure(EntityTypeBuilder<mq_published> builder)
        {
            builder.ToTable(nameof(mq_published)).HasComment("消息发送表");
            builder.HasKey(e => e.id);
            builder.Property(e => e.id).HasMaxLength(36).HasComment("主键");
            builder.Property(e => e.create_time).HasComment("创建时间").IsRequired();

            builder.Property(e => e.exchange_name).HasMaxLength(255);
            builder.Property(e => e.route_key).HasMaxLength(255);
            builder.Property(e => e.queue_name).HasMaxLength(255).IsRequired();
            builder.Property(e => e.message).HasComment("消息体").HasColumnType("longtext").IsRequired();
            builder.Property(e => e.status).HasMaxLength(11).HasComment("发送状态").IsRequired();
            builder.Property(e => e.retries).HasMaxLength(11).HasComment("重试次数").HasDefaultValue(0).IsRequired();
            builder.Property(e => e.expires_time).HasComment("过期时间");
            builder.Property(e => e.query_url).HasMaxLength(255).HasComment("查询结果地址").IsRequired();
            builder.Property(e => e.remark).HasColumnType("longtext").HasComment("备注");
            builder.Property(e => e.service_name).HasMaxLength(255).HasComment("服务名").IsRequired();
        }
    }
}
