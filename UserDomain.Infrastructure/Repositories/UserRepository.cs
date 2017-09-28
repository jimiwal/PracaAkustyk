using MeasurementDomain.Infrastructure.Repositories;
using RepositoryComponents.Patternts;
using RepositoryComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Model.Entities;
using UserDomain.Model.RepositoryInterfaces;

namespace UserDomain.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private static string className = "UserRepository";
        
        public const string DBConfigName = "User.cfg.xml";

        private UserRepository() { }

        public override NHibernate.ISession Session
        {
            get
            {
                return SessionProvider.GetNewOrCurrentSession(DBConfigName);
            }
        }

        public void Create(User user)
        {
            Save(user);
            Flush();
        }

        public void Delete(User user)
        {
            Remove(user);
            Flush();
        }

        public User Get(int id)
        {
            var query = Query();// Session.QueryOver<User>();

            query = query.Where(f => f.Id==id);

            return query.FirstOrDefault();
        }

        public IEnumerable<User> Get(string name, string lastName, string email, string phoneNumber)
        {
            var query = Query();

            if (string.IsNullOrEmpty(name) == false)
            {
                query = query.Where(f => f.Name.Contains(name));
            }

            if (string.IsNullOrEmpty(lastName) == false)
            {
                query = query.Where(f => f.LastName.Contains(lastName));
            }

            if (string.IsNullOrEmpty(email) == false)
            {
                query = query.Where(f => f.Email.Contains(email));
            }

            if (string.IsNullOrEmpty(phoneNumber) == false)
            {
                query = query.Where(f => f.Phone.Contains(phoneNumber));
            }

            return query.OrderBy(f => f.Name).ThenBy(g => g.LastName);
        }

        public void Update(User user)
        {
            Save(user);
            Flush();
        }
    }

    public sealed class UserRepositorySingleton
    {
        private static IUserRepository _instance;

        public static IUserRepository Instance
        {
            get { return _instance ?? SingletonBase<UserRepository>.Instance; }

            set { _instance = value; }
        }
    }
}