using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityComponent.Entities;

namespace UserDomain.Model.Entities
{
    public class User : GenericEntity
    {
        public virtual string Name { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Address { get; set; }
        public virtual string Custom1 { get; set; }
        public virtual string Custom2 { get; set; }
        public virtual string Custom3 { get; set; }
    }
}
