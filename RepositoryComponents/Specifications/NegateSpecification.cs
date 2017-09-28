using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryComponents.Specifications
{
    public class NegateSpecification<T> : Specification<T>
    {
        private readonly AbstractSpecification<T> spec;

        public NegateSpecification(AbstractSpecification<T> spec)
        {
            this.spec = spec;
        }

        public override Expression<Func<T, bool>> IsSatisfiedBy()
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            return Expression.Lambda<Func<T, bool>>(
                Expression.Not(spec.IsSatisfiedBy().Body), spec.IsSatisfiedBy().Parameters);
        }

        protected override object[] Parameters
        {
            get
            {
                return new[] { spec };
            }
        }
    }
}
