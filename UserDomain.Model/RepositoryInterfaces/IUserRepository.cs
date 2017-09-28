using RepositoryComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Model.Entities;

namespace UserDomain.Model.RepositoryInterfaces
{
    public interface IUserRepository : IGenericRepository<User, int>
    {
        void Create(User user);

        void Delete(User user);

        void Update(User user);

        User Get(int id);

        IEnumerable<User> Get(string name, string lastName, string email, string phoneNumber);

        new IList<User> FindAll();
    }
}
