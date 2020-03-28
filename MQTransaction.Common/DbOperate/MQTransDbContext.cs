using Microsoft.EntityFrameworkCore;
using MQTransaction.Common.Extensions;
using MQTransaction.Common.Models;
using MQTransaction.Common.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.DbOperate
{
    /// <summary>
    /// EF作为存储组件，使用的dbcontext
    /// </summary>
    internal class MQTransDbContext : DbContext
    {
        public MQTransDbContext(DbContextOptions options) : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //设置表结构
            modelBuilder.AddMQTransactionTable();
            base.OnModelCreating(modelBuilder);
        }

        //发送消息
        public virtual DbSet<mq_published> published { get; set; }
        //接收消息
        public virtual DbSet<mq_received> received { get; set; }

    }
}
