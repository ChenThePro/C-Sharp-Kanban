using IntroSE.Kanban.Backend.DAL;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class BoardUserDTO : IDTO
    {
        internal const string BOARD_ID_COLUMN_NAME = "id";
        internal const string BOARD_USER_EMAIL_COLUMN_NAME = "email";
        private int _id;
        private string _email;
        private readonly BoardUserController _controller;

        internal int Id => _id;

        internal string Email
        {
            get => _email;
            set { _controller.Update(BOARD_ID_COLUMN_NAME, _id, BOARD_USER_EMAIL_COLUMN_NAME, value); _email = value; }
        }

        internal BoardUserDTO(string email, int id)
        {
            _controller = new BoardUserController();
            _id = id;
            _email = email;
        }

        internal void Insert()
        {
            _controller.Insert(this);
        }

        internal void Delete()
        {
            _controller.Delete(BOARD_ID_COLUMN_NAME, Id);
        }

        internal List<BoardUserDTO> SelectAll()
        {
            return _controller.SelectAll();
        }

        public string[] GetColumnNames() => new[] { BOARD_ID_COLUMN_NAME, BOARD_USER_EMAIL_COLUMN_NAME };
        public object[] GetColumnValues() => new object[] { Email, Id };
    }
}