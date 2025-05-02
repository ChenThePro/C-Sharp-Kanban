using Backend.BuisnessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.ServiceLayer
{
    public class UserSL
    {
        private readonly string _password;
        public string Email { get; set; }
        public List<BoardSL> Boards { get; set; }

         internal UserSL(UserBL user)
         {
            _password = user.password;
            Email = user.email;
            Boards = new List<BoardSL>();
         }
    }
}
