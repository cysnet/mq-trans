using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MQPoducer.DbOperate;
using MQPoducer.Models;
using MQTransaction.Common.ServiceBus.RabbitMQ;
using MQTransaction.Interfaces;

namespace MQPoducer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly MyDbContext myDbContext;
        private readonly IMQPublisher mpPublisher;
        public ValuesController(MyDbContext myDbContext, IMQPublisher mpPublisher)
        {
            this.myDbContext = myDbContext;
            this.mpPublisher = mpPublisher;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var testa = new test();
            testa.name = "a";
            myDbContext.Add(testa);
            //消息与业务表test同时入库
            var msg = mpPublisher.AddMsgToDb("你好",  new RabbitPublishQueueConfig { });
            myDbContext.SaveChanges();
            //消息真实推送
            var result = mpPublisher.DirectPublish(msg);
            return Ok(DateTime.Now);
        }
    }
}
