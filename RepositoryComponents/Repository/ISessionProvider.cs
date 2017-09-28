using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using NHibernate;

namespace RepositoryComponents.Repository
{
    public interface ISessionProvider
    {
        Configuration DefaultConfiguration { get; }
        Configuration GetSessionConfiguration(string name);

        ISessionFactory GetSessionFactory();
        ISessionFactory GetSessionFactory(string name);

        ISession GetNewSession();
        ISession GetNewSession(string name);
        ISession GetNewOrCurrentSession();
        ISession GetNewOrCurrentSession(string name);
    }
}
