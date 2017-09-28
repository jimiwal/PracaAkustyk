using UtilityComponent.Entities;

namespace SoundDomain.Model.ValueObjects
{
    public class Sound : GenericValueObject
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }
        public virtual double Frequency { get; set; }
        public virtual double Volume { get; set; }
    }
}
