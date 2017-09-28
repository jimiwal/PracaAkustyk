using RepositoryComponents.Patternts;
using RepositoryComponents.Repository;
using SoundDomain.Model.Entities;
using SoundDomain.Model.RepositoryInterfaces;
using SoundDomain.Model.ValueObjects;

namespace SoundDomain.Infrastructure.Repositories
{
    public class SoundRepository : GenericRepository<Sound>, ISoundRepository
    {
        private SoundRepository() { }

        private static string className = "SoundRepository";

        public const string DBConfigName = "Sound.cfg.xml";        

        public override NHibernate.ISession Session
        {
            get
            {
                return SessionProvider.GetNewOrCurrentSession(DBConfigName);
            }
        }
    }

    public sealed class SoundRepositorySingleton
    {
        private static ISoundRepository _instance;

        public static ISoundRepository Instance
        {
            get { return _instance ?? SingletonBase<SoundRepository>.Instance; }

            set { _instance = value; }
        }
    }
}
