using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Interfaces
{
    /// <summary>
    /// Register约束
    /// </summary>
    public interface IMqTransRegister
    {
        void AddServices(IServiceCollection services);
    }
}
