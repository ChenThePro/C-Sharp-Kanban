using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DAL
{
    internal class UserDAL
    {
        public string Email;
        public string Password;


        public UserDAL(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }
    }
}
