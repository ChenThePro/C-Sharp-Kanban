using Frontend.Model;

namespace Frontend.ViewModel
{
    internal class UserHomeWindowViewModel : NotifiableObject
    {
        private readonly UserModel _user;
        private readonly BackendController _controller;

        public UserHomeWindowViewModel(UserModel user)
        {
            _user = user;
            _controller = user.Controller;
        }
    }
}
