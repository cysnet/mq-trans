using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTransaction.Common.Models.ConstValue;
using MQTransaction.Common.Models.Requests;
using MQTransaction.Common.MQTransProxies;
using MQTransaction.Common.Options;
using MQTransaction.Common.Redis;
using MQTransaction.Interfaces;
using MQTransaction.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQTransaction.Common.HostServices
{
    internal class QueryStatusHostService : BackgroundService
    {
        private readonly ILogger<QueryStatusHostService> _logger;
        public IServiceProvider Services { get; }
        public QueryStatusHostService(IServiceProvider services, ILogger<QueryStatusHostService> logger)
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
                IMQTransRepository repository = null;
                MQTransOptions options = null;
                MQtransProxy mQtransProxy = null;

                try
                {
                    using (var scope = Services.CreateScope())
                    {
                        IMQTransRedis redis = null;
                        RedisOptions redisOptions = null;
                        var logid = Guid.NewGuid().ToString();

                        repository = scope.ServiceProvider.GetService<IMQTransRepository>();
                        options = scope.ServiceProvider.GetService<MQTransOptions>();
                        mQtransProxy = scope.ServiceProvider.GetService<MQtransProxy>();
                        redis = scope.ServiceProvider.GetService<IMQTransRedis>();
                        redisOptions = scope.ServiceProvider.GetService<RedisOptions>();
                        _logger.LogInformation($"{logid}->查询消息JOB*BEGIN*{options.ServiceName}*{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

                        var needSaveChange = false;
                        if (redis.LockTake(ConstValues.LockQueryMsg + options.ServiceName, ConstValues.LockQueryMsg + options.ServiceName, TimeSpan.FromSeconds(redisOptions.LockMsgSenconds)))
                        {
                            try
                            {
                                var needQueryMsgs = repository.GetDealingMsgs(500, options.ServiceName, options.DefaultRetryCount);

                                if (needQueryMsgs != null && needQueryMsgs.Count > 0)
                                {
                                    foreach (var request in needQueryMsgs)
                                    {
                                        var resultstr = await mQtransProxy.Post(request.query_url, new[] { request.id });
                                        if (!string.IsNullOrEmpty(resultstr))
                                        {
                                            var result = JsonConvert.DeserializeObject<List<QueryStatusRequest>>(resultstr);
                                            if (result != null)//找到了
                                            {
                                                var queryresult = result.Any(e => e.Id == request.id);
                                                repository.UpdateCallbackStatus(request, queryresult, options.DefaultRetryCount);
                                                needSaveChange = true;

                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"{logid}->查询消息JOB*{ex.ToString()}");
                            }
                            finally
                            {
                                if (needSaveChange)
                                    if (!repository.SaveChange())
                                        _logger.LogError($"{logid}->发送消息JOB*保存失败");
                              
                                var a = redis.LockRelease(ConstValues.LockQueryMsg + options.ServiceName, ConstValues.LockQueryMsg + options.ServiceName);
                                _logger.LogInformation($"{logid}->查询消息JOB*END*{options.ServiceName}*{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("查询消息JOB ERR：" + ex.ToString());
                }

                //3min
                Thread.Sleep(1000 * 60 * options.JobFrequencyMinute);
            }
        }
    }
}
