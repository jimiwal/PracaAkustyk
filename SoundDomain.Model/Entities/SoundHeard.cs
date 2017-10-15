using SoundDomain.Model.ValueObjects;
using UtilityComponent.Entities;

namespace SoundDomain.Model.Entities
{
    public class SoundHeard : GenericEntity
    {
        //public virtual Measurement Measurement { get; set; }
        public virtual Sound Sound { get; set; }
        public virtual long Answer { get; set; }
    }
}
