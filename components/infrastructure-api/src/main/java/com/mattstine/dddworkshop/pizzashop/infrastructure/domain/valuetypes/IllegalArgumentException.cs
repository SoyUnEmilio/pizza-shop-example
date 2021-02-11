using System;
using System.Runtime.Serialization;

namespace com.mattstine.dddworkshop.pizzashop.infrastructure.domain.valuetypes
{
    [Serializable]
    internal class IllegalArgumentException : Exception
    {
        public IllegalArgumentException()
        {
        }

        public IllegalArgumentException(string message) : base(message)
        {
        }

        public IllegalArgumentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}