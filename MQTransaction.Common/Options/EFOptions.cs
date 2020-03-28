using Microsoft.EntityFrameworkCore;
using MQTransaction.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Options
{
    /// <summary>
    /// 使用EF作为存储组件时的配置
    /// </summary>
    public class EFOptions
    {
        /// <summary>
        /// DbContext配置
        /// </summary>
        public Action<DbContextOptionsBuilder> DbOptionbuilderAction { get; set; }

        /// <summary>
        /// 用户使用的DbContext
        /// </summary>
        public Type MQDbContext { get; set; }

        public void CheckNull() 
        {
            this.DbOptionbuilderAction.CheckObjectNull(nameof(this.DbOptionbuilderAction));
            this.MQDbContext.CheckObjectNull(nameof(this.MQDbContext));
        }
    }
}
