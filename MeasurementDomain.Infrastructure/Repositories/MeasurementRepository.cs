using MeasurementDomain.Model.Entities;
using MeasurementDomain.Model.RepositoryInterfaces;
using RepositoryComponents.Patternts;
using RepositoryComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasurementDomain.Infrastructure.Repositories
{
    public class MeasurementRepository : GenericRepository<Measurement>, IMeasurementRepository
    {
        public const string MeasurementDBConfigName = "Measurement.cfg.xml";

        private MeasurementRepository()
        {

        }

        public override NHibernate.ISession Session
        {
            get
            {
                return SessionProvider.GetNewOrCurrentSession(MeasurementDBConfigName);
            }
        }

        public Measurement Get(int id)
        {
            return base.Find(id);
        }

        public List<Measurement> GetAll()
        {
            return base.FindAll().ToList();
        }
    }

    public sealed class MeasurementRepositorySingleton
    {
        private static IMeasurementRepository _instance;

        public static IMeasurementRepository Instance
        {
            get { return _instance ?? SingletonBase<MeasurementRepository>.Instance; }

            set { _instance = value; }
        }
    }
}
