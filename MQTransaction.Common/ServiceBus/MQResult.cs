using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.ServiceBus
{
    /// <summary>
    /// 消息发送结果
    /// </summary>
    public class MQResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSucess { get; set; }

        public string Code { get; set; }
        public int ChannelNumber { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ExceptionMsg { get; set; }
    }
}
