using System;
using System.Runtime.Serialization;

namespace BoxStorage
{
    [Serializable]
    internal class BoxUnderflowException : Exception
    {
        public BoxUnderflowException()
        {
        }

        public BoxUnderflowException(string message) : base(message)
        {
        }

        public BoxUnderflowException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BoxUnderflowException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}