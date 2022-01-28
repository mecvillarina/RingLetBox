using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Dtos.ApiRequest
{
    public class SmartContractsPropertyCallRequestDto 
    {
        public string ContractAddress { get; set; }
        public string PropertyName { get; set; }
        public string Sender { get; set; }
    }
}
