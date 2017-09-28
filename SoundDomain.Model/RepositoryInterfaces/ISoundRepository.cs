using RepositoryComponents.Repository;
using SoundDomain.Model.Entities;
using SoundDomain.Model.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundDomain.Model.RepositoryInterfaces
{
    public interface ISoundRepository : IGenericRepository<Sound, int>
    {
    }
}
