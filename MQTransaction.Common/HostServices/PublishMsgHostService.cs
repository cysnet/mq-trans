using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTransaction.Common.Models.ConstValue;
using MQTransaction.Common.Models.Entity;
using MQTransaction.Common.Options;
using MQTransaction.Common.Redis;
using MQTransaction.Common.ServiceBus;
using MQTransaction.Interfaces;
using MQTransaction.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQTransaction.Common.HostServices
{
    internal class PublishMsgHostService : BackgroundService
    {

        private readonly ILogger<PublishMsgHostService> _logger;
        public IServiceProvider Services { get; }
        public PublishMsgHostService(IServiceProvider services, ILogger<PublishMsgHostService> logger)
        {
            Services = services;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () => await DoAsync(stoppingToken));
        }

        public async Task DoAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                MQTransOptions options = null;
                IMQTransRepository repository = null;
                IMQOperate mqOperate = null;
                IMQGenerateMsgBody generateMsgBody = null;

                try
                {
                    using (var scope = Services.CreateScope())
                    {
                        IMQTransRedis redis = null;
                        RedisOptions redisOptions = null;
                        var logid = Guid.NewGuid().ToString();
                        _logger.LogInformation($"{logid}->发送消息JOB*BEGIN*{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                        repository = scope.ServiceProvider.GetService<IMQTransRepository>();
                        mqOperate = scope.ServiceProvider.GetService<IMQOperate>();
                        generateMsgBody = scope.ServiceProvider.GetService<IMQGenerateMsgBody>();
                        options = scope.ServiceProvider.GetService<MQTransOptions>();
                        redis = scope.ServiceProvider.GetService<IMQTransRedis>();
                        redisOptions = scope.ServiceProvider.GetService<RedisOptions>();
                        var needSaveChange = false;
                        if (redis.LockTake(ConstValues.LockSendMsg + options.ServiceName, options.ServiceName, TimeSpan.FromSeconds(redisOptions.LockMsgSenconds)))
                        {
                            var needPublishMsgs = repository.GetNeedPublishMsgs(100, options.ServiceName);
                            _logger.LogInformation($"{logid}->发送消息JOB*COUNT*{needPublishMsgs.Count}*{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                            try
                            {
                                foreach (var msg in needPublishMsgs)
                                {
                                    var result = mqOperate.DirectPublish(generateMsgBody.GenerateQueueConfig(msg), JsonConvert.DeserializeObject<Message>(msg.message));
                                    if (result.IsSucess)
                                    {
                                        needSaveChange = true;
                                        repository.UpdateSuccessStatus(msg, options.ExpiresMinute);
                                    }
                                    else
                                    {
                                        //发送失败,不做处理,会自动重新发起
                                        _logger.LogError($"{logid}->发送消息JOB*未发送成功{JsonConvert.SerializeObject(result)}");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //未查到,不做处理,会自动重新发起
                                _logger.LogError($"{logid}->发送消息JOB*{ex.ToString()}");
                            }
                            finally
                            {
                                if (needSaveChange)
                                    if (!repository.SaveChange())
                                        _logger.LogError($"{logid}->发送消息JOB*保存失败");
                               
                                var a = redis.LockRelease(ConstValues.LockSendMsg + options.ServiceName, options.ServiceName);
                                _logger.LogError($"{logid}->发送消息JOB*END*{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("发送消息JOB ERR：" + ex.ToString());
                }

                //3min
                Thread.Sleep(1000 * 60 * options.JobFrequencyMinute);
            }
        }
    }
}
