using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryComponents.Specifications
{
    public class SortSpecification<T> : SearchOptionSpecification<T, object>
    {
        public ListSortDirection Direction { get; set; }

        public override bool IsSatisfiedBy(T value)
        {
            return Predicate.Compile().Invoke(value) != null;
        }
    }
}
