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
    public class SoundSettingRepository : GenericRepository<SoundSetting>, ISoundSettingRepository
    {
        private SoundSettingRepository() { }

        private static string className = "SoundSetting";

        public const string DBConfigName = "Sound.cfg.xml";

        public override NHibernate.ISession Session
        {
            get
            {
                return SessionProvider.GetNewOrCurrentSession(DBConfigName);
            }
        }
    }
    public sealed class SoundSettingRepositorySingleton
    {
        private static ISoundSettingRepository _instance;

        public static ISoundSettingRepository Instance
        {
            get { return _instance ?? SingletonBase<SoundSettingRepository>.Instance; }

            set { _instance = value; }
        }
    }
}
