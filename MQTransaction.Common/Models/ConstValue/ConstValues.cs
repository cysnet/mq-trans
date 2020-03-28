using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.Models.ConstValue
{
    public class ConstValues
    {
        /// <summary>
        /// 默认交换机
        /// </summary>
        public const string DeafaultExchangeName = "mqtrans.ex";
        /// <summary>
        /// 默认路由
        /// </summary>
        public const string DeafaultRouteKey = "mqtrans.rt";

        /// <summary>
        /// 默认队列名
        /// </summary>
        public const string DeafaultQueueName = "mqtrans.q";

        /// <summary>
        /// 回调地址
        /// </summary>
        public const string CallbackPath = "/mqtrans/callback";

        /// <summary>
        /// 查询地址
        /// </summary>
        public const string QueryStatusPath = "/mqtrans/querystatus";


        /// <summary>
        /// Redis前缀
        /// </summary>
        public const string RedisPrefix = "mqtrans:";

        /// <summary>
        /// LockSendMsg
        /// </summary>
        public const string LockSendMsg = "locksendmsg";


        /// <summary>
        /// LockQueryMsg
        /// </summary>
        public const string LockQueryMsg = "lockquerymsg";

    }
}
