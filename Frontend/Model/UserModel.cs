using IntroSE.Kanban.Backend.ServiceLayer;
using System.Collections.ObjectModel;

namespace Frontend.Model
{
    public class UserModel : NotifiableModelObject<UserController> 
    {
        private string _email;
        public string Email { get => _email; set { _email = value; RaisePropertyChanged(nameof(Email)); } }
        public ObservableCollection<BoardModel> Boards { get; init; }

        public UserModel(UserController controller, UserSL user) : base(controller)
        {
            _email = user.Email;
            Boards = new(user.Boards.Select(b => new BoardModel(controller, b)));
        }
    }
}