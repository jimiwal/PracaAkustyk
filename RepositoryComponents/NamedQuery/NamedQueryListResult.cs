using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryComponents.NamedQuery
{
    public class NamedQueryListResult<TResult> : NamedQueryAutoSetParameters<TResult>
    {
        protected override TResult Execute(NHibernate.IQuery query)
        {
            return (TResult)query.List();
        }
    }
}
