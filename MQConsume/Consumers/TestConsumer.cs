using Microsoft.Extensions.Logging;
using MQTransaction.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MQConsume.Consumers
{
    public class TestConsumer : IBaseConsumer
    {
        private readonly ILogger<TestConsumer> logger;
        public TestConsumer(ILogger<TestConsumer> logger)
        {
            this.logger = logger;
        }
        public Task<bool> DealMessage(string message)
        {
            logger.LogInformation("收到消息：", message);
            return Task.FromResult(true);
        }
    }
}
