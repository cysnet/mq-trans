using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.DbOperate
{
    /// <summary>
    /// 生成EF Code First迁移文件
    /// </summary>
    internal class MigrateDbContextFactory : IDesignTimeDbContextFactory<MQTransDbContext>
    {
        public MQTransDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MQTransDbContext>();
            builder.UseMySql("Server=*****;Database=*****;Uid=*****;Pwd=*****;");
            return new MQTransDbContext(builder.Options);
        }
    }
}
