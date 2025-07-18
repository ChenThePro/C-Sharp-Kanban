﻿using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserSL
    {
        public string Email { get; init; }
        public string Password { get; set; }
        public List<BoardSL> Boards { get; set; }
        public bool IsDark { get; set; }

        public UserSL(string email, string password, List<BoardSL> boards, bool isDark)
        {
            Email = email;
            Password = password;
            Boards = boards;
            IsDark = isDark;
        }
    }
}