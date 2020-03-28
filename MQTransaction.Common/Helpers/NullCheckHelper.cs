using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.Helpers
{
    public static class NullCheckHelper
    {
        /// <summary>
        /// 检查字符串是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <param name="name"></param>
        public static void CheckStringEmpty(this string str,string name)
        {
            if(string.IsNullOrEmpty(str))
                throw new ArgumentNullException($"{name}不可为空");
        }
        /// <summary>
        /// 检查int是否为0
        /// </summary>
        /// <param name="val"></param>
        /// <param name="name"></param>
        public static void CheckIntZero(this int val,string name)
        {
            if (val == 0)
            {
                throw new ArgumentNullException($"{name}不可为0");
            }
        }

        /// <summary>
        /// 检查object是否为空
        /// </summary>
        /// <param name="val"></param>
        /// <param name="name"></param>
        public static void CheckObjectNull(this object obj,string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException($"{name}不可为空");
            }
        }
    }
}
