using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryComponents.Specifications
{
    public class OrSpecification<T> : Specification<T>
    {
        private readonly AbstractSpecification<T> spec1;
        private readonly AbstractSpecification<T> spec2;

        public OrSpecification(AbstractSpecification<T> spec1, AbstractSpecification<T> spec2)
        {
            this.spec1 = spec1;
            this.spec2 = spec2;
        }

        protected override object[] Parameters
        {
            get
            {
                return new object[] { spec1, spec2 };
            }
        }

        public override Expression<Func<T, bool>> IsSatisfiedBy()
        {

            var expr1 = spec1.IsSatisfiedBy();
            var expr2 = spec2.IsSatisfiedBy();

            ParameterExpression param = expr1.Parameters[0];
            if (ReferenceEquals(param, expr2.Parameters[0]))
            {
                return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, expr2.Body), param);
            }
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, Expression.Invoke(expr2, param)), param);
        }

    }
}
