using System;
using System.Runtime.Serialization;

namespace Application.Common.Exceptions
{
    [Serializable]
    public class ContractCallException : Exception
    {
        public ContractCallException() : base()
        {
        }

        public ContractCallException(string message)
                : base(message)
        {
        }

        protected ContractCallException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
