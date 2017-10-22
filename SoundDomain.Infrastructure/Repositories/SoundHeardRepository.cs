using RepositoryComponents.Patternts;
using RepositoryComponents.Repository;
using SoundDomain.Model.Entities;
using SoundDomain.Model.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundDomain.Infrastructure.Repositories
{
    class SoundHeardRepository : GenericRepository<SoundHeard>, ISoundHeardRepository
    {
        private SoundHeardRepository() { }

        public const string DBConfigName = "Sound.cfg.xml";

        public override NHibernate.ISession Session
        {
            get
            {
                return SessionProvider.GetNewOrCurrentSession(DBConfigName);
            }
        }
    }

    public sealed class SoundHeardRepositorySingleton
    {
        private static ISoundHeardRepository _instance;

        public static ISoundHeardRepository Instance
        {
            get { return _instance ?? SingletonBase<SoundHeardRepository>.Instance; }

            set { _instance = value; }
        }
    }
}
