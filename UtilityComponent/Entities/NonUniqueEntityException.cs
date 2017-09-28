using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UtilityComponent.Entities
{
    public class NonUniqueEntityException : Exception
    {
        public NonUniqueEntityException() { }
        public NonUniqueEntityException(string message) : base(message) { }
        public NonUniqueEntityException(string message, Exception inner) : base(message, inner) { }
        protected NonUniqueEntityException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
