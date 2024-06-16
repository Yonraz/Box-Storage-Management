using System;
using System.Runtime.Serialization;

namespace BoxStorage
{
    [Serializable]
    public class OutOfBoxesException : Exception
    {
        public OutOfBoxesException()
        {
        }

        public OutOfBoxesException(string message) : base(message)
        {
        }

        public OutOfBoxesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OutOfBoxesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}