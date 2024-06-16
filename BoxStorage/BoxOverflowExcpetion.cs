using System;
using System.Runtime.Serialization;

namespace BoxStorage
{
    [Serializable]
    internal class BoxOverflowExcpetion : Exception
    {
        public BoxOverflowExcpetion()
        {
        }

        public BoxOverflowExcpetion(string message) : base(message)
        {
        }

        public BoxOverflowExcpetion(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BoxOverflowExcpetion(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}