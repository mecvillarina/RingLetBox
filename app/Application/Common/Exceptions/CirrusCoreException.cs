using System;
using System.Runtime.Serialization;

namespace Application.Common.Exceptions
{
    [Serializable]
    public class CirrusCoreException : Exception
    {
        public CirrusCoreException(string message)
                : base(message)
        {
        }

        protected CirrusCoreException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
