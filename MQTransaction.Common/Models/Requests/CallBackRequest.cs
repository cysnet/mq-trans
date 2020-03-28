using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.Models.Requests
{
    /// <summary>
    /// 回调接口请求参数
    /// </summary>
    public class CallBackRequest
    {
        public string SourceMsgId { get; set; }
    }
}
