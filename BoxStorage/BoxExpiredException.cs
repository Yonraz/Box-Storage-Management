using System;
using System.Runtime.Serialization;

namespace BoxStorage
{
    [Serializable]
    public class BoxExpiredException : Exception
    {
        public BoxExpiredException()
        {
        }

        public BoxExpiredException(string message) : base(message)
        {
        }

        public BoxExpiredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BoxExpiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}