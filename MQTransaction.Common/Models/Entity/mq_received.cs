using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.Models.Entity
{
    /// <summary>
    /// 消息接收
    /// </summary>
    public class mq_received : base_entity
    {
        /// <summary>
        /// 消息发送方id-唯一
        /// </summary>
        public string source_msg_id { get; set; }

        /// <summary>
        /// 队列名
        /// </summary>
        public string queue_name { get; set; }

        /// <summary>
        /// 消息体
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 接收状态
        /// </summary>
        public received_status status { get; set; } = received_status.未处理;

        /// <summary>
        /// 回调地址
        /// </summary>
        public string callback_url { get; set; }
    }

    public enum received_status
    {
        处理失败 = -1,
        未处理 = 0,
        处理成功 = 1
    }

    public class mq_received_map : IEntityTypeConfiguration<mq_received>
    {
        public void Configure(EntityTypeBuilder<mq_received> builder)
        {
            builder.ToTable(nameof(mq_received)).HasComment("消息接收表");
            builder.HasKey(e => e.id);
            builder.Property(e => e.id).HasMaxLength(36).HasComment("主键");
            builder.Property(e => e.create_time).HasComment("创建时间").IsRequired();

            builder.Property(e => e.source_msg_id).HasMaxLength(36).IsRequired().HasComment("消息发送方id-唯一");
            builder.HasIndex(e => e.source_msg_id).IsUnique();
            builder.Property(e => e.queue_name).HasMaxLength(255).IsRequired();
            builder.Property(e => e.message).HasComment("消息体").HasColumnType("longtext").IsRequired();
            builder.Property(e => e.status).HasMaxLength(11).HasComment("接收状态").IsRequired();
            builder.Property(e => e.callback_url).HasMaxLength(255).HasComment("回调地址").IsRequired();
        }
    }
}
