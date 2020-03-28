using Microsoft.EntityFrameworkCore;
using MQPoducer.DbOperate;
using MQPoducer.Models;
using MQTransaction.Common.DbOperate;
using MQTransaction.Common.Extensions;
using MQTransaction.Common.Models;
using MQTransaction.Common.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQPoducer.DbOperate
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //添加消息事务相关表
            modelBuilder.AddMQTransactionTable();
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<test> test { get; set; }
    }
}
