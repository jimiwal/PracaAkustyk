using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryComponents.Specifications
{
    public class GroupSpecification<T, TGroup> : SearchOptionSpecification<IEnumerable<T>, IEnumerable<TGroup>>
    {
        public override bool IsSatisfiedBy(IEnumerable<T> value)
        {
            return Predicate.Compile().Invoke(value) != null;
        }
    }
}
