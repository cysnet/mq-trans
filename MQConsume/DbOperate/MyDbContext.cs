using Microsoft.EntityFrameworkCore;
using MQConsume.Models;
using MQTransaction.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MQConsume.DbOperate
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
