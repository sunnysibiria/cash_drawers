using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CashManagment.Api.Extensions
{
    [DataContract]
    public class ServiceError
    {
        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }

#if DEBUG
        [DataMember(Name = "errorDetail")]
        public string ErrorDetail { get; set; }
#endif
    }
}
