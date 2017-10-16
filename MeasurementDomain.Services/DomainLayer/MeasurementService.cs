using MeasurementDomain.Infrastructure.Repositories;
using MeasurementDomain.Model.Entities;
using MeasurementDomain.Model.RepositoryInterfaces;
using MeasurementDomain.Services.ServiceLayer;
using NHibernate.Transform;
using RepositoryComponents.Patternts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Model.Entities;

namespace MeasurementDomain.Services.DomainLayer
{
    public class MeasurementService : IMeasurementLayer
    {
        IMeasurementRepository MeasurementRepository;

        private MeasurementService()
        {
            MeasurementRepository = MeasurementRepositorySingleton.Instance;
        }

        static MeasurementService()
        {
            LastMeasurementName = string.Empty;
        }

        public void GetMeasuremetns()
        {
            var item = MeasurementRepository.GetAll().FirstOrDefault();
            item.Name = "Heheszki";
            MeasurementRepository.Save(item);
            MeasurementRepository.Flush();
        }

        public IList<Measurement> GetMeasurementsForUser(User user)
        {
            IList<Measurement> userMeasurements;
            var query = MeasurementRepositorySingleton.Instance.Session.QueryOver<Measurement>();
            query.Where(r => r.User.Id == user.Id);

            userMeasurements = query.TransformUsing(Transformers.DistinctRootEntity)
                                .List<Measurement>();

            return userMeasurements;
        }

        public void RemoveMeasuremant(Measurement measurement)
        {
            MeasurementRepository.Remove(measurement);
            MeasurementRepository.Flush();
        }

        public static string LastMeasurementName { get; set; }
    }


    public sealed class MeasurementServiceSingleton
    {
        private static IMeasurementLayer _instance;

        public static IMeasurementLayer Instance
        {
            get { return _instance ?? SingletonBase<MeasurementService>.Instance; }

            set { _instance = value; }
        }
    }
}
