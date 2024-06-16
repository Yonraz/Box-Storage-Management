using System;
using System.Runtime.Serialization;

namespace BoxStorage
{
    [Serializable]
    internal class BoxDoesntExistException : Exception
    {
        public BoxDoesntExistException()
        {
        }

        public BoxDoesntExistException(string message) : base(message)
        {
        }

        public BoxDoesntExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BoxDoesntExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}