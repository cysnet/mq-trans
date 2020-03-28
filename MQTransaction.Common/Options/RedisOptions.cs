using MQTransaction.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.Options
{
    public class RedisOptions
    {
        /// <summary>
        /// redis连接串
        /// </summary>
        public string RedisConnectString { get; set; }
        /// <summary>
        /// job执行锁定时间
        /// </summary>
        public int LockMsgSenconds { get; set; } = 3;

        public void CheckNull()
        {
            this.RedisConnectString.CheckStringEmpty(nameof(this.RedisConnectString));
            this.LockMsgSenconds.CheckIntZero(nameof(this.LockMsgSenconds));
        }
    }
}
