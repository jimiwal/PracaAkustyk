using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryComponents.NamedQuery
{
    public interface INamedQueryFactory
    {
        /// <summary>
        /// Looks up and finds the Query Object for a named query.
        /// </summary>
        /// <typeparam name="TNamedQuery">Interface of the query object for the named query</typeparam>
        /// <returns></returns>
        TNamedQuery GetNamedQuery<TNamedQuery>() where TNamedQuery : INamedQuery;
    }
}
