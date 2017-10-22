using MeasurementDomain.Model.Entities;
using RepositoryComponents.Repository;
using RepositoryComponents.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasurementDomain.Model.RepositoryInterfaces
{
    public interface IMeasurementRepository : IGenericRepository<Measurement, int>
    {
        Measurement Get(int id);
        List<Measurement> GetAll();
    }
}
