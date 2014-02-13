using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace FunckyApp.Services
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/FunckyApp.Models")]
    public class GetScriptStatsResponse
    {
        [DataMember]
        public List<LiteralStats> NumericLiteralStatistics { get; set; }
        [DataMember]
        public List<LiteralStats> StringLiteralStatistics { get; set; }
        [DataMember]
        public List<IdentifierStats> IdentifierStatistics { get; set; }
        [DataMember]
        public bool IsValid { get; set; }
        [DataMember]
        public List<string> InvalidTokens { get; set; } 
    }

    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/FunckyApp.Models")]
    public class StatsInfoBase
    {
        [DataMember]
        public int Count {get; set;}
    }

    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/FunckyApp.Models")]
    public class LiteralStats : StatsInfoBase
    {
        [DataMember]
        public string Value;
    }

    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/FunckyApp.Models")]
    public class IdentifierStats : StatsInfoBase
    {
        [DataMember]
        public string Name { get; set; }
        
    }
}