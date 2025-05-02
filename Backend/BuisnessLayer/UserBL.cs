using Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.BuisnessLayer
{
    internal class UserBL
    {
        private bool loggedIn;
        internal string email;
        internal string password;
        internal List<BoardBL> boards;

        internal UserBL(string email, string password)
        {
            this.email = email;
            this.password = password;
            boards = new List<BoardBL>();
            loggedIn = true;
        }

        internal UserBL Login(string password)
        {
            if (this.password == password)
            {
                loggedIn = true;
                return this;
            }
            throw new UnauthorizedAccessException("password incorrect");
        }

        internal void Logout()
        {
            loggedIn = false;
        }
    }
}
