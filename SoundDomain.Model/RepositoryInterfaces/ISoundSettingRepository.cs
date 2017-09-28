using RepositoryComponents.Repository;
using SoundDomain.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundDomain.Model.RepositoryInterfaces
{
    public interface ISoundSettingRepository : IGenericRepository<SoundSetting, int>
    {
    }
}
