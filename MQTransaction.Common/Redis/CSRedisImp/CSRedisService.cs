using MQTransaction.Common.Helpers;
using MQTransaction.Common.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQTransaction.Common.Redis.CSRedisImp
{
    public class CSRedisService : IMQTransRedis
    {
        public CSRedisService()
        {

        }
        public bool LockRelease(string key, string value)
        {
            return RedisHelper.Del(key) > 0;
        }

        public bool LockTake(string key, string value, TimeSpan span)
        {
            return RedisLock(key, Convert.ToInt64(span.TotalSeconds));
        }


        private bool RedisLock(string key, long timeoutSecond)
        {
            if (RedisHelper.SetNx(key, DateTimeOffset.Now.DateTimeToUnixTime() + timeoutSecond))
            {
                //设置过期时间
                RedisHelper.Expire(key, TimeSpan.FromSeconds(timeoutSecond));
                return true;
            }
            else
            {
                //未获取到锁，继续判断，判断时间戳看看是否可以重置并获取锁
                var lockValue = RedisHelper.Get(key);
                var time = DateTimeOffset.Now.DateTimeToUnixTime();

                if (!string.IsNullOrEmpty(lockValue) && time > Convert.ToInt64(lockValue))
                {
                    //再次用当前时间戳getset
                    //返回固定key的旧值，旧值判断是否可以获取锁
                    var getsetResult = RedisHelper.GetSet(key, time);
                    if (getsetResult == null || (getsetResult != null && getsetResult == lockValue))
                    {
                        //真正获取到锁
                        RedisHelper.Expire(key, TimeSpan.FromSeconds(timeoutSecond));
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

        }
    }
}
