using System;
using System.Collections.Generic;
using System.Text;

namespace MQTransaction.Common.Models.Entity
{
    public class base_entity
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public DateTime create_time { get; set; } = DateTime.Now;
    }
}
