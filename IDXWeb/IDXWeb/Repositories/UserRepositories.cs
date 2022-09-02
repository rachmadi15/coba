using System.Collections.Generic;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Persistence;

namespace IDXWeb.Repositories
{
    public class UserRepository
    {
        private readonly Database _database;

        public UserRepository()
        {
            _database = new Database("umbracoDbDSN");
        }

        public IList<User> GetAll()
        {
            return _database.Fetch<User>("SELECT * FROM [umbracoUser] ORDER BY userName Desc");
        }

        public IList<User> GetByUsername(string username)
        {
            string sqlQuery = @"SELECT * FROM [umbracoUser] WHERE userLogin = @param1 ORDER BY userName Desc";
            return _database.Fetch<User>(sqlQuery, new {
                param1 = username
            });
        }

        public IList<User> GetByEmail(string email)
        {
            string sqlQuery = @"SELECT * FROM [umbracoUser] WHERE userEmail = @param1 ORDER BY userName Desc";
            return _database.Fetch<User>(sqlQuery, new {
                param1 = email
            });
        }

        public User GetUser(string username) {
            string sqlQuery = @"SELECT * FROM [umbracoUser] WHERE userLogin = @param1 ORDER BY userName Desc";
            return _database.First<User>(sqlQuery, new {
                param1 = username
            });
        }

        public User GetUserById(int id)
        {
            return _database.First<User>(string.Format(@"SELECT * FROM [umbracoUser] WHERE id = '{0}' ORDER BY userName Desc", id));
        }

        public void Update(User user)
        {
            _database.Update(user);
        }
    }
}