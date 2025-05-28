using System;
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

        internal int Id
        {
            get => _id;
            set { _controller.Update(BOARD_ID_COLUMN_NAME, _id, BOARD_USER_EMAIL_COLUMN_NAME, _email, BOARD_ID_COLUMN_NAME,
                value); _id = value; }
        }

        internal string Email
        {
            get => _email;
            set { _controller.Update(BOARD_ID_COLUMN_NAME, _id, BOARD_USER_EMAIL_COLUMN_NAME, _email, 
                BOARD_USER_EMAIL_COLUMN_NAME, value); _email = value; }
        }

        internal BoardUserDTO(int id, string email)
        {
            _id = id;
            _email = email;
            _controller = new BoardUserController();
        }

        internal BoardUserDTO()
        {
            _controller = new BoardUserController();
        }

        public BoardUserDTO(int id)
        {
            _id = id;
        }

        public BoardUserDTO(string email)
        {
            _email = email;
        }

        internal void Insert()
        {
            _controller.Insert(this);
        }

        internal void Delete()
        {
            _controller.Delete(BOARD_ID_COLUMN_NAME, _id, BOARD_USER_EMAIL_COLUMN_NAME, _email);
        }

        internal List<BoardUserDTO> SelectAll()
        {
            return _controller.SelectAll();
        }

        public string[] GetColumnNames() => new[] { BOARD_ID_COLUMN_NAME, BOARD_USER_EMAIL_COLUMN_NAME };
        public object[] GetColumnValues() => new object[] { _id, _email };

        internal List<string> GetParticipants()
        {
            return _controller.GetParticipants(BOARD_ID_COLUMN_NAME, _id);
        }

        internal List<int> GetBoards()
        {
            return _controller.GetBoards(BOARD_USER_EMAIL_COLUMN_NAME, _email);
        }
    }
}