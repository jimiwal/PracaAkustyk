using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryComponents.NamedQuery
{
    /// <summary>
    /// Interface used to wrap a named query as a Query Object. 
    /// The basic interface without any generic type for the result.
    /// Eays to use in injections.
    /// </summary>
    public interface INamedQuery
    {
        string QueryName { get; }
    }

    /// <summary>
    /// Interface used to wrap a named query as a Query Object.
    /// An extension to the basic defining resulting type and basic operations.
    /// </summary>
    public interface INamedQuery<TResult> : INamedQuery
    {
        TResult Execute();
        int MaxResult { get; set; }
    }
}
