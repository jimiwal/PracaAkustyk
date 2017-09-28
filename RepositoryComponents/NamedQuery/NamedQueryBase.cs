using NHibernate;
using RepositoryComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryComponents.NamedQuery
{
    public abstract class NamedQueryBase<TResult> : INamedQuery<TResult>
    {
        private readonly ISession session = SessionProvider.GetNewOrCurrentSession();
        public int MaxResult { get; set; }

        /// <summary>
        /// Coordinates the execution flow.
        /// </summary>
        /// <returns></returns>
        public virtual TResult Execute()
        {
            var nhQuery = GetNamedQuery();
            SetParameters(nhQuery);
            SetMaxResult(nhQuery);
            return Execute(nhQuery);
        }

        /// <summary>
        /// Override to execute the query. Often just a call to query.List() or query.UniqueResult().
        /// </summary>
        /// <returns></returns>
        protected abstract TResult Execute(IQuery query);

        /// <summary>
        /// Returns the query.
        /// </summary>
        /// <returns></returns>
        protected virtual IQuery GetNamedQuery()
        {
            return session.GetNamedQuery(this.QueryName);
        }

        /// <summary>
        /// Sets the maximun number of records to retrieve.
        /// </summary>
        /// <param name="query"></param>
        protected virtual void SetMaxResult(IQuery query)
        {
            if (MaxResult > 0)
                query.SetMaxResults(MaxResult);
        }

        /// <summary>
        /// Override to set the parameters in the query.
        /// </summary>
        /// <param name="nhQuery"></param>
        protected abstract void SetParameters(IQuery nhQuery);

        /// <summary>
        /// Returns the name of the query. The convetion is that the Query Object and the Named Query
        /// share the same name.
        /// </summary>
        /// <returns></returns>
        public virtual string QueryName
        {
            get { return GetType().Name; }
        }
    }
}
