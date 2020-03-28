using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MQTransaction.Common.Redis
{
    public interface IMQTransRedis
    {

        /// <summary>
        /// 加锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        bool LockTake(string key, string value, TimeSpan span);

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool LockRelease(string key, string value);

    }
}
