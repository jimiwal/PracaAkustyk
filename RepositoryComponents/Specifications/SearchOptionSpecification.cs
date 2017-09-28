using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryComponents.Specifications
{
    public abstract class SearchOptionSpecification<T, TResult>
    {
        public abstract bool IsSatisfiedBy(T value);
        public Expression<Func<T, TResult>> Predicate { get; set; }
    }
}
