using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Model.Entities;

namespace UserDomain.Services.ServiceLayer
{
    public interface IUserService
    {
        void Create(User user);

        void Delete(User user);

        void Update(User user);

        User Get(int id);

        IEnumerable<User> Get(string name, string lastName, string email, string phoneNumber);
    }
}
