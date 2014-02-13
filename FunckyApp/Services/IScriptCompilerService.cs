using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace FunckyApp.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IScriptCompilerService" in both code and config file together.
    [ServiceContract(Namespace = "http://schemas.datacontract.org/2004/07/FunckyApp.Services", Name="CompilerServices") ]
    public interface IScriptCompilerService
    {
        [OperationContract]
        GetScriptStatsResponse GetScriptStats(GetTokenStatisticsRequest request);
    }
}
