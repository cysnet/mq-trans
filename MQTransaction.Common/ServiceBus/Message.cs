using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.ServiceBus
{
    /// <summary>
    /// 发送消息体
    /// </summary>
    public class Message
    {
        public long MsgSeq { get; set; }
        /// <summary>
        /// 消息体
        /// </summary>
        public object MsgBody { get; set; }

        /// <summary>
        /// 向发送方推送收到结果接口
        /// </summary>
        public string CallbackUrl { get; set; }

        /// <summary>
        /// 消息Id
        /// </summary>
        public string SourceMsgId { get; set; }

        /// <summary>
        /// 队列表
        /// </summary>
        public string QueueName { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public byte[] ToBytes()
        {
            var str = this.ToString();
            return Encoding.UTF8.GetBytes(str);
        }
    }
}
