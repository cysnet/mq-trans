using Microsoft.EntityFrameworkCore;
using MQTransaction.Common.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.Extensions
{
    /// <summary>
    /// Dbcontext的扩展类
    /// </summary>
    public static class DbContextExtension
    {
        //添加消息事务相关表
        public static void AddMQTransactionTable(this ModelBuilder modelBuilder)
        {
            //发送消息的配置
            modelBuilder.ApplyConfiguration(new mq_published_map());
            //接收消息的配置
            modelBuilder.ApplyConfiguration(new mq_received_map());
        }
    }
}
