using SoundDomain.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Model.Entities;
using UtilityComponent.Entities;

namespace MeasurementDomain.Model.Entities
{
    public class Measurement : GenericEntity
    {
        public Measurement()
        {
            Sounds = new List<SoundHeard>();
        }
        public virtual string Name { get; set; }
        public virtual IList<SoundHeard> Sounds { get; set; }
        public virtual User User { get; set; }
        public virtual DateTime DateTime { get; set; }
    }
}
