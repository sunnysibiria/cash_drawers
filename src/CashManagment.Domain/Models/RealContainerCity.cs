using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CashManagment.Domain.Models
{
    public class RealContainerCity
    {
        public int IdType { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        [JsonProperty(PropertyName = "Id")]
        public Guid Guid { get; set; }
    }
}
