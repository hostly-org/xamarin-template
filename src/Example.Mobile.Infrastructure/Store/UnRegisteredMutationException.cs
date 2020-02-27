using System;
using System.Runtime.Serialization;

namespace Example.Mobile.Infrastructure.Store
{
    public class UnRegisteredMutationException : ModuleException
    {
        public UnRegisteredMutationException()
        {
        }

        public UnRegisteredMutationException(string message) : base(message)
        {
        }

        public UnRegisteredMutationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnRegisteredMutationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
