using SoundDomain.Model.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Model.Entities;
using UtilityComponent.Entities;

namespace SoundDomain.Model.Entities
{
    public class SoundSequence : GenericEntity
    {
        public SoundSequence()
        {
            Sounds = new List<SoundSetting>();
        }
        public virtual string Name { get; set; }
        public virtual IList<SoundSetting> Sounds { get; set; }
        public virtual User User { get; set; }
    }
}
