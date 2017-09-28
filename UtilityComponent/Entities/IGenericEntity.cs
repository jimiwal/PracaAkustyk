using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UtilityComponent.Entities
{
    public interface IGenericEntity<TId>
    {
        /// <summary>
        /// Id of an entity
        /// </summary>
        TId Id { get; set; }
        /// <summary>
        /// Timestamp in NHibernate
        /// </summary>
        int Version { get; set; }

        bool IsTransient();

        IEnumerable<PropertyInfo> GetSignatureProperties();
    }
}
