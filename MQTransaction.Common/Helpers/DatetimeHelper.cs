using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.Helpers
{
    public static class DatetimeHelper
    {
        /// <summary>
        /// 将DateTimeOffset格式转换为Unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUnixTime(this DateTimeOffset dateTime)
        {
            return dateTime.ToUnixTimeSeconds();
        }
    }
}
