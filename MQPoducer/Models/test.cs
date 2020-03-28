using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MQPoducer.Models
{
    [Table("test")]
    public class test
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string name { get; set; }
    }
}
