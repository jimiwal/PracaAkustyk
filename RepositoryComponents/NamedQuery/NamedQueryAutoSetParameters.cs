using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryComponents.NamedQuery
{
    public abstract class NamedQueryAutoSetParameters<TResult> : NamedQueryBase<TResult>
    {
        protected override void SetParameters(NHibernate.IQuery nhQuery)
        {
            Type queryType = this.GetType();
            foreach (var propertyInfo in queryType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (propertyInfo.Name != "QueryName" && propertyInfo.Name != "MaxResult")
                {
                    nhQuery.SetParameter(propertyInfo.Name, propertyInfo.GetValue(this, null));
                }
            }
        }
    }
}
