using Microsoft.Extensions.DependencyInjection;
using MQTransaction.Common.Models.Requests;
using MQTransaction.Common.MQTransProxies;
using MQTransaction.Interfaces;
using MQTransaction.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MQTransaction.Common.ServiceBus
{
    /// <summary>
    /// 消费触发类
    /// </summary>
    internal class ConsumeMessageWorker
    {
        private readonly IMQOperate mQOperate;
        private readonly MQTransOptions mQTransOptions ;
        public ConsumeMessageWorker(IMQOperate mQOperate, MQTransOptions mQTransOptions)
        {
            this.mQOperate = mQOperate;
            this.mQTransOptions = mQTransOptions;
        }

        public void DirectConsume(IServiceProvider serviceProvider)
        {
            this.mQTransOptions.ConsumerOptions?.ToList().ForEach(options =>
            {
                mQOperate.DirectConsume(options.ConsumerQueueConfig, async msg =>
                {
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var repository = scope.ServiceProvider.GetService<IMQTransRepository>();
                        var mQtransProxy = scope.ServiceProvider.GetService<MQtransProxy>();
                        var message = JsonConvert.DeserializeObject<Message>(msg);
                        if (!string.IsNullOrEmpty(message.CallbackUrl))
                        {
                            //回调
                            var result = await mQtransProxy.Post(message.CallbackUrl, new CallBackRequest { SourceMsgId = message.SourceMsgId });
                        }
                        //入库
                        if (repository.ComsumeMsg(message))
                        {
                            //消息处理
                            var consumer = scope.ServiceProvider.GetService(options.ConsumerType) as IBaseConsumer;
                            var result = consumer.DealMessage(msg).Result;
                        }
                    }
                    return true;
                });

            });
        }
    }
}
