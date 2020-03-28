using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MQTransaction.Interfaces
{
    public interface IBaseConsumer
    {
        Task<bool> DealMessage(string message);
    }
}
