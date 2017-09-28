using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryComponents.Specifications
{
    public abstract class AbstractSpecification<T>
    {
        public abstract Expression<Func<T, bool>> IsSatisfiedBy();

        public static AbstractSpecification<T> operator &(AbstractSpecification<T> spec1, AbstractSpecification<T> spec2)
        {
            return new AndSpecification<T>(spec1, spec2);
        }

        public static bool operator false(AbstractSpecification<T> spec1)
        {
            return false; // no-op. & and && do exactly the same thing.
        }

        public static bool operator true(AbstractSpecification<T> spec1)
        {
            return false; // no - op. & and && do exactly the same thing.
        }

        public static AbstractSpecification<T> operator |(AbstractSpecification<T> spec1, AbstractSpecification<T> spec2)
        {
            return new OrSpecification<T>(spec1, spec2);
        }

        public static AbstractSpecification<T> operator !(AbstractSpecification<T> spec1)
        {
            return new NegateSpecification<T>(spec1);
        }

        protected virtual object[] Parameters { get { return new object[] { Guid.NewGuid() }; } }

        public bool Equals(AbstractSpecification<T> other)
        {
            return Parameters.SequenceEqual(other.Parameters);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((AbstractSpecification<T>)obj);
        }

        public override int GetHashCode()
        {
            return Parameters.GetHashCode();
        }

    }
}
