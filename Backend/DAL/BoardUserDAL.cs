using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DAL
{
    internal class BoardUserDAL
    {
        public int id;
        public string email;

        public BoardUserDAL(string email, int id)
        {
            this.email = email;
            this.id = id;
        }
    }
}
