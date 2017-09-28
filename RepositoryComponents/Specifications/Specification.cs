using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryComponents.Specifications
{
    public class Specification<T> : AbstractSpecification<T>
    {

        public Specification() { }

        public Specification(Expression<Func<T, bool>> predicate)
        {
            this.Predicate = predicate;
        }

        public override Expression<Func<T, bool>> IsSatisfiedBy()
        {
            return this.Predicate;
        }

        public Expression<Func<T, bool>> Predicate { get; set; }
    }
}
