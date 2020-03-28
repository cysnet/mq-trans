using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MQTransaction.Common.DbOperate;
using MQTransaction.Common.HostServices;
using MQTransaction.Common.HttpMaps;
using MQTransaction.Common.Models.ConstValue;
using MQTransaction.Common.MQTransProxies;
using MQTransaction.Common.ServiceBus;
using MQTransaction.Interfaces;
using MQTransaction.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.Extensions
{
    /// <summary>
    /// 开启MQTrans扩展类
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// 添加MQTransaction注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddMQTransaction(this IServiceCollection services, Action<MQTransOptions> optionsaction)
        {
            var options = new MQTransOptions();
            //执行委托中方法
            optionsaction(options);

            options.CheckNull();

            //使委托中配置注入容器
            foreach (var register in options.Registers)
            {
                register.AddServices(services);
            }
            //注入全局配置
            services.AddSingleton(options);

            //注入job
            services.AddHostedService<PublishMsgHostService>();
            services.AddHostedService<QueryStatusHostService>();

            //注入消费端
            foreach (var consumer in options.ConsumerOptions)
            {
                services.AddScoped(consumer.ConsumerType);
            }
            //注入触发消费类
            services.AddScoped<ConsumeMessageWorker>();

            //http注入
            services.AddHttpClient();
            services.AddTransient<MQtransProxy>();

            return services;
        }

        /// <summary>
        /// 应用MQTransaction
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMQTransaction(this IApplicationBuilder app)
        {
            //开启可重复读取body
            app.Use(next => context =>
            {
                context.Request.EnableBuffering();
                return next(context);
            });

            app.Map(ConstValues.CallbackPath, MQtransHttpMaps.CallbackMap);
            app.Map(ConstValues.QueryStatusPath, MQtransHttpMaps.QueryStatusMap);


 
            using (var scope = app.ApplicationServices.CreateScope())
            {
                (scope.ServiceProvider.GetRequiredService<IMQTransRepository>()).Migrate();
                (scope.ServiceProvider.GetRequiredService<ConsumeMessageWorker>()).DirectConsume(app.ApplicationServices);
            }
            return app;
        }
    }
}
