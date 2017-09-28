using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryComponents.Repository
{
    public class SessionProvider
    {
        public const string DefaultSessionName = "DefaultNHSession";

        private static Dictionary<string, ConfiguredSessionFactory> sessionFactories;

        static SessionProvider()
        {
            sessionFactories = new Dictionary<string, ConfiguredSessionFactory>();
            // Initialize

            //// Note: Set the BuildAction property of each mapping file to "Embedded Resource"
            //configuration = new NHibernate.Cfg.Configuration().Configure();
            //// Create session factory from configuration object
            //defaultSessionFactory = configuration.BuildSessionFactory();
        }

        public static NHibernate.Cfg.Configuration DefaultConfiguration
        {
            get
            {
                ConfiguredSessionFactory sessionFactory = FindOrCreateSessionFactory(DefaultSessionName);
                return sessionFactory.Configuration;
            }
        }

        /// <summary>
        /// Returns singleton factory created from 1) app.config or 2) hibernate.cfg.xml
        /// </summary>
        /// <returns></returns>
        public static ISessionFactory GetSessionFactory()
        {
            return FindOrCreateSessionFactory(DefaultSessionName).SessionFactory;
        }

        /// <summary>
        /// Returns singleton factory created from given config name
        /// </summary>
        /// <returns></returns>
        public static ISessionFactory GetSessionFactory(string name)
        {
            return FindOrCreateSessionFactory(name).SessionFactory;
        }

        /// <summary>
        /// Get new session
        /// </summary>
        /// <returns></returns>
        public static ISession GetNewSession()
        {
            return GetNewSession(DefaultSessionName);
        }

        /// <summary>
        /// Get new session
        /// </summary>
        /// <returns></returns>
        public static void CloseCurrentSession()
        {
            CloseCurrentSession(DefaultSessionName);
        }

        /// <summary>
        /// Get new session
        /// </summary>
        /// <returns></returns>
        public static void CloseCurrentSession(string factoryName)
        {
            if (sessionFactories.ContainsKey(factoryName))
                sessionFactories[factoryName].CloseCurrentSession();
        }

        /// <summary>
        /// Get new session
        /// </summary>
        /// <returns></returns>
        public static ISession GetNewSession(string name)
        {
            return FindOrCreateSessionFactory(name).SessionFactory.OpenSession();
        }

        /// <summary>
        /// Get new or open session from the default session factory
        /// </summary>
        /// <returns></returns>
        public static ISession GetNewOrCurrentSession()
        {
            System.Diagnostics.Trace.WriteLine("GetNewOrCurrentSession " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + " " + System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            return GetNewOrCurrentSession(DefaultSessionName);
        }

        /// <summary>
        /// Get new or open session from named session factory
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ISession GetNewOrCurrentSession(string name)
        {
            ISession session = null;

            /* not used in Matris now T.R. 2011-12-28
            #region Used only for WCF
            // Used only for WCF
            if (OperationContext.Current != null)
            {
                var contextManager = OperationContext.Current.InstanceContext.Extensions.Find<NHibernateContextExtension>();
                if (contextManager == null)
                {
                    throw new InvalidOperationException(@"There is no context manager available.");
                }
                session = contextManager.Session;
                return session;
            }
            #endregion
             */

            //ISessionFactory sessionFactory = FindOrCreateSessionFactory(name).SessionFactory;


            //if (CanBindToContext)
            //{
            //    if (!CurrentSessionContext.HasBind(sessionFactory))
            //    {
            //        session = sessionFactory.OpenSession();
            //        CurrentSessionContext.Bind(session);
            //    }
            //    else
            //    {
            //        session = sessionFactory.GetCurrentSession();
            //        if (!session.IsOpen)
            //            session = sessionFactory.OpenSession();
            //    }
            //}
            //else
            //    session = sessionFactory.OpenSession();

            session = FindOrCreateSessionFactory(name).GetNewOrCurrentSession();

            return session;
        }

        public static NHibernate.Cfg.Configuration GetSessionConfiguration(string name)
        {
            return FindOrCreateSessionFactory(name).Configuration;
        }

        public static bool CanBindToContext
        {
            // we can bind to session context if we have only one factory in use!!!
            get { return sessionFactories.Count <= 1; }
        }

        private static ConfiguredSessionFactory FindOrCreateSessionFactory(string name)
        {
            if (!sessionFactories.ContainsKey(name))
            {
                ConfiguredSessionFactory sessionFactory;
                if (name.Equals(DefaultSessionName))
                    sessionFactory = new ConfiguredSessionFactory();
                else
                    sessionFactory = new ConfiguredSessionFactory(name);

                sessionFactories.Add(name, sessionFactory);
            }
            return sessionFactories[name];
        }
    }


    public class ConfiguredSessionFactory
    {
        public ConfiguredSessionFactory()
        {
            ConfigName = SessionProvider.DefaultSessionName;

            // use app.config or hibernate.cfg.xml to configure
            Configuration = new NHibernate.Cfg.Configuration().Configure();

            // Create session factory from configuration object
            SessionFactory = Configuration.BuildSessionFactory();
        }

        public ConfiguredSessionFactory(string configName)
        {
            ConfigName = configName;

            Configuration = new NHibernate.Cfg.Configuration().Configure(ConfigName);
            // Create session factory from configuration object
            SessionFactory = Configuration.BuildSessionFactory();
        }
        public ISession CurrentSession { get; private set; }
        public string ConfigName { get; private set; }
        public ISessionFactory SessionFactory { get; private set; }
        public NHibernate.Cfg.Configuration Configuration { get; private set; }

        public ISession GetNewOrCurrentSession()
        {
            if (CurrentSession == null || !CurrentSession.IsOpen)
                CurrentSession = SessionFactory.OpenSession();

            return CurrentSession;
        }

        public void CloseCurrentSession()
        {
            if (CurrentSession != null)
            {
                CurrentSession.Close();
                CurrentSession.Dispose();
                CurrentSession = null;
            }
        }
    }
}
