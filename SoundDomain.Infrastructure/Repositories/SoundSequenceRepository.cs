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
    public class SoundSequenceRepository : GenericRepository<SoundSequence>, ISoundSequenceRepository
    {
        private SoundSequenceRepository() { }

        private static string className = "SoundSequenceRepository";

        public const string DBConfigName = "Sound.cfg.xml";

        public override NHibernate.ISession Session
        {
            get
            {
                return SessionProvider.GetNewOrCurrentSession(DBConfigName);
            }
        }
    }

    public sealed class SoundSequenceRepositorySingleton
    {
        private static ISoundSequenceRepository _instance;

        public static ISoundSequenceRepository Instance
        {
            get { return _instance ?? SingletonBase<SoundSequenceRepository>.Instance; }

            set { _instance = value; }
        }
    }
}
