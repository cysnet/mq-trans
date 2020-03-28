using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MQTransaction.Common.DbOperate;
using MQTransaction.Common.Stores.EF;
using MQTransaction.Interfaces;
using MQTransaction.Options;

namespace MQTransaction.Registers
{
    /// <summary>
    /// 使用EF的Register
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    internal class EFRegister<TDbContext> : IMqTransRegister
    {
        private readonly EFOptions mysqption;
        public EFRegister(EFOptions mysqption)
        {
            this.mysqption = mysqption;
            this.mysqption.CheckNull();
        }
        public void AddServices(IServiceCollection services)
        {
            //注入dbcontext
            services.AddSingleton(mysqption);
            services.AddDbContext<MQTransDbContext>(mysqption.DbOptionbuilderAction);
            services.AddScoped<IMQTransRepository, EFMQTransRepository>();
            //EF操作类
            services.AddScoped<IMQPublisher, EFMQPublisher>();
        }
    }
}
