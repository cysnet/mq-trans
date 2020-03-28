using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQConsume.Consumers;
using MQConsume.DbOperate;
using MQTransaction.Common.Extensions;
using MQTransaction.Common.Models.ConstValue;
using MQTransaction.Common.Options;
using MQTransaction.Common.ServiceBus.RabbitMQ;
using MQTransaction.Options;

namespace MQConsume
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var dbConnection = Configuration.GetConnectionString("db");

            services.AddDbContext<MyDbContext>(options => options.UseMySql(dbConnection));
            services.AddMQTransaction(o =>
            {
                o.ServiceName = "MQConsume";

                //使用EF作为存储框架，可替换
                o.UseEF<MyDbContext>(new EFOptions
                {
                    DbOptionbuilderAction = options => options.UseMySql(dbConnection)
                });

                //使用rabbit作为消息组件，可替换
                o.UseRabbitMQ(new RabbitMQOptions
                {
                    Host = Configuration["RabbitMQ:Host"],
                    Port = int.Parse(Configuration["RabbitMQ:Port"]),
                    Pwd = Configuration["RabbitMQ:Pwd"],
                    User = Configuration["RabbitMQ:User"],
                    VHost = Configuration["RabbitMQ:VHost"],
                    DeafaultExchangeName = ConstValues.DeafaultExchangeName,
                    DeafaultQueueName = ConstValues.DeafaultQueueName,
                    DeafaultRouteKey = ConstValues.DeafaultRouteKey
                });

                o.UseStackExchangeRedis(new RedisOptions
                {
                    RedisConnectString = Configuration.GetConnectionString("redis"),
                    LockMsgSenconds = 5
                });

                o.DeafaultCallbackUrl = $"http://localhost:5001{ConstValues.CallbackPath}";
                o.DeafaultQueryUrl = $"http://localhost:5000{ConstValues.QueryStatusPath}";
                o.DefaultRetryCount = 3;
                o.ExpiresMinute = 5;
                o.JobFrequencyMinute = 1;

                //添加消费端
                o.AddConsumer<TestConsumer>(new RabbitConsumeConfig());
            });
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMQTransaction();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
