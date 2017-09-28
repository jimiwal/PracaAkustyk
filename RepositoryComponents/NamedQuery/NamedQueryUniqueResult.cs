using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryComponents.NamedQuery
{
    public class NamedQueryUniqueResult<TResult> : NamedQueryAutoSetParameters<TResult>
    {
        protected override TResult Execute(IQuery query)
        {
            return query.UniqueResult<TResult>();
        }
    }
}
