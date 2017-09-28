using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Model.Entities;
using UserDomain.Model.RepositoryInterfaces;
using UserDomain.Services.ServiceLayer;

namespace UserDomain.Services.DomainLayer
{
    public class UserService : IUserService
    {
        public IUserRepository UserRepository { get; set; }

        public static User SelectedUser { get; set; }

        public void Create(User user)
        {
            UserRepository.Create(user);
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(User user)
        {
            UserRepository.Delete(user);
        }

        public User Get(int id)
        {
            return UserRepository.Get(id);
        }
        public IEnumerable<User> Get(string name, string lastName, string email, string phoneNumber)
        {
            return UserRepository.Get(name, lastName, email, phoneNumber);
        }
    }
}
