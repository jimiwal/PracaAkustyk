using RepositoryComponents.Specifications;
using System.ComponentModel;

namespace RepositoryComponents.Repository
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