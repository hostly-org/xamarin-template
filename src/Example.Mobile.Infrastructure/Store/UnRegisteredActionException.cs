using System;
using System.Runtime.Serialization;

namespace Example.Mobile.Infrastructure.Store
{
    public class UnRegisteredActionException : StoreException
    {
        public UnRegisteredActionException()
        {
        }

        public UnRegisteredActionException(string message) : base(message)
        {
        }

        public UnRegisteredActionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnRegisteredActionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
