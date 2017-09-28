using MeasurementDomain.Infrastructure.Repositories;
using MeasurementDomain.Model.Entities;
using MeasurementDomain.Model.RepositoryInterfaces;
using MeasurementDomain.Services.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasurementDomain.Services.DomainLayer
{
    public class MeasurementService : IMeasurementLayer
    {
        IMeasurementRepository MeasurementRepository;

        public MeasurementService()
        {
            MeasurementRepository = MeasurementRepositorySingleton.Instance;
        }
        public void GetMeasuremetns()
        {
            var item = MeasurementRepository.GetAll().FirstOrDefault();
            item.Name = "Heheszki";
            MeasurementRepository.Save(item);
            MeasurementRepository.Flush();
        }
    }
}
