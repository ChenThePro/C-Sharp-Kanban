using Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.BuisnessLayer
{
    internal class UserFacade
    {
        private Dictionary<string, UserBL> _users;
        internal UserSL Login(string username, string password)
        {
            if (_users.ContainsKey(username))
            {
                return _users[username].Login(password);
            }
            else
            {
                throw new Exception("user doesnt exist");
            }
        }

        internal void Logout()
        {
            throw new NotImplementedException();
        }

        internal UserSL Register(string username, string password, string email)
        {
            if (_users.ContainsKey(username))
            {
                throw new InvalidOperationException("user name is not valid");
            }
            else
            {
                return Register(username, password, email);
            }
        }
    }
}
