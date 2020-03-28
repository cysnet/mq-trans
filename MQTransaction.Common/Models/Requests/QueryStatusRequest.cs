using MQTransaction.Common.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.Models.Requests
{
    /// <summary>
    /// 查询接口请求体
    /// </summary>
    public class QueryStatusRequest
    {
        public string Id { get; set; }
        public received_status Status { get; set; }
    }
}
