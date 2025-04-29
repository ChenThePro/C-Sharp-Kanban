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
        private Dictionary<string, UserBL> _emails;
        internal UserSL Login(string email, string password)
        {
            if (_emails.ContainsKey(email))
            {
                return Login( email , password);
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

        internal UserSL Register( string password, string email)
        {
            if (_emails.ContainsKey(email))
            {
                throw new InvalidOperationException("user name is not valid");
            }
            
            else
            {
                return Register(password, email);
            }

        }
    }
}
