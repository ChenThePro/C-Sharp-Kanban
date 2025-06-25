using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal class BoardUserDTO : IDTO
    {
        private const string ID = "id", EMAIL = "email";

        private int _id;
        private string _email;

        internal int Id
        {
            get => _id;
            set { Update(ID, value); _id = value; }
        }

        internal string Email
        {
            get => _email;
            set { Update(EMAIL, value); _email = value; }
        }

        private readonly BoardUserController _controller;

        internal BoardUserDTO(int id, string email)
        {
            _id = id;
            _email = email;
            _controller = new();
        }

        internal BoardUserDTO(int id)
        {
            _id = id;
            _controller = new();
        }

        internal BoardUserDTO(string email)
        {
            _email = email;
            _controller = new();
        }

        internal void Insert() => _controller.Insert(this);

        internal void Delete() => _controller.Delete(ID, _id, EMAIL, _email);

        internal List<string> GetParticipants() => _controller.GetParticipants(ID, _id);

        internal List<int> GetBoards() => _controller.GetBoards(EMAIL, _email);

        public void Update(string column, object value) => 
            _controller.Update(ID, _id, EMAIL, _email, column, value);

        public string[] GetColumnNames() => new[] { ID, EMAIL };
        
        public object[] GetColumnValues() => new object[] { _id, _email };
    }
}