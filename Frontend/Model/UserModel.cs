using Frontend.Controllers;
using Frontend.Utils;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Collections.ObjectModel;

namespace Frontend.Model
{
    public class UserModel : NotifiableModelObject<UserController> 
    {
        private string _email;

        public string Email { get => _email; set { _email = value; RaisePropertyChanged(nameof(Email)); } }
        public ObservableCollection<BoardModel> Boards { get; init; }
        public bool IsDark { get; set; }

        public UserModel(UserController controller, UserSL user) : base(controller)
        {
            _email = user.Email;
            IsDark = user.IsDark;
            Boards = new(user.Boards.Select(b => new BoardModel(ControllerFactory.Instance.BoardController, b)));
        }
    }
}