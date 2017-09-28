using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UtilityComponent.Entities
{
    public class EntityValidationException : Exception
    {
        public EntityValidationException() { }
        public EntityValidationException(string message) : base(message) { }
        public EntityValidationException(string message, Exception inner) : base(message, inner) { }
        protected EntityValidationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
