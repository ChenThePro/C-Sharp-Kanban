using IntroSE.Kanban.Backend.ServiceLayer;
using System.Collections.ObjectModel;

namespace Frontend.Model
{
    public class UserModel : NotifiableModelObject
    {
        private string _email, _password;
        public string Email { get => _email; set { _email = value; RaisePropertyChanged(nameof(Email)); } }
        public string Password { get => _password; set { _password = value; RaisePropertyChanged(nameof(Password)); } }
        public ObservableCollection<BoardModel> Boards { get; init; }

        public UserModel(BackendController controller, UserSL user) : base(controller)
        {
            _email = user.Email;
            _password = user.Password;
            Email = _email;
            Password = _password;
            Boards = new ObservableCollection<BoardModel>(user.Boards.Select(b => new BoardModel(controller, b)));
        }
    }
}