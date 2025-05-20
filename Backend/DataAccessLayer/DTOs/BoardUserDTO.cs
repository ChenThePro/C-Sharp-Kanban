using IntroSE.Kanban.Backend.DAL;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class BoardUserDTO
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
            set { _controller.Update(_id, value, BOARD_USER_EMAIL_COLUMN_NAME); _email = value; }
        }

        internal BoardUserDTO(string email, int id)
        {
            _controller = new BoardUserController();
            _id = id;
            _email = email;
        }
    }
}