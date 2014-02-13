using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace FunckyApp.Services
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/FunckyApp.Models")]
    public class GetTokenStatisticsRequest
    {
        [DataMember]
        public string Script { get; set; }
    }
}