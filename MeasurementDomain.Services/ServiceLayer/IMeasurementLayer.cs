using MeasurementDomain.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Model.Entities;

namespace MeasurementDomain.Services.ServiceLayer
{
    public interface IMeasurementLayer
    {
        void GetMeasuremetns();
        IList<Measurement> GetMeasurementsForUser(User user);
    }
}
