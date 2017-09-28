using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UtilityComponent.Entities
{
    [Serializable]
    public abstract class GenericValueObject : ValidatableObject
    {
        protected override IEnumerable<PropertyInfo> GetTypeSpecificSignatureProperties()
        {
            return GetType().GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(DomainSignatureAttribute), true));
        }

        public static bool operator ==(GenericValueObject valueObject1, GenericValueObject valueObject2)
        {
            if ((object)valueObject1 == null)
                return (object)valueObject2 == null;
            // now we can do "normal" comparison.
            // we must force to call base version of Equals methods, otherwise we end up in infinitive recursion
            return ((object)valueObject1).Equals(valueObject2);
        }

        public static bool operator !=(GenericValueObject valueObject1, GenericValueObject valueObject2)
        {
            return !(valueObject1 == valueObject2);
        }

        public virtual bool Equals(GenericValueObject other)
        {
            return this == other;
        }

    }
}
