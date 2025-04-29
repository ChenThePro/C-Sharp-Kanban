using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.ServiceLayer
{
    public class UserSL
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public List<object> Boards { get; set; }

        public UserSL(string username, string password, string email, List<object> boards)
        {
            Username = username;
            Password = password;
            Email = email;
            Boards = boards;
        }
    }
}
