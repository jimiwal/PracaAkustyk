using SoundDomain.Model.ValueObjects;
using UserDomain.Model.Entities;
using UtilityComponent.Entities;

namespace SoundDomain.Model.Entities
{
    public class SoundSetting : GenericEntity
    {
        public virtual Sound Sound { get; set; }
        public virtual User User { get; set; }
    }
}
